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

    public List<GameObject> slotlist = new List<GameObject>();

    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;

    private GameObject whatSlotToEquip;

    public bool isOpen;

    public bool inventoryUpdated;

    public bool isPop;

    public List<string> itemsPickedup = new List<string>();

    public int stackLimit = 64;

    private bool isUpdateRequired = false;

    public bool isStacked = false;

    //�`���[�g���A���p�̕ϐ�
    [HideInInspector] public bool isMinonget = false;
    [HideInInspector] public bool isHeal = false;

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
                slotlist.Add(child.gameObject);
            }
        }
    }

    void Update()
    {

        //if (inventoryUpdated)
        //{
        ReCalculeList();
        CraftingSystem.Instance.RefreshNeededItems();
        inventoryUpdated = false;
        //}



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
            CraftingSystem.Instance.RefreshNeededItems();

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

        if (isUpdateRequired)
        {
            UpdateInventoryItems();
            isUpdateRequired = false;
        }
    }

    public void UpdateInventoryItems()
    {
        if (!CraftingSystem.Instance.canupdate) return;


        foreach (GameObject slot in slotlist)
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
                    foreach (string itemNameInList in itemList)
                    {
                        if (inventrySlot.itemInSlot.thisName == itemNameInList)
                        {

                            // ��v����A�C�e�������������ꍇ�A���ʂ𑝂₷
                            inventrySlot.SetItemInSlot(); // �A�C�e�������X�V
                            inventrySlot.itemInSlot.amountInventry++;
                            break; // ��v����A�C�e�������������玟�̃A�C�e������
                        }
                    }
                }
            }
        }
    }


    public void AddToinventry(string itemName, bool shoodStack)
    {
        GameObject stack = CheckIfStackExists(itemName);

        if (stack != null && shoodStack)
        {
            isUpdateRequired = true;
            //stack.GetComponent<InventrySlot>().itemInSlot.amountInventry++;
            //stack.GetComponent<InventrySlot>().SetItemInSlot();
        }
        else
        {
            inventoryUpdated = true;
            whatSlotToEquip = FindNextEmptySlot();

            itemName = GetReturnItemName(itemName);
            itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
            itemToAdd.transform.SetParent(whatSlotToEquip.transform);
            if(itemName == "�~�j�I��3" || itemName == "�~�j�I��2" || itemName == "�~�j�I��")
            {
                isMinonget = true;
            }
            itemToAdd.transform.SetAsFirstSibling();
            itemName = GetItemName(itemName);

            if (SelectionManager.Instance.onTarget)
            {
                PopupManager.Instance.TriggerPickupPop(itemName, itemToAdd.GetComponent<Image>().sprite);
            }


            Debug.Log("Adding item: " + itemName + " to itemList");
            
            itemList.Add(itemName);
            Debug.Log("ItemList count after addition: " + itemList.Count);
        }

        if (!SoundManager.Instance.craftingSound.isPlaying)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.PickUpItemSound);
        }

        // �A�C�e���ǉ���Ƀ��X�g���X�V
        //ReCalculeList();
        CraftingSystem.Instance.RefreshNeededItems();
    }

    public void SetStackState(bool state)
    {
        isStacked = state;
    }


    private GameObject CheckIfStackExists(string itemName)
    {
        foreach (GameObject slot in slotlist)
        {
            InventrySlot inventryslot = slot.GetComponent<InventrySlot>();

            inventryslot.SetItemInSlot();

            if (inventryslot != null && inventryslot.itemInSlot != null)
            {

                itemName = GetItemName(itemName);

                if (inventryslot.itemInSlot.thisName == itemName &&
                    inventryslot.itemInSlot.amountInventry < stackLimit)
                {
                    itemName = GetReturnItemName(itemName);
                    return slot;
                }
            }
        }

        return null;
    }

    public string GetItemName(string objectname)
    {
        switch (objectname)
        {
            case "Mana":
                objectname = "�}�i";
                break;
            case "Stone":
                objectname = "�΂���";
                break;
            case "Log":
                objectname = "�ۑ�";
                break;
            case "Axe":
                objectname = "��";
                break;
            case "�~�j�I��3":
                objectname = "�������~�j�I��";
                break;
            case "�������~�j�I��":
                objectname = "�~�j�I��(������)";
                break;
            case "Minion3Seed":
                objectname = "�������~�j�I���̎�";
                break;
            case "Minion2Seed":
                objectname = "�^���N�~�j�I���̎�";
                break;
            case "MinionSeed":
                objectname = "�~�j�I���̎�";
                break;
            case "Mana(Clone)":
                objectname = "�}�i";
                break;
            case "Stone(Clone)":
                objectname = "�΂���";
                break;
            case "Log(Clone)":
                objectname = "�ۑ�";
                break;
            case "Axe(Clone)":
                objectname = "��";
                break;

        }

        return objectname;
    }

    public string GetReturnItemName(string objectname)
    {
        switch (objectname)
        {
            case "�}�i":
                objectname = "Mana";
                break;
            case "�΂���":
                objectname = "Stone";
                break;
            case "�ۑ�":
                objectname = "Log";
                break;
            case "��":
                objectname = "Axe";
                break;
            case "�������~�j�I��":
                objectname = "�~�j�I��3";
                break;
            case "�^���N�~�j�I��":
                objectname = "�~�j�I��2";
                break;
            case "�������~�j�I���̎�":
                objectname = "Minion3Seed";
                break;
            case "�^���N�~�j�I���̎�":
                objectname = "Minion2Seed";
                break;
            case "�~�j�I���̎�":
                objectname = "MinionSeed";
                break;
            case "Stone(Clone)":
                objectname = "Stone";
                break;
            case "Log(Clone)":
                objectname = "Log";
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


    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in slotlist)
        {
            if (slot.transform.childCount <= 1)
            {
                return slot;
            }
        }

        return new GameObject();
    }

    public bool CheckSlotAvailable(int emptyNeeded)
    {
        int emptySlot = 0;

        foreach (GameObject slot in slotlist)
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


    public void RemoveItem(string itemName, int amountToRemove)
    {
        int remainingAmountToRemove = amountToRemove;

        // Iterate through the slot list and reduce inventory as long as we still have items to remove
        foreach (GameObject slot in slotlist)
        {
            if (slot.GetComponent<InventrySlot>() != null && remainingAmountToRemove > 0)
            {
                InventrySlot inventrySlot = slot.GetComponent<InventrySlot>();
                InventoryItem item = inventrySlot.itemInSlot;

                itemName = GetItemName(itemName);
                if (item != null && item.thisName == itemName && item.amountInventry > 0)
                {
                    int amountToDeduct = Mathf.Min(item.amountInventry, remainingAmountToRemove);
                    item.amountInventry -= amountToDeduct;
                    remainingAmountToRemove -= amountToDeduct;

                    // If the amount in the slot becomes 0, destroy the item
                    if (item.amountInventry == 0)
                    {
                        Destroy(item.gameObject);
                        inventrySlot.itemInSlot = null;
                    }

                    itemName = GetReturnItemName(itemName);
                }
            }

            ReCalculeList();
            CraftingSystem.Instance.RefreshNeededItems();

            if (remainingAmountToRemove == 0)
                break; // Exit the loop early if all items are removed
        }

        // After removing items, recalculate the inventory and update crafting

    }


    //public void RemoveItem2(string nameToRemove, int amountToRemove)
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
        itemList.Clear();  // ���X�g���N���A

        foreach (GameObject slot in slotlist)
        {
            if (slot.GetComponent<InventrySlot>())
            {
                InventoryItem item = slot.GetComponent<InventrySlot>().itemInSlot;

                if (item != null && item.amountInventry > 0)
                {
                    // �A�C�e���̃X�^�b�N���������X�g�ɒǉ�
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
            itemName = GetItemName(itemName);
            if (item == itemName)
            {
                count++;
                itemName = GetReturnItemName(itemName);
            }
        }

        return count;
    }
}