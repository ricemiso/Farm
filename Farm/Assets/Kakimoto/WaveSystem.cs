using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

	public List<EnemyStruct> m_EnemyList;

	// 1Wave������̗̑͂̏㏸�l
	public float m_HealthMultiply = 0.0f;
	// 1Wave������̍U���͂̏㏸�l
	public float m_DamageMultiply = 0.0f;
	// 1Wave������̃T�C�Y�̏㏸�l
	public float m_SizeMultiply = 0.0f;


	// �����͈́i���S����͈͂����������̂܂ł̋����j
	[SerializeField] Vector3 Range;

	// ���������G�ɓn���N���X�^���ւ̎Q��
	[SerializeField] GameObject Crystal;

	// �G���X�g�ւ̎Q��
	[SerializeField] GameObject EnemyParent;

	// ���̏��������R�X�g
	[SerializeField] int NoonBase;
	// ����Wave���ɑ����鏢���R�X�g
	[SerializeField] int NoonAdd;
	// ��̏��������R�X�g
	[SerializeField] int NightBase;
	// ���Wave���ɑ����鏢���R�X�g
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

		// ������x�|�C���g�����܂�����G�𐶐�
		// ��
		if (day * 2 - 1 > m_WaveCount && time > 0.5f)
		{
			int cost = NoonBase + m_WaveCount * NoonAdd;
			SummonEnemy(cost);
		}
		// ��
		if (day * 2 - 2 > m_WaveCount)
		{
			int cost = NightBase + m_WaveCount * NightAdd;
			SummonEnemy(cost);
		}

	}


	// ���X�g�����烉���_���ɓG����������
	public void SummonEnemy(int cost)
	{
		m_WaveCount++;

		// �G�̌���
		List<int> enemyIndexList = new List<int>(); // �����ł���G�̃��X�g�inum m_EnemyList�̔ԍ��j
		for (int i = 0; i < m_EnemyList.Count; ++i)
		{
			if (m_WaveCount >= m_EnemyList[i].WaveLimit)
			{
				enemyIndexList.Add(i);
			}
		}
		if (enemyIndexList.Count == 0) return;

		// �X�|�i�[�̌���
		List<int> spawnerIndexList = new List<int>(); // �����ł���X�|�i�[�̃��X�g�inum m_SpawnerList�̔ԍ��j
		for (int i = 0; i < m_SpawnerList.Count; ++i)
		{
			if (m_WaveCount >= m_SpawnerList[i].WaveLimit)
			{
				spawnerIndexList.Add(i);
			}
		}
		if (spawnerIndexList.Count == 0) return;

		// �G�̐�������
		List<int> enemyNumList = new List<int>(); // �G�̏������鐔�̃��X�g�iindex m_EnemyList�̔ԍ��j
		int total = 0;  // ratio�̍��v
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
					// �R�X�g���c���Ă���ꍇ
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

		// �G�̏���

		float health = 1.0f + m_HealthMultiply * m_WaveCount;
		float damage = 1.0f + m_DamageMultiply * m_WaveCount;
		float size = 1.0f + m_SizeMultiply * m_WaveCount;
		for (int index = 0; index < m_EnemyList.Count; ++index)
		{
			for (int cnt = 0; cnt < enemyNumList[index]; cnt++)
			{
				// �X�|�i�[������
				int spawnerIndex = spawnerIndexList[UnityEngine.Random.Range(0, spawnerIndexList.Count)];

				// ���W�̌���
				Vector3 gap;
				gap.x = UnityEngine.Random.Range(-Range.x, Range.x);
				gap.y = UnityEngine.Random.Range(-Range.y, Range.y);
				gap.z = UnityEngine.Random.Range(-Range.z, Range.z);

				// ����
				GameObject obj = Instantiate(m_EnemyList[index].EnemyObject,
					gap + m_SpawnerList[spawnerIndex].Object.transform.position,
					Quaternion.identity);
				obj.transform.SetParent(EnemyParent.transform); // �G���X�g�ɓo�^
				obj.GetComponent<Animal>().maxHealth = (int)(obj.GetComponent<Animal>().maxHealth * health);
				obj.GetComponent<Animal>().damage = (int)(obj.GetComponent<Animal>().damage * damage);
				obj.transform.localScale *= size;

				// �N���X�^���̍��W���Z�b�g
				EnemyAI_Movement ai = obj.GetComponent<EnemyAI_Movement>();
				ai.Crystal = Crystal;
				ai.CrystalMini = m_SpawnerList[spawnerIndex].CrystalMini;
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
	// ���������G�ɓn���~�j�N���X�^���ւ̎Q��
	public GameObject CrystalMini;
}


// ��̃X�|�i�[�̏󋵂��Ǘ�����\����
[System.Serializable]
public class EnemyStruct
{
	// �G�̃I�u�W�F�N�g
	public GameObject EnemyObject;
	// �G�������̃R�X�g
	public int Cost;
	// Wave����
	public int WaveLimit;
	// �G�̏o�������i�����ł���G�S�̂��炱�̐��l�̊����ŏ��������j
	public int Ratio;
}