using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventrySlot : MonoBehaviour
{
    public Text amountTXT;
    public InventoryItem itemInSlot;
    public bool quickSlot;

    private void Update()
    {

        InventoryItem item = CheckInventryItem();

        if (item != null)
        {
            Debug.Log("Item found and set in slot: " + item.thisName);
            itemInSlot = item;
        }
        else
        {
            Debug.Log("No item in this slot.");
            itemInSlot = null;
        }




        if (itemInSlot != null && itemInSlot.amountInventry >= 2)
        {
            amountTXT.gameObject.SetActive(true);
            amountTXT.text = $"{itemInSlot.amountInventry}";
            amountTXT.transform.SetAsLastSibling();

        }
        else
        {
            amountTXT.gameObject.SetActive(false);
        }

    }

    public InventoryItem CheckInventryItem()
    {
        foreach (Transform child in transform)
        {
           // InventoryItem item = child.GetComponent<InventoryItem>();
            if (child.GetComponent<InventoryItem>())
            {
                return child.GetComponent<InventoryItem>();
            }
        }

        Debug.Log("No item found in this slot.");
        return null;
    }

    public void SetItemInSlot()
    {
        itemInSlot = CheckInventryItem();
    }
}
