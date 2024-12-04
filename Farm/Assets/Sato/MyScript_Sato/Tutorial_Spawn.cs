using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Spawn : MonoBehaviour
{
	private GameObject itemToAdd;
	// Start is called before the first frame update
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
		Vector3 spawnPosition = new Vector3(15.0f, 0.0f, 46.0f);
		GameObject newTree = Instantiate(itemToAdd, spawnPosition, Quaternion.identity);
		newTree.name = "Tree_";  // ���O�ɘA�Ԃ�ǉ�

	}
	// Update is called once per frame
	void Update()
	{}

	private GameObject SetName(string gameObject)
	{
		return itemToAdd = Resources.Load<GameObject>(gameObject);
	}
}
