using System.Collections;
using System.Collections.Generic;
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
	float m_HealthMultiply;
	// 1Wave������̃T�C�Y�̏㏸�l
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

		// ���t���ύX����Ă����珢���i�v�����j
		if (day > m_WaveCount/* && time >= 0.50f*/)
		{
			m_WaveCount = day;

			// �G�̃X�e�[�^�X������
			float health = 1.0f + m_HealthMultiply * m_WaveCount;
			float size = 1.0f + m_SizeMultiply * m_WaveCount;

			// ����
			while (true)
			{
				// �X�|�[��������X�|�i�[�̌���
				int num = Random.Range(0, m_SpawnerList.Count);
				if (m_SpawnerList[num].IsActive &&
					m_WaveCount > m_SpawnerList[num].WaveLimit)
				{
					// �X�|�[��
					m_SpawnerList[num].Object.GetComponent<Spawner>().SummonEnemy(3, health, size);
					break;
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