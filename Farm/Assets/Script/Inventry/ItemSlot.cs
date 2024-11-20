using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class ItemSlot : MonoBehaviour, IDropHandler
{

    //public GameObject Item
    //{
    //    get
    //    {
    //        if (transform.childCount > 0)
    //        {
    //            return transform.GetChild(0).gameObject;
    //        }

    //        return null;
    //    }
    //}

    public void OnDrop(PointerEventData eventData)
    {
        InventorySystem.Instance.inventoryUpdated = true;


        //if there is not item already then set our item.
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
            if (dragedItem.thisName == GetStoredItem().thisName && IsLimitExceded(dragedItem) ==false)
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

    InventoryItem GetStoredItem()
    {
        return transform.GetChild(0).GetComponent<InventoryItem>();
    }

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