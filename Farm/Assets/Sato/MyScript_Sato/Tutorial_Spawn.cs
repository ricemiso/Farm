//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Tutorial_Spawn : MonoBehaviour
//{
//	private GameObject itemToAdd;
//	// Start is called before the first frame update
//	void Start()
//	{
//		// Tree�̃��\�[�X�����[�h
//		SetName("Tree");
//		if (itemToAdd == null)
//		{
//			Debug.LogError("Tree�̃��\�[�X��������܂���I");
//			return;
//		}

//		// Tree�𐶐�
//		for (int i = 0; i < 2; i++)
//		{
//			Vector3 spawnPosition = new Vector3(15.0f, 0.0f, 46.0f + i * 10.0f);
//			GameObject newTree = Instantiate(itemToAdd, spawnPosition, Quaternion.identity);
//			newTree.name = "Tree_" + i;  // ���O�ɘA�Ԃ�ǉ�
//		}

//	}
//	// Update is called once per frame
//	void Update()
//	{}

//	private GameObject SetName(string gameObject)
//	{
//		return itemToAdd = Resources.Load<GameObject>(gameObject);
//	}
//}

using System.Collections;
using UnityEngine;

public class Tutorial_Spawn : MonoBehaviour
{
	private GameObject itemToAdd; // �X�|�[��������GameObject�iEnemy�p�j
	public float spawnInterval = 5.0f; // Enemy�̃X�|�[���Ԋu
	private int enemyCount = 0; // Enemy�̐������J�E���g
	public float spawnRange = 10.0f; // �v���C���[���ӂ̃X�|�[���͈�
	public Transform playerTransform; // �v���C���[��Transform

	public bool hasSpawnAllowed = false; // �����������邩
	public bool isManaHaving;
	public bool isEnemyPresent;
	public ItemCounter manaCount;

	void Start()
	{
		// Tree�̃��\�[�X�����[�h
		SetName("Tree");
		if (itemToAdd == null)
		{
			Debug.LogError("Tree�̃��\�[�X��������܂���I");
			return;
		}

		// Tree�𐶐�
		for (int i = 0; i < 2; i++)
		{
			Vector3 spawnPosition = new Vector3(15.0f, 0.0f, 46.0f + i * 10.0f);
			GameObject newTree = Instantiate(itemToAdd, spawnPosition, Quaternion.identity);
			newTree.name = "Tree_" + i;  // ���O�ɘA�Ԃ�ǉ�
		}

		isManaHaving = true;
		isEnemyPresent = false;

		// �v���C���[��Transform�������I�Ɏ擾
		if (playerTransform == null)
		{
			GameObject player = GameObject.FindGameObjectWithTag("Player"); // "Player" �^�O���g���ăv���C���[������
			if (player != null)
			{
				playerTransform = player.transform; // �v���C���[��Transform��ݒ�
			}
			else
			{
				Debug.LogError("�v���C���[��������܂���B");
			}
		}

		// ������Ԃł̓X�|�[�����J�n���Ȃ�
		hasSpawnAllowed = false;
	}

	void Update()
	{
		// �}�i�̑��݂��m�F
		if (manaCount.counter > 0)
		{
			isManaHaving = true;
			hasSpawnAllowed = true;
		}
		else
		{
			isManaHaving = false;
		}

		// �V�[�����Enemy�I�u�W�F�N�g�����m�F
		isEnemyPresent = GameObject.FindGameObjectsWithTag("Enemy").Length > 0;

		// ����: �}�i��0�œG�����݂��Ȃ��ꍇ�̂݃X�|�[�����J�n
		if (!isManaHaving && !isEnemyPresent && hasSpawnAllowed)
		{
			if(GameObject.FindGameObjectsWithTag("Mana").Length == 0)
			{

				Debug.Log("�}�i�����݂��Ȃ����߁AEnemy�̃X�|�[�����J�n���܂��B");

				SetName("goblin"); // goblin�̃��\�[�X�����[�h
				if (itemToAdd == null)
				{
					Debug.LogError("goblin�̃��\�[�X��������܂���I Resources�t�H���_�ɑ��݂��邩�m�F���Ă��������B");
					return;
				}

				// �v���C���[�̎��ӂŃ����_���ɃX�|�[���ʒu������
				Vector3 spawnPosition = new Vector3(
					playerTransform.position.x + Random.Range(-spawnRange, spawnRange),
					0.0f,
					playerTransform.position.z + Random.Range(-spawnRange, spawnRange)
				);

				// Enemy�𐶐�
				GameObject newEnemy = Instantiate(itemToAdd, spawnPosition, Quaternion.identity);
				newEnemy.name = "Enemy_" + enemyCount; // ���O�ɘA�Ԃ�ǉ�
				newEnemy.tag = "Enemy"; // �^�O�𖾎��I�ɐݒ�
				enemyCount++;
			}

		}

		
	}


	// Resources����w�肵�����O��GameObject�����[�h
	private void SetName(string gameObjectName)
	{
		itemToAdd = Resources.Load<GameObject>(gameObjectName);
	}
}
