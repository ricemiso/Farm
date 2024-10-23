using System.Collections;
using System.Collections.Generic;
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
	float m_HealthMultiply;
	// 1Waveあたりのサイズの上昇値
	float m_SizeMultiply;

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
		//float time = DayNightSystem.Instance.currentTimeOfDay;

		// 日付が変更されていたら召喚（要検討）
		if (day > m_WaveCount/* && time >= 0.50f*/)
		{
			m_WaveCount = day;

			// 敵のステータスを決定
			float health = 1.0f + m_HealthMultiply * m_WaveCount;
			float size = 1.0f + m_SizeMultiply * m_WaveCount;

			// 召喚
			while (true)
			{
				// スポーンさせるスポナーの決定
				int num = Random.Range(0, m_SpawnerList.Count);
				if (m_SpawnerList[num].IsActive &&
					m_WaveCount > m_SpawnerList[num].WaveLimit)
				{
					// スポーン
					m_SpawnerList[num].Object.GetComponent<Spawner>().SummonEnemy(3, health, size);
					break;
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