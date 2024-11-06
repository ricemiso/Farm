using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventrySlot : MonoBehaviour
{
    public Text amountTXT;
    public InventoryItem itemInSlot;

    private void Update()
    {
        InventoryItem item = CheckInventryItem();

        if(item != null)
        {
            itemInSlot = item;
        }
        else
        {
            itemInSlot = null;
        }

        if(itemInSlot != null)
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

    private InventoryItem CheckInventryItem()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<InventoryItem>())
            {
                return child.GetComponent<InventoryItem>();
            }
        }

        return null;
    }

    public void SetItemInSlot()
    {
        itemInSlot = CheckInventryItem();
    }
}
