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

		//インベントリにある数を加算
		foreach (GameObject slot in InventorySystem.Instance.slotlist)
		{
			InventrySlot inventrySlot = slot.GetComponent<InventrySlot>();
			if (inventrySlot != null)
			{
				// スロットからアイテムを取得
				inventrySlot.itemInSlot = inventrySlot.CheckInventryItem();

				// スロット内にアイテムがある場合
				if (inventrySlot.itemInSlot != null)
				{
					// itemList 内の各アイテム名とスロット内のアイテム名が一致するか確認
					foreach (string itemNameInList in InventorySystem.Instance.itemList)
					{
						if (inventrySlot.itemInSlot.thisName == itemName)
						{

							// 一致するアイテムが見つかった場合、数量を増やす
							inventrySlot.SetItemInSlot(); // アイテム情報を更新
							counter += inventrySlot.itemInSlot.amountInventry;
							break; // 一致するアイテムが見つかったら次のアイテム名へ
						}
					}


				}
			}
		}

		//クイックスロットにある数を加算
		foreach (GameObject quickSlot in EquipSystem.Instance.quickSlotsList)
		{
			InventrySlot inventrySlot = quickSlot.GetComponent<InventrySlot>();
			if (inventrySlot != null)
			{
				// スロットからアイテムを取得
				inventrySlot.itemInSlot = inventrySlot.CheckInventryItem();

				// スロット内にアイテムがある場合
				if (inventrySlot.itemInSlot != null)
				{
					// itemList 内の各アイテム名とスロット内のアイテム名が一致するか確認
					foreach (string itemNameInList in InventorySystem.Instance.itemList)
					{
						if (inventrySlot.itemInSlot.thisName == itemName)
						{

							// 一致するアイテムが見つかった場合、数量を増やす
							inventrySlot.SetItemInSlot(); // アイテム情報を更新
							counter += inventrySlot.itemInSlot.amountInventry;
							break; // 一致するアイテムが見つかったら次のアイテム名へ
						}
					}


				}

			}
		}

		counterText.text = "× " + counter.ToString();

	}
}
