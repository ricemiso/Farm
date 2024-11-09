using System.Collections;
using System.Collections.Generic;
using System.Numerics;
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

	// 1Waveあたりの体力の上昇値
	public float m_HealthMultiply = 0.0f;
	// 1Waveあたりのサイズの上昇値
	public float m_SizeMultiply = 0.0f;

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

		// 日付が変更されていたら召喚（判定方法は要検討）
		if (day > m_WaveCount && time >= 0.50f)
		{
			m_WaveCount = day;

			// 敵のステータスを決定
			float health = 1.0f + m_HealthMultiply * m_WaveCount;
			float size = 1.0f + m_SizeMultiply * m_WaveCount;
			uint enemyNum = (uint)m_WaveCount;

			// 可動できるスポナーをリスト化
			List<SpawnerStruct> active = new List<SpawnerStruct>();
			List<uint> num = new List<uint>();  // スポナーが召喚する敵の数
			for (int i = 0; i < m_SpawnerList.Count; ++i)
			{
				if (m_SpawnerList[i].IsActive &&
					m_WaveCount >= m_SpawnerList[i].WaveLimit)
				{
					active.Add(m_SpawnerList[i]);
					num.Add(0);
				}
			}

			if (m_SpawnerList.Count > 0)
			{
				// スポナーが召喚する敵の数を決定
				for (uint i = 0; i < enemyNum; ++i)
				{
					int index = Random.Range(0, active.Count);
					num[index]++;
				}

				// 召喚
				for (int i = 0; i < active.Count; ++i)
				{
					active[i].Object.GetComponent<Spawner>().SummonEnemy(num[i], health, size);
				}
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
}