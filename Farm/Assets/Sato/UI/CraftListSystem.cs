using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�S���ҁ@�����M��

public class CraftListSystem : MonoBehaviour
{

	public GameObject craftSlot; //�N���t�g�X���b�g�̃v���n�u
	public RectTransform contentsRect; //�z�u�̈��Rect
	public GameObject canvas; //�z�u�̈�̐e�I�u�W�F�N�g
	public float contentsHeight; //�̈�̍���
	public float changeRange = 150; //todo:�萔����

	private GameObject newSlot; //�V������������X���b�g
	private Vector3 slotPos; //�X���b�g�̈ʒu

	// Start is called before the first frame update
	void Start()
	{
		contentsRect = gameObject.GetComponent<RectTransform>();

		contentsHeight = contentsRect.rect.bottom; //�z�u�̈�̍������擾

		newSlot = null;
		slotPos = new Vector3(400, -100, 0); //�X���b�g�̈ʒu��������
	}

	// Update is called once per frame
	void Update()
	{
		//�e�X�g�p��Enter�L�[����������N���t�g�X���b�g�𐶐�
		if (Input.GetKeyDown(KeyCode.I))
		for(int i = 0; i < 10; i++)
		{
			//newSlot = Instantiate(craftSlot, slotPos, Quaternion.identity); //�X���b�g�𐶐�

			GameObject prefab = (GameObject)Instantiate(craftSlot);
			prefab.transform.SetParent(canvas.transform, false);

			prefab.transform.localPosition = slotPos; //�X���b�g�̈ʒu��ݒ�

			//�z�u�̈�𒷂�����
			contentsHeight -= changeRange;
			contentsRect.offsetMin = new Vector2(0, contentsHeight);
			contentsRect.offsetMax = new Vector2(0, 0);

			slotPos.y -= changeRange; //�X���b�g�̈ʒu�����ɂ��炷
		}
	}
}
