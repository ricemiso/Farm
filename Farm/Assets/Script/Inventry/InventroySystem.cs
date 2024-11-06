using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{

    public GameObject ItemInfoUI;

    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;

    public List<InventrySlot> slotlist = new List<InventrySlot>();

    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;

    private InventrySlot whatSlotToEquip;

    public bool isOpen;

    public bool inventoryUpdated;

    public bool isPop;

    public List<string> itemsPickedup = new List<string>();

    public int stackLimit = 64;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    void Start()
    {
        isOpen = false;
        inventoryUpdated = false;

        PopulateSlotList();

        Cursor.visible = false;
        stackLimit = 64;
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                InventrySlot slot = child.GetComponent<InventrySlot>();
                slotlist.Add(slot);
            }
        }
    }

    void Update()
    {

        if (inventoryUpdated)
        {
            ReCalculeList();
            CraftingSystem.Instance.RefreshNeededItems();
            inventoryUpdated = false;
        }


        if (Input.GetKeyDown(KeyCode.I) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
        {

            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;
            ReCalculeList();

        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);

            if (!CraftingSystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                SelectionManager.Instance.EnableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            }

            isOpen = false;
        }
    }

    public void AddToinventry(string itemName, bool shoodStack)
    {

        InventrySlot stack = CheckIfStackExists(itemName);

        if (stack != null && shoodStack)
        {
            stack.itemInSlot.amountInventry++;
            stack.SetItemInSlot();
        }
        else
        {
            inventoryUpdated = true;
            whatSlotToEquip = FindNextEmptySlot();

            itemName = GetReturnItemName(itemName);
            itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
            itemToAdd.transform.SetParent(whatSlotToEquip.transform);
            itemName = GetItemName(itemName);
            if (SelectionManager.Instance.onTarget)
            {
                PopupManager.Instance.TriggerPickupPop(itemName, itemToAdd.GetComponent<Image>().sprite);
            }


            itemList.Add(itemName);
        }




        if (!SoundManager.Instance.craftingSound.isPlaying)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.PickUpItemSound);
        }


        ReCalculeList();
        CraftingSystem.Instance.RefreshNeededItems();


    }

    private InventrySlot CheckIfStackExists(string itemName)
    {
        foreach (InventrySlot inventryslot in slotlist)
        {

            inventryslot.SetItemInSlot();

            if (inventryslot != null && inventryslot.itemInSlot != null)
            {
                Debug.Log(itemName);
                itemName = GetItemName(inventryslot.itemInSlot.thisName);
                Debug.Log(itemName);
                if (inventryslot.itemInSlot.thisName == itemName &&
                    inventryslot.itemInSlot.amountInventry < stackLimit)
                {
                    itemName = GetReturnItemName(itemName);
                    Debug.Log(itemName);
                    return inventryslot;
                }
            }
        }

        return null;
    }

    private string GetItemName(string objectname)
    {
        switch (objectname)
        {
            case "Mana":
                objectname = "マナ";
                break;
            case "Stone":
                objectname = "石ころ";
                break;
            case "Log":
                objectname = "丸太";
                break;

        }

        return objectname;
    }

    private string GetReturnItemName(string objectname)
    {
        switch (objectname)
        {
            case "マナ":
                objectname = "Mana";
                break;
            case "Stone":
                objectname = "石ころ";
                break;
            case "Log":
                objectname = "丸太";
                break;
            case "Stone(Clone)":
                objectname = "石ころ";
                break;
            case "Log(Clone)":
                objectname = "丸太";
                break;

        }

        return objectname;
    }

    public void LoadToinventry(string itemName)
    {


        inventoryUpdated = true;
        whatSlotToEquip = FindNextEmptySlot();
        Debug.Log(itemName);
        if (itemName == "TomatoSeed")
        {
            itemName = "MinionSeed";
        }
        itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
        itemToAdd.transform.SetParent(whatSlotToEquip.transform);

        itemList.Add(itemName);



        ReCalculeList();
        CraftingSystem.Instance.RefreshNeededItems();

    }


    private InventrySlot FindNextEmptySlot()
    {
        foreach (InventrySlot slot in slotlist)
        {
            if (slot.transform.childCount <= 1)
            {
                return slot;
            }
        }

        return new InventrySlot();
    }

    public bool CheckSlotAvailable(int emptyNeeded)
    {
        int emptySlot = 0;

        foreach (InventrySlot slot in slotlist)
        {
            if (slot.transform.childCount <= 1)
            {
                emptySlot += 1;
            }

        }

        if (emptySlot >= emptyNeeded)
        {
            return true;
        }
        else
        {
            return false;
        }

    }


    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int remainingAmountToRemove = amountToRemove;

        while (remainingAmountToRemove != 0)
        {

        }
    }

    //public void RemoveItem(string nameToRemove, int amountToRemove)
    //{
    //    inventoryUpdated = true;
    //    int counter = amountToRemove;

    //    for (var i = slotlist.Count - 1; i >= 0; i--)
    //    {
    //        if (slotlist[i].transform.childCount > 0)
    //        {
    //            if (slotlist[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter != 0)
    //            {
    //                Destroy(slotlist[i].transform.GetChild(0).gameObject);

    //                counter--;
    //            }
    //        }
    //    }
    //}

    public void ReCalculeList()
    {
        itemList.Clear();

        foreach (InventrySlot inventoryslot in slotlist)
        {

            InventoryItem item = inventoryslot.GetComponent<InventrySlot>().itemInSlot;

            if (item != null)
            {

                if (item.amountInventry > 0)
                {
                    for (int i = 0; i < item.amountInventry; i++)
                    {
                        itemList.Add(item.thisName);
                    }
                }

            }

        }
    }

    public int GetItemCount(string itemName)
    {
        int count = 0;

        foreach (var item in itemList)
        {
            if (item == itemName)
            {
                count++;
            }
        }

        return count;
    }
}