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
    public GameObject selectedMinion;

    public int stackcnt;
    public int selectednumber;

    public bool SwingWait;

    public bool selectMinion;
    public bool selectMana;
    public bool selectedManamodel;

    public GameObject currentSelectedObject;

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
        SwingWait = false;
        selectMinion = false;
        selectedManamodel = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectQuickSlot(1);
            selectednumber = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectQuickSlot(2);
            selectednumber = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectQuickSlot(3);
            selectednumber = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectQuickSlot(4);
            selectednumber = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectQuickSlot(5);
            selectednumber = 5;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectQuickSlot(6);
            selectednumber = 6;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SelectQuickSlot(7);
            selectednumber = 7;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SelectQuickSlot(8);
            selectednumber = 8;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SelectQuickSlot(9);
            selectednumber = 9;
        }


        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)  
        {
            CycleQuickSlot(1);
            
        }
        else if (scroll < 0f)  
        {
            CycleQuickSlot(-1);  
        }

        UpdateCurrentSelectedObject();
    }

    private void UpdateCurrentSelectedObject()
    {
        if (selectedNumber > 0 && selectedNumber <= quickSlotsList.Count)
        {
            GameObject selectedSlot = quickSlotsList[selectedNumber - 1];
            if (selectedSlot.transform.childCount > 0)
            {
                currentSelectedObject = selectedSlot.transform.GetChild(0).gameObject;
            }
            else
            {
                currentSelectedObject = null; // �X���b�g����̏ꍇ
            }
        }
        else
        {
            currentSelectedObject = null; // �I������Ă��Ȃ��ꍇ
        }
    }

    // �N�C�b�N�X���b�g��؂�ւ���
    private void CycleQuickSlot(int direction)
    {
        int currentSlot = GetCurrentQuickSlot();
        int newSlot = currentSlot + direction;

       
        if (newSlot < 1)
        {
            newSlot = 9; 
        }
        else if (newSlot > 9)
        {
            newSlot = 1;
        }
        selectednumber = newSlot;


        SelectQuickSlot(newSlot);
    }

    // ���ݑI������Ă���N�C�b�N�X���b�g���擾����
    public int GetCurrentQuickSlot()
    {
        if(selectedNumber <=1)
        {
            selectedNumber = 1;
        }

        return selectedNumber;

    }


    public void SelectQuickSlot(int number)
    {
        if (checkIfSlotIsFull(number) == true)
        {
            if (selectedNumber != number)
            {

                selectedNumber = number;


                ConstructionManager.Instance.inConstructionMode = false;

                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                }

                selectedItem = GetSelectedItem(number);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;


                if(selectedItem.name == "�~�j�I��"||selectedItem.name == "�~�j�I��2"|| selectedItem.name == "�~�j�I��3"||
                    selectedItem.name == "�~�j�I��(Clone)" || selectedItem.name == "�~�j�I��2(Clone)" || selectedItem.name == "�~�j�I��3(Clone)")
                {
                    selectedMinion = selectedItem;
                    selectMinion = true;
                    selectMana = false;


                }
                else if(selectedItem.name == "Mana"|| selectedItem.name == "�}�i")
                {
                    selectMana = true;
                    selectMinion = false;
                }
                else
                {
                    selectMinion = false;
                    selectMana = false;
                }


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

    public void UseItem(GameObject obj)
    {
        InventorySystem.Instance.isOpen = false;
        InventorySystem.Instance.inventoryScreenUI.SetActive(false);

        CraftingSystem.Instance.isOpen = false;
        CraftingSystem.Instance.craftingScreenUI.SetActive(false);
        CraftingSystem.Instance.toolScreenUI.SetActive(false);
        CraftingSystem.Instance.survivalScreenUI.SetActive(false);
        CraftingSystem.Instance.refineScreenUI.SetActive(false);
        CraftingSystem.Instance.constractionScreenUI.SetActive(false);


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SelectionManager.Instance.EnableSelection();
        SelectionManager.Instance.enabled = true; ;

        if (gameObject != null && !gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }


        //TODO:�z�u�I�u�W�F�N�g�̒ǉ�
        switch (obj.name)
        {
            case "Foundation":
                ConstructionManager.Instance.ActivateConstructionPlacement("FoundationModel");
                break;
            case "Wall":
                ConstructionManager.Instance.ActivateConstructionPlacement("WallModel");
                break;
            case "�~�j�I��":
                ConstructionManager.Instance.ActivateConstructionPlacement("ConstractAI2");
                break;
            case "�~�j�I��2":
                ConstructionManager.Instance.ActivateConstructionPlacement("TankAI2");
                break;
            case "�~�j�I��(�^���N)":
                ConstructionManager.Instance.ActivateConstructionPlacement("TankAI2");
                break;
            case "�~�j�I��3":
                ConstructionManager.Instance.ActivateConstructionPlacement("LongRangeMinion 1");
                break;
            case "�~�j�I��(������)":
                ConstructionManager.Instance.ActivateConstructionPlacement("LongRangeMinion 1");
                break;
            case "Stairs":
                ConstructionManager.Instance.ActivateConstructionPlacement("StairsWoodemodel");
                break;
            case "Chest":
                ConstructionManager.Instance.ActivateConstructionPlacement("Chestmodel");
                break;
            case "Foundation(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("FoundationModel");
                break;
            case "Wall(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("WallModel");
                break;
            case "�~�j�I��(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("ConstractAI2");
                break;
            case "�~�j�I��2(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("TankAI2");
                break;
            case "�~�j�I��3(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("LongRangeMinion 1");
                break;
            case "Stairs(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("StairsWoodemodel");
                break;
            case "Chest(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Chestmodel");
                break;
            default:
                break;
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
                case "���傤��":
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
                case "Mana_Handmodel(Clone)":
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

    //TODO:��Ɏ������郂�f���̒ǉ�
    private void SetEquippedModel(GameObject selectedItem)
    {
        if (selecteditemModel != null)
        {
            DestroyImmediate(selecteditemModel.gameObject);
            selecteditemModel = null;
        }

        string selectedItemName = selectedItem.name.Replace("(Clone)", "");
        Debug.Log("Selected Item Name: " + selectedItemName);

        string instancename = CaculateItemModel(selectedItemName);

        if (instancename == null) return;
        // ���f�����C���X�^���X��
        selecteditemModel = Instantiate(Resources.Load<GameObject>(instancename));

        // ���f�����c�[���z���_�[�ɒǉ�
        selecteditemModel.transform.SetParent(toolHolder.transform, false);
    }


    //TODO:������͂����Œǉ�����(�ʒu�A��]�̓v���n�u�̍��W��!!)
    //TODO:�킪�����邲�Ƃɂ����ɒǉ�����
    private string CaculateItemModel(string selectedItemName)
    {
        switch (selectedItemName)
        {
            case "Axe":
                //TODO:������Œ���
                SoundManager.Instance.PlaySound(SoundManager.Instance.PutSeSound);
                return "Axe_model";

            case "Stone":
                return "Stone_model";

            case "Pickaxe":
                //TODO:������Œ���
                SoundManager.Instance.PlaySound(SoundManager.Instance.PutSeSound);
                return "Pickaxe_model";

            case "MinionSeed":
                return "Hand_model";

            case "Minion2Seed":
                return "Hand_model";

            case "Minion3Seed":
                return "Hand_model";

            case "WateringCan":
                return "WateringCan_model";

            case "Mana":
                return "Mana_Handmodel";
            case "�}�i":
                return "Mana_Handmodel";

            default:
                return null;

        }

    }

    

    public bool IsThereSwingLock()
    {

        if (selecteditemModel && selecteditemModel.GetComponent<EquiableItem>())
        {
            Debug.Log("SwingWait in IsThereSwingLock: " + selecteditemModel.GetComponent<EquiableItem>().Swinging);
            return selecteditemModel.GetComponent<EquiableItem>().Swinging;
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
        if (quickSlotsList[slotNumber - 1].transform.childCount > 1)
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
        itemToEquip.transform.SetAsFirstSibling();
        InventorySystem.Instance.ReCalculeList();
        CraftingSystem.Instance.RefreshNeededItems();
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

    public int GetEquippedItemStackCount(GameObject slot, string itemName)
    {
        int totalStackCount = 0;

        // �����œn���ꂽ�X���b�g�ɃA�C�e���������
        if (slot.transform.childCount > 0)
        {
            GameObject item = slot.transform.GetChild(0).gameObject;
            InventoryItem inventoryItem = item.GetComponent<InventoryItem>();

            itemName = InventorySystem.Instance.GetItemName(itemName);
            if (inventoryItem != null && inventoryItem.thisName == itemName)
            {
                totalStackCount += inventoryItem.amountInventry;
                //itemName = InventorySystem.Instance.GetReturnItemName(itemName);

            }
        }

        return totalStackCount;
    }


    public int GetEquippedItemStackCountBySlot(int slotNumber)
    {
        if (slotNumber > 0 && slotNumber <= quickSlotsList.Count)
        {
            GameObject slot = quickSlotsList[slotNumber - 1];
            string itemName = selectedItem?.GetComponent<InventoryItem>()?.thisName;

            // �X���b�g�ƃA�C�e�������m�F�ł�����A�X�^�b�N�����擾
            if (itemName != null)
            {
                return GetEquippedItemStackCount(slot, itemName);
            }
        }
        return 0; // �����ȃX���b�g�ԍ���A�C�e�����Ȃ��ꍇ��0��Ԃ�
    }

    public int ItemStackCnt(string itemName)
    {
        int totalStackCount = 0;

        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                GameObject item = slot.transform.GetChild(0).gameObject;
                InventoryItem inventoryItem = item.GetComponent<InventoryItem>();

                itemName = InventorySystem.Instance.GetItemName(itemName);
                if (inventoryItem != null && inventoryItem.thisName == itemName)
                {
                    totalStackCount += inventoryItem.amountInventry;
                }
            }
        }

        return totalStackCount;
    }

    public bool CheckIfFull()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount >= 2)  // �X���b�g��2�ȏ�̃A�C�e�������邩�m�F
            {
                return true;  // ���t
            }
        }
        return false;  // �󂫂�����
    }

   
    public void RemoveItemFromQuickSlots(string itemName, int amountToRemove)
    {
        int counter = amountToRemove;

        // quickSlotsList���X�L�������ăA�C�e�����폜
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.GetComponent<InventrySlot>() != null && counter > 0)
            {
                InventrySlot inventrySlot = slot.GetComponent<InventrySlot>();
                InventoryItem item = inventrySlot.itemInSlot;

                itemName = InventorySystem.Instance.GetItemName(itemName);
                if (item != null && item.thisName == itemName && item.amountInventry > 0)
                {
                    int amountToDeduct = Mathf.Min(item.amountInventry, counter);
                    item.amountInventry -= amountToDeduct;
                    counter -= amountToDeduct;

                    // �A�C�e���̎c�ʂ�0�ɂȂ�����폜
                    if (item.amountInventry == 0)
                    {
                        Destroy(item.gameObject);
                        inventrySlot.itemInSlot = null;
                    }

                    itemName = InventorySystem.Instance.GetReturnItemName(itemName);
                }
            }

            // �K�v�Ȑ������폜�ł����ꍇ�̓��[�v�𔲂���
            if (counter == 0)
                break;
        }

        InventorySystem.Instance.ReCalculeList();
        CraftingSystem.Instance.RefreshNeededItems();

        // �A�C�e���폜��̏���
    }
}