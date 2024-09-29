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

    public GameObject pickupAlert;
    public Text pickupName;
    public Image pickupImage;

    public bool inventoryUpdated;


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

        if (inventoryUpdated)
        {
            ReCalculeList();
            CraftingSystem.Instance.RefreshNeededItems();
            inventoryUpdated = false; 
        }


        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {

            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.instance.DisableSelection();
            SelectionManager.instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);

            if (!CraftingSystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                SelectionManager.instance.EnableSelection();
                SelectionManager.instance.GetComponent<SelectionManager>().enabled = true;
            }
            
            isOpen = false;
        }
    }

    public void AddToinventry(string itemName)
    {
        if (!SoundManager.Instance.craftingSound.isPlaying)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.PickUpItemSound);
        }
        


        inventoryUpdated = true;
        whatSlotToEquip = FindNextEmptySlot();

        itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
        itemToAdd.transform.SetParent(whatSlotToEquip.transform);

        itemList.Add(itemName);

        TriggerPickupPop(itemName,itemToAdd.GetComponent<Image>().sprite);

        ReCalculeList();
        CraftingSystem.Instance.RefreshNeededItems();
    }


    void TriggerPickupPop(string itemName,Sprite itemSprite)
    {
        pickupAlert.SetActive(true);
        pickupName.text = itemName;
        pickupImage.sprite = itemSprite;

        StartCoroutine(PopHide());
    }

    public IEnumerator PopHide()
    {
        yield return new WaitForSeconds(1.0f);

        pickupAlert.SetActive(false);
    }


    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in slotlist)
        {
            if (slot.transform.childCount == 0)
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
            if (slot.transform.childCount <= 0)
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

    public void RemoveItem(string nameToRemove,int amountToRemove)
    {
        inventoryUpdated = true;
        int counter = amountToRemove;

        for (var i = slotlist.Count - 1; i >= 0; i--)
        {
            if (slotlist[i].transform.childCount > 0) 
            {
                if (slotlist[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter != 0) 
                {
                    Destroy(slotlist[i].transform.GetChild(0).gameObject);

                    counter --;
                }
            }
        }
    }

    public void ReCalculeList()
    {
        itemList.Clear();

        foreach (GameObject slot in slotlist)
        {
            if (slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name;

                string str2 = "(Clone)";

                string result = name.Replace(str2, "");

                itemList.Add(result);
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