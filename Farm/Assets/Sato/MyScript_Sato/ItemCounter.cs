using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemCounter : MonoBehaviour
{
	public Text counterText;
	public string itemName;
	public int counter;



	// Start is called before the first frame update
	void Start()
    {
		counter = 0;
	}

	// Update is called once per frame
	void Update()
    {
		counter = 0;

		//�C���x���g���ɂ��鐔�����Z
		foreach (GameObject slot in InventorySystem.Instance.slotlist)
		{
			InventrySlot inventrySlot = slot.GetComponent<InventrySlot>();
			if (inventrySlot != null)
			{
				// �X���b�g����A�C�e�����擾
				inventrySlot.itemInSlot = inventrySlot.CheckInventryItem();

				// �X���b�g���ɃA�C�e��������ꍇ
				if (inventrySlot.itemInSlot != null)
				{
					// itemList ���̊e�A�C�e�����ƃX���b�g���̃A�C�e��������v���邩�m�F
					foreach (string itemNameInList in InventorySystem.Instance.itemList)
					{
						if (inventrySlot.itemInSlot.thisName == itemName)
						{

							// ��v����A�C�e�������������ꍇ�A���ʂ𑝂₷
							inventrySlot.SetItemInSlot(); // �A�C�e�������X�V
							counter += inventrySlot.itemInSlot.amountInventry;
							break; // ��v����A�C�e�������������玟�̃A�C�e������
						}
					}


				}
			}
		}

		//�N�C�b�N�X���b�g�ɂ��鐔�����Z
		foreach (GameObject quickSlot in EquipSystem.Instance.quickSlotsList)
		{
			InventrySlot inventrySlot = quickSlot.GetComponent<InventrySlot>();
			if (inventrySlot != null)
			{
				// �X���b�g����A�C�e�����擾
				inventrySlot.itemInSlot = inventrySlot.CheckInventryItem();

				// �X���b�g���ɃA�C�e��������ꍇ
				if (inventrySlot.itemInSlot != null)
				{
					// itemList ���̊e�A�C�e�����ƃX���b�g���̃A�C�e��������v���邩�m�F
					foreach (string itemNameInList in InventorySystem.Instance.itemList)
					{
						if (inventrySlot.itemInSlot.thisName == itemName)
						{

							// ��v����A�C�e�������������ꍇ�A���ʂ𑝂₷
							inventrySlot.SetItemInSlot(); // �A�C�e�������X�V
							counter += inventrySlot.itemInSlot.amountInventry;
							break; // ��v����A�C�e�������������玟�̃A�C�e������
						}
					}


				}

			}
		}

		counterText.text = "�~ " + counter.ToString();

	}
}
