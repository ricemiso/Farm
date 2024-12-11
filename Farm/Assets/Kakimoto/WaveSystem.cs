using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
// 
// 敵を召喚するウェーブを管理するクラス
// 
//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


public class WaveSystem : MonoBehaviour
{
	public static WaveSystem Instance { get; set; }

	// 現在のウェーブ
	public int m_WaveCount;


	// スポナーを管理するリスト
	public List<SpawnerStruct> m_SpawnerList;

	public List<EnemyStruct> m_EnemyList;

	// 1Waveあたりの体力の上昇値
	public float m_HealthMultiply = 0.0f;
	// 1Waveあたりの攻撃力の上昇値
	public float m_DamageMultiply = 0.0f;
	// 1Waveあたりのサイズの上昇値
	public float m_SizeMultiply = 0.0f;


	// 召喚範囲（中心から範囲を示す直方体までの距離）
	[SerializeField] Vector3 Range;

	// 召喚した敵に渡すクリスタルへの参照
	[SerializeField] GameObject Crystal;

	// 敵リストへの参照
	[SerializeField] GameObject EnemyParent;

	// 昼の初期召喚コスト
	[SerializeField] int NoonBase;
	// 昼のWave毎に増える召喚コスト
	[SerializeField] int NoonAdd;
	// 夜の初期召喚コスト
	[SerializeField] int NightBase;
	// 夜のWave毎に増える召喚コスト
	[SerializeField] int NightAdd;


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		m_WaveCount = 0;

	}

	// Update is called once per frame
	void Update()
	{
		int day = TimeManager.Instance.dayInGame;
		float time = DayNightSystem.Instance.currentTimeOfDay;

		// ある程度ポイントが溜まったら敵を生成
		// 昼
		if (day * 2 - 1 > m_WaveCount && time > 0.5f)
		{
			int cost = NoonBase + m_WaveCount * NoonAdd;
			SummonEnemy(cost);
		}
		// 夜
		if (day * 2 - 2 > m_WaveCount)
		{
			int cost = NightBase + m_WaveCount * NightAdd;
			SummonEnemy(cost);
		}

	}


	// リスト内からランダムに敵を召喚する
	public void SummonEnemy(int cost)
	{
		m_WaveCount++;

		// 敵の決定
		List<int> enemyIndexList = new List<int>(); // 召喚できる敵のリスト（num m_EnemyListの番号）
		for (int i = 0; i < m_EnemyList.Count; ++i)
		{
			if (m_WaveCount >= m_EnemyList[i].WaveLimit)
			{
				enemyIndexList.Add(i);
			}
		}
		if (enemyIndexList.Count == 0) return;

		// スポナーの決定
		List<int> spawnerIndexList = new List<int>(); // 召喚できるスポナーのリスト（num m_SpawnerListの番号）
		for (int i = 0; i < m_SpawnerList.Count; ++i)
		{
			if (m_WaveCount >= m_SpawnerList[i].WaveLimit)
			{
				spawnerIndexList.Add(i);
			}
		}
		if (spawnerIndexList.Count == 0) return;

		// 敵の数を決定
		List<int> enemyNumList = new List<int>(); // 敵の召喚する数のリスト（index m_EnemyListの番号）
		int total = 0;  // ratioの合計
		for (int i = 0; i < m_EnemyList.Count; ++i)
		{
			enemyNumList.Add(0);
			total += m_EnemyList[i].Ratio;
		}

		int missCount = 0;
		int costRemains = cost;
		while (missCount <= 8)
		{
			int random = UnityEngine.Random.Range(0, total);

			int currentWeight = 0;
			for (int i = 0; i < enemyIndexList.Count; i++)
			{
				currentWeight += m_EnemyList[enemyIndexList[i]].Ratio;
				if (random < currentWeight)
				{
					// コストが残っている場合
					if (costRemains >= m_EnemyList[enemyIndexList[i]].Ratio)
					{
						costRemains -= m_EnemyList[enemyIndexList[i]].Cost;
						enemyNumList[i]++;
						missCount = 0;
					}
					else
					{
						missCount++;
					}
				}
			}
		}

		// 敵の召喚

		float health = 1.0f + m_HealthMultiply * m_WaveCount;
		float damage = 1.0f + m_DamageMultiply * m_WaveCount;
		float size = 1.0f + m_SizeMultiply * m_WaveCount;
		for (int index = 0; index < m_EnemyList.Count; ++index)
		{
			for (int cnt = 0; cnt < enemyNumList[index]; cnt++)
			{
				// スポナーを決定
				int spawnerIndex = spawnerIndexList[UnityEngine.Random.Range(0, spawnerIndexList.Count)];

				// 座標の決定
				Vector3 gap;
				gap.x = UnityEngine.Random.Range(-Range.x, Range.x);
				gap.y = UnityEngine.Random.Range(-Range.y, Range.y);
				gap.z = UnityEngine.Random.Range(-Range.z, Range.z);

				// 召喚
				GameObject obj = Instantiate(m_EnemyList[index].EnemyObject,
					gap + m_SpawnerList[spawnerIndex].Object.transform.position,
					Quaternion.identity);
				obj.transform.SetParent(EnemyParent.transform); // 敵リストに登録
				obj.GetComponent<Animal>().maxHealth = (int)(obj.GetComponent<Animal>().maxHealth * health);
				obj.GetComponent<Animal>().damage = (int)(obj.GetComponent<Animal>().damage * damage);
				obj.transform.localScale *= size;

				// クリスタルの座標をセット
				EnemyAI_Movement ai = obj.GetComponent<EnemyAI_Movement>();
				ai.Crystal = Crystal;
				ai.CrystalMini = m_SpawnerList[spawnerIndex].CrystalMini;
			}
		}


	}
}

// 一つのスポナーの状況を管理する構造体
[System.Serializable]
public class SpawnerStruct
{
	// スポナーのオブジェクト
	public GameObject Object;
	// 起動可能か
	public bool IsActive;
	// Wave制限
	public int WaveLimit;
	// 召喚した敵に渡すミニクリスタルへの参照
	public GameObject CrystalMini;
}


// 一つのスポナーの状況を管理する構造体
[System.Serializable]
public class EnemyStruct
{
	// 敵のオブジェクト
	public GameObject EnemyObject;
	// 敵召喚時のコスト
	public int Cost;
	// Wave制限
	public int WaveLimit;
	// 敵の出現割合（召喚できる敵全体からこの数値の割合で召喚される）
	public int Ratio;
}