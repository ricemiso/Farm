using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
// 
// �G����������E�F�[�u���Ǘ�����N���X
// 
//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


public class WaveSystem : MonoBehaviour
{
	public static WaveSystem Instance { get; set; }

	// ���݂̃E�F�[�u
	public int m_WaveCount;


	// �X�|�i�[���Ǘ����郊�X�g
	public List<SpawnerStruct> m_SpawnerList;

	// 1Wave������̗̑͂̏㏸�l
	public float m_HealthMultiply = 0.0f;
	// 1Wave������̃T�C�Y�̏㏸�l
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

		// ���t���ύX����Ă����珢���i������@�͗v�����j
		if (day > m_WaveCount && time >= 0.50f)
		{
			m_WaveCount = day;

			// �G�̃X�e�[�^�X������
			float health = 1.0f + m_HealthMultiply * m_WaveCount;
			float size = 1.0f + m_SizeMultiply * m_WaveCount;
			uint enemyNum = (uint)m_WaveCount;

			// ���ł���X�|�i�[�����X�g��
			List<SpawnerStruct> active = new List<SpawnerStruct>();
			List<uint> num = new List<uint>();  // �X�|�i�[����������G�̐�
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
				// �X�|�i�[����������G�̐�������
				for (uint i = 0; i < enemyNum; ++i)
				{
					int index = Random.Range(0, active.Count);
					num[index]++;
				}

				// ����
				for (int i = 0; i < active.Count; ++i)
				{
					active[i].Object.GetComponent<Spawner>().SummonEnemy(num[i], health, size);
				}
			}

		}



	}
}

// ��̃X�|�i�[�̏󋵂��Ǘ�����\����
[System.Serializable]
public class SpawnerStruct
{
	// �X�|�i�[�̃I�u�W�F�N�g
	public GameObject Object;
	// �N���\��
	public bool IsActive;
	// Wave����
	public int WaveLimit;
}