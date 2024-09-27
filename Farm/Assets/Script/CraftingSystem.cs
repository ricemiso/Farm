using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem Instance { get; set; }

    public GameObject craftingScreenUI;
    public GameObject toolScreenUI;

    public List<string> inventryitemList = new List<string>();
    public List<GameObject> quickitemList = new List<GameObject>();

    Button toolsBTN;
    Button craftAxeBTN;

    Text AxeReq1, AxeReq2;

    public bool isOpen;

    public BluePrint AxeBLP;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        AxeBLP = new BluePrint("Axe", 2, "Stone", 3, "Stick", 3);

        isOpen = false;

        toolsBTN = craftingScreenUI.transform.Find("ToolButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate { OpenToolsCategory(); });


        AxeReq1 = toolScreenUI.transform.Find("Axe").transform.Find("rec1").GetComponent<Text>();
        AxeReq2 = toolScreenUI.transform.Find("Axe").transform.Find("rec2").GetComponent<Text>();

        craftAxeBTN = toolScreenUI.transform.Find("Axe").transform.Find("AxeButton").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });

    }

    

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {

            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.instance.DisableSelection();
            SelectionManager.instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            toolScreenUI.SetActive(false);

            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                SelectionManager.instance.EnableSelection();
                SelectionManager.instance.GetComponent<SelectionManager>().enabled = true;
            }
            isOpen = false;
        }
    }

    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        toolScreenUI.SetActive(true);
    }

    private void CraftAnyItem(BluePrint blueprintToCraft)
    {
        // ���݂̃C���x���g�����̑f�ނ̐����擾
        int currentAmountReq1 = InventorySystem.Instance.GetItemCount(blueprintToCraft.Req1);
        int currentAmountReq2 = blueprintToCraft.numOfRequirement == 2 ? InventorySystem.Instance.GetItemCount(blueprintToCraft.Req2) : 0;

        // �s�����Ă��鐔���v�Z
        int req1Shortage = Mathf.Max(0, blueprintToCraft.Req1amount - currentAmountReq1);
        int req2Shortage = blueprintToCraft.numOfRequirement == 2 ? Mathf.Max(0, blueprintToCraft.Req2amount - currentAmountReq2) : 0;

        // �N�C�b�N�X���b�g�̃A�C�e���폜����
        if (req1Shortage > 0)
        {
            EquipSystem.Instance.RemoveItemFromQuickSlots(blueprintToCraft.Req1, req1Shortage);
        }

        if (req2Shortage > 0)
        {
            EquipSystem.Instance.RemoveItemFromQuickSlots(blueprintToCraft.Req2, req2Shortage);
        }

        // �C���x���g������K�v�ȃA�C�e�����폜����
        if (blueprintToCraft.numOfRequirement == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
        }
        else if (blueprintToCraft.numOfRequirement == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2amount);
        }

        // �N���t�g���ꂽ�A�C�e�����C���x���g���ɒǉ�
        InventorySystem.Instance.AddToinventry(blueprintToCraft.itemName);

        // �Čv�Z��UI�̍X�V���R���[�`���ŌĂяo��
        StartCoroutine(calulate());
    }


    public void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;

        // �C���x���g�����̃A�C�e�������J�E���g
        inventryitemList = InventorySystem.Instance.itemList;

        foreach (string itemname in inventryitemList)
        {
            switch (itemname)
            {
                case "Stone":
                    stone_count += 1;
                    break;
                case "Stick":
                    stick_count += 1;
                    break;
            }
        }

        // �N�C�b�N�X���b�g���̃A�C�e�����J�E���g
        int quickStoneCount = 0;
        int quickStickCount = 0;

        foreach (GameObject quickSlot in EquipSystem.Instance.quickSlotsList)
        {
            if (quickSlot.transform.childCount > 0)
            {
                string itemName = quickSlot.transform.GetChild(0).name.Replace("(Clone)", "").Trim();
                if (itemName == "Stone")
                {
                    quickStoneCount++;
                }
                else if (itemName == "Stick")
                {
                    quickStickCount++;
                }
            }
        }

        // �K�v�ȃA�C�e���̐���\��
        AxeReq1.text = "3 Stone [" + (stone_count + quickStoneCount) + "]";
        AxeReq2.text = "3 Stick [" + (stick_count + quickStickCount) + "]";

        // �N���t�g�{�^���̗L����
        if ((stone_count + quickStoneCount) >= 3 && (stick_count + quickStickCount) >= 3)
        {
            craftAxeBTN.gameObject.SetActive(true);
        }
        else
        {
            craftAxeBTN.gameObject.SetActive(false);
        }
    }


    public IEnumerator calulate()
    {
        yield return 0;

        InventorySystem.Instance.ReCalculeList();
        RefreshNeededItems();
    }
}
