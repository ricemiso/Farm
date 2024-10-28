using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }

    // -- UI -- //
    public GameObject quickSlotsPanel;

    public List<GameObject> quickSlotsList = new List<GameObject>();

    public GameObject numbersHolder;


    public int selectedNumber = -1;
    public GameObject selectedItem;

    public GameObject toolHolder;

    public GameObject selecteditemModel;


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


    private void Start()
    {

        PopulateSlotList();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectQuickSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectQuickSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectQuickSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectQuickSlot(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectQuickSlot(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectQuickSlot(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SelectQuickSlot(7);
        }
    }


    public void SelectQuickSlot(int number)
    {
        if (checkIfSlotIsFull(number) == true)
        {
            if (selectedNumber != number)
            {

                selectedNumber = number;

                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                }

                selectedItem = GetSelectedItem(number);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;


                SetEquippedModel(selectedItem);

                foreach (Transform child in numbersHolder.transform)
                {
                    child.transform.Find("Text").GetComponent<Text>().color = Color.gray;
                }

                Text toBeChange = numbersHolder.transform.Find("number" + number).transform.Find("Text").GetComponent<Text>();
                toBeChange.color = Color.white;
            }
            else
            {
                selectedNumber = -1;

                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                    selectedItem = null;
                }

                if (selecteditemModel != null)
                {
                    DestroyImmediate(selecteditemModel.gameObject);
                    selecteditemModel = null;
                }


                foreach (Transform child in numbersHolder.transform)
                {
                    child.transform.Find("Text").GetComponent<Text>().color = Color.gray;
                }
            }
        }
    }

    internal int GetWeaPonDamage()
    {
        if (selectedItem != null)
        {
            return selectedItem.GetComponent<Weapon>().weaponDamage;
        }
        else
        {
            return 0;
        }
    }

    
    public bool IsPlayerHooldingSeed()
    {
        if (selecteditemModel != null)
        {
            switch (selecteditemModel.gameObject.name)
            {
                case "Hand_model(Clone)":
                    return true;
                default:
                    return false;
            }
        }
        else
        {
            return false;
        }
    }

    internal bool IsPlayerHooldingWateringCan()
    {
        if (selectedItem != null)
        {
            switch (selectedItem.GetComponent<InventoryItem>().thisName)
            {
                case "じょうろ":
                    return true;

                default:
                    return false;
            }
        }
        else
        {
            return false;
        }
    }

    internal bool IsPlayerHooldingMana()
    {
        if (selecteditemModel != null)
        {
            switch (selecteditemModel.gameObject.name)
            {
                case "Mana_model(Clone)":
                    return true;
                default:
                    return false;
            }
        }
        else
        {
            return false;
        }
    }

    internal bool IsHoldingWeapon()
    {
        if (selectedItem != null)
        {
            if (selectedItem.GetComponent<Weapon>() != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    //TODO:手に持たせるモデルの追加
    private void SetEquippedModel(GameObject selectedItem)
    {
        if (selecteditemModel != null)
        {
            DestroyImmediate(selecteditemModel.gameObject);
            selecteditemModel = null;
        }

        string selectedItemName = selectedItem.name.Replace("(Clone)", "");
        Debug.Log("Selected Item Name: " + selectedItemName);

        selecteditemModel = Instantiate(Resources.Load<GameObject>(CaculateItemModel(selectedItemName)));

        selecteditemModel.transform.SetParent(toolHolder.transform, false);
    }

    //TODO:持つ武器はここで追加する(位置、回転はプレハブの座標で!!)
    //TODO:種が増えるごとにここに追加する
    private string CaculateItemModel(string selectedItemName)
    {
        switch (selectedItemName)
        {
            case "Axe":
                //TODO:音を後で直す
                SoundManager.Instance.PlaySound(SoundManager.Instance.PutSeSound);
                return "Axe_model";

            case "Stone":
                return "Stone_model";

            case "Pickaxe":
                //TODO:音を後で直す
                SoundManager.Instance.PlaySound(SoundManager.Instance.PutSeSound);
                return "Pickaxe_model";

            case "MinionSeed":
                return "Hand_model";

            case "Minion2Seed":
                return "Hand_model";

            case "WateringCan":
                return "WateringCan_model";

            case "Mana":
                return "Mana_model";
            default:
                return null;

        }

    }

    

    public bool IsThereSwingLock()
    {

        if (selecteditemModel && selecteditemModel.GetComponent<EquiableItem>())
        {
            Debug.Log("SwingWait in IsThereSwingLock: " + selecteditemModel.GetComponent<EquiableItem>().SwingWait);
            return selecteditemModel.GetComponent<EquiableItem>().SwingWait;
        }
        else
        {
            return false;
        }
    }

    GameObject GetSelectedItem(int slotnumber)
    {
        return quickSlotsList[slotnumber - 1].transform.GetChild(0).gameObject;
    }


    bool checkIfSlotIsFull(int slotNumber)
    {
        if (quickSlotsList[slotNumber - 1].transform.childCount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
        }
    }

    public void AddToQuickSlots(GameObject itemToEquip)
    {

        GameObject availableSlot = FindNextEmptySlot();

        itemToEquip.transform.SetParent(availableSlot.transform, false);

        InventorySystem.Instance.ReCalculeList();

    }


    public GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {

        int counter = 0;

        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }

        if (counter == 7)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveItemFromQuickSlots(string itemName, int amountToRemove)
    {
        int counter = amountToRemove;

        for (var i = quickSlotsList.Count - 1; i >= 0; i--)
        {
            if (quickSlotsList[i].transform.childCount > 0)
            {
                if (quickSlotsList[i].transform.GetChild(0).name == itemName + "(Clone)" && counter > 0)
                {
                    Destroy(quickSlotsList[i].transform.GetChild(0).gameObject);
                    counter--;
                }
            }

            // 必要な数だけ削除できた場合はループを抜ける
            if (counter <= 0)
                break;
        }
    }
}