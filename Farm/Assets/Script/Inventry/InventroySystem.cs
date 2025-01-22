using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �C���x���g�����Ǘ�����v���O����
/// </summary>
public class InventorySystem : MonoBehaviour
{
    /// <summary>
    /// �C���x���g���V�X�e���̃C���X�^���X
    /// </summary>
    public static InventorySystem Instance { get; set; }

    /// <summary>
    /// �A�C�e�����UI
    /// </summary>
    public GameObject ItemInfoUI;

    /// <summary>
    /// �C���x���g�����UI
    /// </summary>
    public GameObject inventoryScreenUI;

    /// <summary>
    /// �C���x���g���X���b�g�̃��X�g
    /// </summary>
    public List<GameObject> slotlist = new List<GameObject>();

    /// <summary>
    /// �A�C�e�����X�g
    /// </summary>
    public List<string> itemList = new List<string>();

    /// <summary>
    /// �ǉ�����A�C�e��
    /// </summary>
    private GameObject itemToAdd;

    /// <summary>
    /// ��������X���b�g
    /// </summary>
    private GameObject whatSlotToEquip;

    /// <summary>
    /// �C���x���g�����J���Ă��邩�ǂ���
    /// </summary>
    public bool isOpen;

    /// <summary>
    /// �C���x���g�����X�V���ꂽ���ǂ���
    /// </summary>
    public bool inventoryUpdated;

    /// <summary>
    /// �擾�����A�C�e���̃��X�g
    /// </summary>
    public List<string> itemsPickedup = new List<string>();

    /// <summary>
    /// �X�^�b�N����
    /// </summary>
    public int stackLimit = 999;

    /// <summary>
    /// �A�C�e�����X�^�b�N���Ă��邩�ǂ���
    /// </summary>
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

    /// <summary>
    /// ������
    /// </summary>
    void Start()
    {
        isOpen = false;
        inventoryUpdated = false;

        PopulateSlotList();

        Cursor.visible = false;
        stackLimit = 999;
    }

    /// <summary>
    /// �C���x���g���X���b�g���X�g�̐����B
    /// </summary>
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

    /// <summary>
    /// �A�C�e���̌��̌v�Z
    /// �C���x���g��(���ݔp�~��)�A�N���t�g�X�N���[���̊J��
    /// �X���b�g�ɃA�C�e����K��������
    /// </summary>
    void Update()
    {

        ReCalculeList();
        CraftingSystem.Instance.RefreshNeededItems();
       

        if (Input.GetKeyDown(KeyCode.E) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;
            ReCalculeList();
            CraftingSystem.Instance.RefreshNeededItems();

        }
        else if (Input.GetKeyDown(KeyCode.E) && isOpen)
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
                    inventrySlot.SetItemInSlot();
                }
            }
        }
    }

    /// <summary>
    /// �C���x���g���A�C�e�����X�V����֐��B
    /// </summary>
    /// <param name="itemName">�A�C�e����</param>
    public void UpdateInventoryItems(string itemName)
    {
        if (!CraftingSystem.Instance.canupdate) return;

        bool itemAdded = false; // �A�C�e�����ǉ����ꂽ���ǂ������L�^����t���O

        // �C���x���g���X���b�g���X�g������
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
                        if (inventrySlot.itemInSlot.thisName == itemNameInList &&
                            !itemAdded &&
                            inventrySlot.itemInSlot.thisName == itemName)
                        {
                            // ��v����A�C�e�������������ꍇ�A���ʂ𑝂₷
                            inventrySlot.SetItemInSlot(); // �A�C�e�������X�V
                            inventrySlot.itemInSlot.amountInventry++;

                            // �A�C�e�����ǉ����ꂽ���Ƃ��L�^���A�������I��
                            itemAdded = true;
                            break;
                        }
                    }

                    // �A�C�e�����ǉ����ꂽ�ꍇ�̓��[�v���I��
                    if (itemAdded)
                    {
                        return;
                    }
                }
            }
        }

        // �N�C�b�N�X���b�g���X�g������
        foreach (GameObject quickSlot in EquipSystem.Instance.quickSlotsList)
        {
            InventrySlot inventrySlot = quickSlot.GetComponent<InventrySlot>();
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
                        if (inventrySlot.itemInSlot.thisName == itemNameInList &&
                            !itemAdded &&
                            inventrySlot.itemInSlot.thisName == itemName)
                        {
                            // ��v����A�C�e�������������ꍇ�A���ʂ𑝂₷
                            inventrySlot.SetItemInSlot(); // �A�C�e�������X�V
                            inventrySlot.itemInSlot.amountInventry++;

                            // �A�C�e�����ǉ����ꂽ���Ƃ��L�^���A�������I��
                            itemAdded = true;
                            break;
                        }
                    }

                    // �A�C�e�����ǉ����ꂽ�ꍇ�̓��[�v���I��
                    if (itemAdded)
                    {
                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// �C���x���g���ɃA�C�e����ǉ�����֐��B
    /// </summary>
    /// <param name="itemName">�A�C�e����</param>
    /// <param name="shoodStack">�X�^�b�N���邩�ǂ���</param>
    public void AddToinventry(string itemName, bool shoodStack)
    {
        GameObject stack = CheckIfStackExists(itemName);

        if (stack != null && shoodStack)
        {
            itemName = GetItemName(itemName);
            UpdateInventoryItems(itemName);
        }
        else
        {
            inventoryUpdated = true;
            itemName = GetReturnItemName(itemName);

            //�C���x���g���̏ꍇ�͂�����
            if (itemName == "Stone" || itemName == "Log")
            {
                whatSlotToEquip = FindNextEmptySlot();
            }
            else
            {
                whatSlotToEquip = FindQuickNextNameSlot(itemName);
            }

            itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
            itemToAdd.transform.SetParent(whatSlotToEquip.transform);
            if (itemName == "�~�j�I��3" || itemName == "�~�j�I��2" || itemName == "�~�j�I��")
            {
                isMinonget = true;
            }
            itemToAdd.transform.SetAsFirstSibling();
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

        // �A�C�e���ǉ���Ƀ��X�g���X�V
        CraftingSystem.Instance.RefreshNeededItems();
    }

    /// <summary>
    /// �X�^�b�N��Ԃ�ݒ肷��֐��B
    /// </summary>
    /// <param name="state">�X�^�b�N���</param>
    public void SetStackState(bool state)
    {
        isStacked = state;
    }

    /// <summary>
    /// �w�肵���A�C�e���̃X�^�b�N�����݂��邩�ǂ������m�F����֐��B
    /// </summary>
    /// <param name="itemName">�A�C�e����</param>
    /// <returns>�����𖞂����X���b�g��GameObject</returns>
    private GameObject CheckIfStackExists(string itemName)
    {
        // �N�C�b�N�X���b�g���X�g������
        foreach (GameObject quickSlot in EquipSystem.Instance.quickSlotsList)
        {
            InventrySlot inventryslot = quickSlot.GetComponent<InventrySlot>();

            inventryslot.SetItemInSlot();

            if (inventryslot != null && inventryslot.itemInSlot != null)
            {
                itemName = GetItemName(itemName);

                if (inventryslot.itemInSlot.thisName == itemName &&
                    inventryslot.itemInSlot.amountInventry < stackLimit)
                {
                    itemName = GetReturnItemName(itemName);
                    return quickSlot; // �����𖞂������炷���ɕԂ�
                }
            }
        }

        // �C���x���g���X���b�g���X�g������
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
                    return slot; // �����𖞂������炷���ɕԂ�
                }
            }
        }
        // �����𖞂����X���b�g��������Ȃ������ꍇ
        return null;
    }


    /// <summary>
    /// �A�C�e��������{��ɕϊ�����֐��B
    /// </summary>
    /// <param name="objectname">���̃A�C�e����</param>
    /// <returns>�ϊ���̃A�C�e����</returns>
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
            case "�~�j�I��2":
                objectname = "�^���N�~�j�I��";
                break;
            case "�^���N�~�j�I��":
                objectname = "�~�j�I��(�^���N)";
                break;
            case "�~�j�I��3(Clone)":
                objectname = "�������~�j�I��";
                break;
            case "�~�j�I��2(Clone)":
                objectname = "�^���N�~�j�I��";
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
            default:
                return objectname;
        }

        return objectname;
    }

    /// <summary>
    /// �A�C�e�������p��ɖ߂��֐��B
    /// </summary>
    /// <param name="objectname">�ϊ���̃A�C�e����</param>
    /// <returns>���̃A�C�e����</returns>
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

    /// <summary>
    /// �C���x���g���ɃA�C�e�������[�h����֐��B
    /// </summary>
    /// <param name="itemName">�A�C�e����</param>
    public void LoadToinventry(string itemName)
    {
        inventoryUpdated = true;
        whatSlotToEquip = FindNextEmptySlot();
        Debug.Log(itemName);
        if (itemName == "TomatoSeed")
        {
            itemName = "MinionSeed";
        }
        itemName = GetReturnItemName(itemName);
        itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
        itemToAdd.transform.SetParent(whatSlotToEquip.transform);

        itemList.Add(itemName);

        ReCalculeList();
        CraftingSystem.Instance.RefreshNeededItems();
    }

    /// <summary>
    /// ���̋󂫃X���b�g��������֐��B
    /// </summary>
    /// <returns>���̋󂫃X���b�g��GameObject</returns>
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

    /// <summary>
    /// �N�C�b�N�X���b�g���玟�̋󂫃X���b�g��������֐��B
    /// </summary>
    /// <returns>���̋󂫃X���b�g��GameObject</returns>
    private GameObject FindQuickNextEmptySlot()
    {
        foreach (GameObject slot in EquipSystem.Instance.quickSlotsList)
        {
            if (slot.transform.childCount <= 1)
            {
                return slot;
            }
        }

        return new GameObject();
    }

    /// <summary>
    /// �N�C�b�N�X���b�g�������̃A�C�e�����Ɋ�Â��Ď��̋󂫃X���b�g��������֐��B
    /// </summary>
    /// <param name="itemName">�A�C�e����</param>
    /// <returns>���̋󂫃X���b�g��GameObject</returns>
    private GameObject FindQuickNextNameSlot(string itemName)
    {
        int slotIndex = 0;

        switch (itemName)
        {
            case "Mana":
                slotIndex = 8;
                break;
            case "Minion3Seed":
                slotIndex = 7;
                break;
            case "Minion2Seed":
                slotIndex = 6;
                break;
            case "MinionSeed":
                slotIndex = 5;
                break;
            case "�~�j�I��2":
                slotIndex = 3;
                break;
            case "�~�j�I��3":
                slotIndex = 4;
                break;
            case "�~�j�I��":
                slotIndex = 2;
                break;
            default:
                break;
        }

        GameObject slot = EquipSystem.Instance.quickSlotsList[slotIndex];

        return slot;
    }

    /// <summary>
    /// �w�肳�ꂽ�󂫃X���b�g�̐������p�\���ǂ������m�F����֐��B
    /// </summary>
    /// <param name="emptyNeeded">�K�v�ȋ󂫃X���b�g�̐�</param>
    /// <returns>�󂫃X���b�g�����p�\�ȏꍇ��true�A�����łȂ��ꍇ��false</returns>
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

    /// <summary>
    /// �w�肳�ꂽ�A�C�e�����C���x���g������폜����֐��B
    /// </summary>
    /// <param name="itemName">�A�C�e����</param>
    /// <param name="amountToRemove">�폜���鐔��</param>
    public void RemoveItem(string itemName, int amountToRemove)
    {
        int remainingAmountToRemove = amountToRemove;

        // �X���b�g���X�g�𔽕��������A�폜����A�C�e�����������C���x���g�������炷
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

                    // �X���b�g���̐��ʂ�0�ɂȂ����ꍇ�A�A�C�e�����폜
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
                break; // ���ׂẴA�C�e�����폜���ꂽ�烋�[�v�𑁊��I��
        }
    }


    /// <summary>
    /// �C���x���g�����X�g���Čv�Z����֐��B
    /// </summary>
    public void ReCalculeList()
    {
        itemList.Clear();  // ���X�g���N���A

        // �C���x���g���X���b�g���X�V
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

        // �N�C�b�N�X���b�g���X�V
        foreach (GameObject quickSlot in EquipSystem.Instance.quickSlotsList)
        {
            if (quickSlot.GetComponent<InventrySlot>())
            {
                InventoryItem item = quickSlot.GetComponent<InventrySlot>().itemInSlot;

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

    /// <summary>
    /// �w�肳�ꂽ�A�C�e���̑������擾����֐��B
    /// </summary>
    /// <param name="itemName">�A�C�e����</param>
    /// <returns>�A�C�e���̑���</returns>
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

    /// <summary>
    /// �w�肳�ꂽ�A�C�e���̃C���x���g�����̐��ʂ��擾����֐��B
    /// </summary>
    /// <param name="itemName">�A�C�e����</param>
    /// <returns>�C���x���g�����̃A�C�e���̐���</returns>
    public int GetInventryItemCount(string itemName)
    {
        int count = 0;

        foreach (GameObject slot in slotlist)
        {
            if (slot.transform.childCount > 1)
            {
                itemName = GetItemName(itemName);
                slot.transform.GetChild(0).name = GetItemName(slot.transform.GetChild(0).name);

                if (slot.transform.GetChild(0).name == itemName)
                {
                    count++;
                }
            }
        }
        return count;
    }

}