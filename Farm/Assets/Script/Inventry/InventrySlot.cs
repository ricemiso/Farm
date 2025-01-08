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
    [SerializeField] private GameObject alphaobject;

    private void Update()
    {

        InventoryItem item = CheckInventryItem();

        if (item != null)
        {
            itemInSlot = item;
            if(alphaobject != null)
            {
                alphaobject.SetActive(false);
            }
            
        }
        else
        {
            itemInSlot = null;
            if (alphaobject != null)
            {
                alphaobject.SetActive(true);
            }
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
        return null;
    }

    public void SetItemInSlot()
    {
        itemInSlot = CheckInventryItem();
    }
}
