using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//担当者　越浦晃生

/// <summary>
/// アイテムスロットを管理するクラス。
/// </summary>
public class ItemSlot : MonoBehaviour, IDropHandler
{
    /// <summary>
    /// ドロップされたアイテムを処理する関数。
    /// </summary>
    /// <param name="eventData">イベントデータ</param>
    public void OnDrop(PointerEventData eventData)
    {
        InventorySystem.Instance.inventoryUpdated = true;

        if (transform.childCount <= 1)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.dropItemSound);

            DragDrop.itemBeingDragged.transform.SetParent(transform);
            DragDrop.itemBeingDragged.transform.localPosition = new Vector2(0, 0);

            if (transform.CompareTag("QuickSlot") == false)
            {
                DragDrop.itemBeingDragged.GetComponent<InventoryItem>().isInsideQuiqSlot = false;
                InventorySystem.Instance.ReCalculeList();
                CraftingSystem.Instance.RefreshNeededItems();
            }

            if (transform.CompareTag("QuickSlot"))
            {
                DragDrop.itemBeingDragged.GetComponent<InventoryItem>().isInsideQuiqSlot = true;
                InventorySystem.Instance.ReCalculeList();
                CraftingSystem.Instance.RefreshNeededItems();
            }
        }
        else
        {
            InventoryItem dragedItem = DragDrop.itemBeingDragged.GetComponent<InventoryItem>();

            var itemName = InventorySystem.Instance.GetItemName(dragedItem.thisName);
            if (dragedItem.thisName == GetStoredItem().thisName && IsLimitExceded(dragedItem) == false)
            {
                GetStoredItem().amountInventry += dragedItem.amountInventry;
                DestroyImmediate(DragDrop.itemBeingDragged);
            }
            else
            {
                DragDrop.itemBeingDragged.transform.SetParent(transform);
            }
        }
    }

    /// <summary>
    /// スロット内に保存されているアイテムを取得する関数。
    /// </summary>
    /// <returns>インベントリアイテム</returns>
    InventoryItem GetStoredItem()
    {
        return transform.GetChild(0).GetComponent<InventoryItem>();
    }

    /// <summary>
    /// アイテムのスタック制限を超えているかどうかを確認する関数。
    /// </summary>
    /// <param name="draggItem">ドラッグされたアイテム</param>
    /// <returns>制限を超えている場合はtrue、それ以外はfalse</returns>
    bool IsLimitExceded(InventoryItem draggItem)
    {
        if ((draggItem.amountInventry + GetStoredItem().amountInventry) > InventorySystem.Instance.stackLimit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
