using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//�S���ҁ@�z�Y�W��

/// <summary>
/// �N���t�g���j���[�̑�����Ǘ�
/// UI�v�f�̕\��/��\���؂�ւ��A�{�^���̃N���b�N�C�x���g�ݒ�A
/// ����уN���t�g�\�A�C�e����Blueprint�f�[�^�Ǘ�
/// </summary>
public class CraftingSystem : MonoBehaviour
{
    /// <summary>
    /// �N���t�e�B���O�V�X�e���̃C���X�^���X�B
    /// </summary>
    public static CraftingSystem Instance { get; set; }

    /// <summary>
    /// �N���t�e�B���O��ʂ�UI�B
    /// </summary>
    public GameObject craftingScreenUI;

    /// <summary>
    /// �c�[����ʂ�UI�B
    /// </summary>
    public GameObject toolScreenUI;

    /// <summary>
    /// �T�o�C�o����ʂ�UI�B
    /// </summary>
    public GameObject survivalScreenUI;

    /// <summary>
    /// ���B��ʂ�UI�B
    /// </summary>
    public GameObject refineScreenUI;

    /// <summary>
    /// ���z��ʂ�UI�B
    /// </summary>
    public GameObject constractionScreenUI;

    /// <summary>
    /// �C���x���g���A�C�e�����X�g�B
    /// </summary>
    public List<string> inventryitemList = new List<string>();

    Button toolsBTN, survivalBTN, refineBTN, construnctinBTN;
    Button toolsExitBTN, survivalExitBTN, refineExitBTN, construnctinExitBTN;
    Button craftAxeBTN, craftPlankBTN, craftfoundationBTN, craftWallBTN, craftPickaxeBTN, MinionBTN, craftStairBTN, craftChestBTN;
    Button normalMinionBTN, TankMinionBTN, MagicMinionBTN;
    Button craftLogManaBTN, craftStoneManaBTN;

    Text AxeReq1, AxeReq2, PickaxeReq1, PickaxeReq2, PlankReq1, foundationReq1, WallReq1, MinionReq1, MinionReq2, StairReq1, ChestReq1, LogReq1, StoneReq1;
    Text normalMinionReq1, TankMinionReq1, TankMinionReq2, MagicMinionReq1, MagicMinionReq2;

    /// <summary>
    /// �N���t�e�B���O��ʂ��J���Ă��邩�ǂ����B
    /// </summary>
    public bool isOpen;

    /// <summary>
    /// ���x���A�b�v���Ă��邩�ǂ����B
    /// </summary>
    public bool islevelUp;

    private bool canCraft = true;

    /// <summary>
    /// �A�b�v�f�[�g�\���ǂ����B
    /// </summary>
    public bool canupdate = true;

    /// <summary>
    /// �A�C�e�������������ǂ����B
    /// </summary>
    public bool itemIncreased = false;

    /// <summary>
    /// �~�j�I���̃J�E���g�B
    /// </summary>
    public int minion_count;

    [HideInInspector] public BluePrint AxeBLP;
    [HideInInspector] public BluePrint PickaxeBLP;
    [HideInInspector] public BluePrint PlankBLP;
    [HideInInspector] public BluePrint foundationBLP;
    [HideInInspector] public BluePrint WallBLP;
    [HideInInspector] public BluePrint MinionBLP;
    [HideInInspector] public BluePrint StairBLP;
    [HideInInspector] public BluePrint NormalMinionBLP;
    [HideInInspector] public BluePrint TankMinionBLP;
    [HideInInspector] public BluePrint MagicMinionBLP;
    [HideInInspector] public BluePrint ChestBLP;
    [HideInInspector] public BluePrint LogManaBLP;
    [HideInInspector] public BluePrint StoneManaBLP;

    [HideInInspector] public bool isMinionCraft = false;
    [HideInInspector] public bool isTankMinionCraft = false;
    [HideInInspector] public bool isMagicMinionCraft = false;

    // �`���[�g���A���p�̕ϐ�
    [HideInInspector] public bool isFarm3 = false;



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
    /// �N���t�e�B���O�V�X�e���̏����ݒ���s���܂��B
    /// �K�v�ȃ{�^����UI�R���|�[�l���g���擾���A���X�i�[��ǉ����܂��B
    /// �N���t�g�̃��V�s�Ȃǂ������ŏ�����
    /// </summary>
    void Start()
    {
        inventryitemList = new List<string>();

        AxeBLP = new BluePrint("Axe", 1, 2, "Stone", 3, "Stick", 3);
        PickaxeBLP = new BluePrint("Pickaxe", 1, 2, "Stone", 3, "Stick", 3);
        PlankBLP = new BluePrint("Plank", 2, 1, "Log", 1, "", 0);
        foundationBLP = new BluePrint("Foundation", 1, 1, "Plank", 4, "", 0);
        WallBLP = new BluePrint("Wall", 1, 1, "Plank", 2, "", 0);
        MinionBLP = new BluePrint("�~�j�I��", 1, 2, "Mana", 1, "�~�j�I��", 1);
        StairBLP = new BluePrint("Stairs", 1, 1, "Plank", 4, "", 0);
        NormalMinionBLP = new BluePrint("MinionSeed", 1, 1, "Mana", 1, "", 0);
        TankMinionBLP = new BluePrint("Minion2Seed", 1, 2, "Mana", 1, "Stone", 1);
        MagicMinionBLP = new BluePrint("Minion3Seed", 1, 2, "Mana", 1, "Log", 1);
        ChestBLP = new BluePrint("Chest", 1, 1, "Log", 4, "", 0);
        LogManaBLP = new BluePrint("Mana", 1, 1, "Log", 2, "", 0);
        StoneManaBLP = new BluePrint("Mana", 2, 1, "Stone", 6, "", 0);

        isOpen = false;


        //�I�����
        toolsBTN = craftingScreenUI.transform.Find("ToolButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate { OpenToolsCategory(); });

        survivalBTN = craftingScreenUI.transform.Find("SurvivalButton").GetComponent<Button>();
        survivalBTN.onClick.AddListener(delegate { OpenSurvivalCategory(); });

        refineBTN = craftingScreenUI.transform.Find("RefineButton").GetComponent<Button>();
        refineBTN.onClick.AddListener(delegate { OpenRefineCategory(); });

        construnctinBTN = craftingScreenUI.transform.Find("ConstructionButton").GetComponent<Button>();
        construnctinBTN.onClick.AddListener(delegate { OpenconstrunctinCategory(); });



        //Exit
        toolsExitBTN = toolScreenUI.transform.Find("CraftingToolsButton").GetComponent<Button>();
        toolsExitBTN.onClick.AddListener(delegate { CloseToolsCategory(); });

        survivalExitBTN = survivalScreenUI.transform.Find("SurvivalToolsButton").GetComponent<Button>();
        survivalExitBTN.onClick.AddListener(delegate { CloseSurvivalCategory(); });

        refineExitBTN = refineScreenUI.transform.Find("RefineToolsButton").GetComponent<Button>();
        refineExitBTN.onClick.AddListener(delegate { CloseRefineCategory(); });

        construnctinExitBTN = constractionScreenUI.transform.Find("ConstractionButton").GetComponent<Button>();
        construnctinExitBTN.onClick.AddListener(delegate { CloseConstractionCategory(); });


        //----�N���t�g-----//
        //Axe
        AxeReq1 = toolScreenUI.transform.Find("Axe").transform.Find("rec1").GetComponent<Text>();
        AxeReq2 = toolScreenUI.transform.Find("Axe").transform.Find("rec2").GetComponent<Text>();

        craftAxeBTN = toolScreenUI.transform.Find("Axe").transform.Find("AxeButton").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });

        //Pickaxe
        PickaxeReq1 = toolScreenUI.transform.Find("Pickaxe").transform.Find("rec1").GetComponent<Text>();
        PickaxeReq2 = toolScreenUI.transform.Find("Pickaxe").transform.Find("rec2").GetComponent<Text>();

        craftPickaxeBTN = toolScreenUI.transform.Find("Pickaxe").transform.Find("PickaxeButton").GetComponent<Button>();
        craftPickaxeBTN.onClick.AddListener(delegate { CraftAnyItem(PickaxeBLP); });


        //Plank
        PlankReq1 = refineScreenUI.transform.Find("Plank").transform.Find("rec1").GetComponent<Text>();

        craftPlankBTN = refineScreenUI.transform.Find("Plank").transform.Find("PlankButton").GetComponent<Button>();
        craftPlankBTN.onClick.AddListener(delegate { CraftAnyItem(PlankBLP); });


        //foundatin
        foundationReq1 = constractionScreenUI.transform.Find("foundation").transform.Find("rec1").GetComponent<Text>();

        craftfoundationBTN = constractionScreenUI.transform.Find("foundation").transform.Find("foundationButton").GetComponent<Button>();
        craftfoundationBTN.onClick.AddListener(delegate { CraftAnyItem(foundationBLP); });

        //Wall
        WallReq1 = constractionScreenUI.transform.Find("Wall").transform.Find("rec1").GetComponent<Text>();

        craftWallBTN = constractionScreenUI.transform.Find("Wall").transform.Find("WallButton").GetComponent<Button>();
        craftWallBTN.onClick.AddListener(delegate { CraftAnyItem(WallBLP); });

        //Minion
        MinionReq1 = survivalScreenUI.transform.Find("Minion").transform.Find("rec1").GetComponent<Text>();
        MinionReq2 = survivalScreenUI.transform.Find("Minion").transform.Find("rec2").GetComponent<Text>();

        MinionBTN = survivalScreenUI.transform.Find("Minion").transform.Find("MinionButton").GetComponent<Button>();
        MinionBTN.onClick.AddListener(delegate { CraftAnyItem(MinionBLP); });

        //Stair
        StairReq1 = constractionScreenUI.transform.Find("Stair").transform.Find("rec1").GetComponent<Text>();

        craftStairBTN = constractionScreenUI.transform.Find("Stair").transform.Find("StairButton").GetComponent<Button>();
        craftStairBTN.onClick.AddListener(delegate { CraftAnyItem(StairBLP); });


        //normalMinion
        normalMinionReq1 = refineScreenUI.transform.Find("NormalMinion").transform.Find("rec1").GetComponent<Text>();
        normalMinionBTN = refineScreenUI.transform.Find("NormalMinion").transform.Find("NormalMinionButton").GetComponent<Button>();
        normalMinionBTN.onClick.AddListener(delegate { CraftAnyItem(NormalMinionBLP); });

        //TankMinion
        TankMinionReq1 = refineScreenUI.transform.Find("TankMinion").transform.Find("rec1").GetComponent<Text>();
        TankMinionReq2 = refineScreenUI.transform.Find("TankMinion").transform.Find("rec2").GetComponent<Text>();

        TankMinionBTN = refineScreenUI.transform.Find("TankMinion").transform.Find("TankMinionButton").GetComponent<Button>();
        TankMinionBTN.onClick.AddListener(delegate { CraftAnyItem(TankMinionBLP); });

        //LongMinion
        MagicMinionReq1 = refineScreenUI.transform.Find("MagicMinion").transform.Find("rec1").GetComponent<Text>();
        MagicMinionReq2 = refineScreenUI.transform.Find("MagicMinion").transform.Find("rec2").GetComponent<Text>();

        MagicMinionBTN = refineScreenUI.transform.Find("MagicMinion").transform.Find("MagicMinionButton").GetComponent<Button>();
        MagicMinionBTN.onClick.AddListener(delegate { CraftAnyItem(MagicMinionBLP); });

        //Chest1
        ChestReq1 = toolScreenUI.transform.Find("Chest").transform.Find("rec1").GetComponent<Text>();

        craftChestBTN = toolScreenUI.transform.Find("Chest").transform.Find("ChestButton").GetComponent<Button>();
        craftChestBTN.onClick.AddListener(delegate { CraftAnyItem(ChestBLP); });

        //LogMana
        LogReq1 = toolScreenUI.transform.Find("LogMana").transform.Find("rec1").GetComponent<Text>();

        craftLogManaBTN = toolScreenUI.transform.Find("LogMana").transform.Find("LogManaButton").GetComponent<Button>();
        craftLogManaBTN.onClick.AddListener(delegate { CraftAnyItem(LogManaBLP); GrobalState.Instance.isManaCraft = true; });


        //StoneMana
        StoneReq1 = toolScreenUI.transform.Find("StoneMana").transform.Find("rec1").GetComponent<Text>();

        craftStoneManaBTN = toolScreenUI.transform.Find("StoneMana").transform.Find("StoneManaButton").GetComponent<Button>();
        craftStoneManaBTN.onClick.AddListener(delegate { CraftAnyItem(StoneManaBLP); GrobalState.Instance.isManaCraft = true; });
    }

    /// <summary>
    /// �c�[���J�e�S���[��ʂ���A�N���t�e�B���O��ʂ�\�����܂��B
    /// </summary>
    void CloseToolsCategory()
    {
        toolScreenUI.SetActive(false);
        craftingScreenUI.SetActive(true);
    }

    /// <summary>
    /// �c�[���J�e�S���[��ʂ��J���A���̃J�e�S���[��ʂ��\���ɂ��܂��B
    /// </summary>
    void OpenToolsCategory()
    {
        // craftingScreenUI.SetActive(false);
        toolScreenUI.SetActive(true);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        constractionScreenUI.SetActive(false);
    }

    void CloseSurvivalCategory()
    {
        survivalScreenUI.SetActive(false);
        craftingScreenUI.SetActive(true);
    }

    /// <summary>
    /// �T�o�C�o���J�e�S���[��ʂ��J���A���̃J�e�S���[��ʂ��\���ɂ��܂��B
    /// </summary>
    void OpenSurvivalCategory()
    {
        toolScreenUI.SetActive(false);
        // craftingScreenUI.SetActive(false);
        survivalScreenUI.SetActive(true);
        refineScreenUI.SetActive(false);
        constractionScreenUI.SetActive(false);
    }


    /// <summary>
    /// ���B�J�e�S���[��ʂ���A�N���t�e�B���O��ʂ�\�����܂��B
    /// </summary>
    void CloseRefineCategory()
    {
        refineScreenUI.SetActive(false);
        craftingScreenUI.SetActive(true);
    }

    /// <summary>
    /// ���B�J�e�S���[��ʂ��J���A���̃J�e�S���[��ʂ��\���ɂ��܂��B
    /// </summary>
    void OpenRefineCategory()
    {
        toolScreenUI.SetActive(false);
        //  craftingScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(true);
        constractionScreenUI.SetActive(false);
    }

    /// <summary>
    /// ���z�J�e�S���[��ʂ���A�N���t�e�B���O��ʂ�\�����܂��B
    /// </summary>
    void CloseConstractionCategory()
    {
        constractionScreenUI.SetActive(false);
        craftingScreenUI.SetActive(true);
    }

    /// <summary>
    /// ���z�J�e�S���[��ʂ��J���A���̃J�e�S���[��ʂ��\���ɂ��܂��B
    /// </summary>
    void OpenconstrunctinCategory()
    {
        toolScreenUI.SetActive(false);
        // craftingScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        constractionScreenUI.SetActive(true);
    }

   
    /// <summary>
    /// �N���t�g�X�N���[�����L�[���͂ŏo��
    /// </summary>
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
        {
            //TODO:��U�d���̍��������Ȃ����ߒ��ڍ����X�N���[�����o��
            //�A���t�@�ł͖߂�
           // craftingScreenUI.SetActive(true);
            refineScreenUI.SetActive(true);
            toolScreenUI.SetActive(true);



            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;

            RefreshNeededItems();

        }
        else if (Input.GetKeyDown(KeyCode.E) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            toolScreenUI.SetActive(false);
            survivalScreenUI.SetActive(false);
            refineScreenUI.SetActive(false);
            constractionScreenUI.SetActive(false);

            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                SelectionManager.Instance.EnableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            }
            isOpen = false;
        }
    }


    /// <summary>
    /// �N���t�g�ҋ@����
    /// </summary>
    /// <param name="blueprintToCraft">�N���t�g����u���[�v�����g</param>
    private void CraftAnyItem(BluePrint blueprintToCraft)
    {
        if (!canCraft) return;
        StartCoroutine(CraftWithCooldown(blueprintToCraft));
    }

    /// <summary>
    /// �A�C�e���̐����Ə�������
    /// </summary>
    /// <param name="blueprintToCraft">�N���t�g����u���[�v�����g</param>
    /// <returns></returns>
    private IEnumerator CraftWithCooldown(BluePrint blueprintToCraft)
    {
        canCraft = false;

        SoundManager.Instance.PlaySound(SoundManager.Instance.craftingSound);

        for (var i = 0; i < blueprintToCraft.numberOfItemsToProduce; i++)
        {

            if (blueprintToCraft.itemName == "�~�j�I��")
            {
                islevelUp = true;
                GrobalState.Instance.level += 1;
            }
            else
            {
                canupdate = true;

                bool itemExists = false;
                foreach (GameObject slot in InventorySystem.Instance.slotlist)
                {
                    InventrySlot inventrySlot = slot.GetComponent<InventrySlot>();
                    if (inventrySlot != null && inventrySlot.itemInSlot != null)
                    {
                        var itemName = InventorySystem.Instance.GetItemName(blueprintToCraft.itemName);

                        if (inventrySlot.itemInSlot.thisName == itemName)
                        {
                            itemExists = true;
                            break;
                        }
                    }
                }

                if (!itemExists)
                {
                    InventorySystem.Instance.AddToinventry(blueprintToCraft.itemName, true);
                }
                else
                {
                    foreach (GameObject slot in InventorySystem.Instance.slotlist)
                    {
                        InventrySlot inventrySlot = slot.GetComponent<InventrySlot>();
                        if (inventrySlot != null)
                        {
                            // �X���b�g����A�C�e�����擾
                            inventrySlot.itemInSlot = inventrySlot.CheckInventryItem();

                            // �X���b�g���ɃA�C�e��������ꍇ
                            if (inventrySlot.itemInSlot != null)
                            {
                                var itemName = InventorySystem.Instance.GetItemName(blueprintToCraft.itemName);
                                // itemList ���̊e�A�C�e�����ƃX���b�g���̃A�C�e��������v���邩�m�F
                                foreach (string itemNameInList in InventorySystem.Instance.itemList)
                                {
                                    if (inventrySlot.itemInSlot.thisName == itemName && !itemIncreased)
                                    {

                                        // ��v����A�C�e�������������ꍇ�A���ʂ𑝂₷
                                        inventrySlot.SetItemInSlot(); // �A�C�e�������X�V
                                        inventrySlot.itemInSlot.amountInventry++;
                                        itemIncreased = true;
                                        break; // ��v����A�C�e�������������玟�̃A�C�e������
                                    }
                                }


                            }
                        }
                    }
                }                  

            }
        }
        itemIncreased = false;
        canupdate = false;

        // ���݂̃C���x���g�����̑f�ނ̐����擾
        int currentAmountReq1 = InventorySystem.Instance.GetInventryItemCount(blueprintToCraft.Req1);
        int currentAmountReq2 = blueprintToCraft.numOfRequirement == 2 ? InventorySystem.Instance.GetInventryItemCount(blueprintToCraft.Req2) : 0;

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

        // �C���x���g������K�v�ȃA�C�e�����폜
        if (blueprintToCraft.numOfRequirement == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
        }
        else if (blueprintToCraft.numOfRequirement == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2amount);
        }

        // �Čv�Z��UI�̍X�V���R���[�`���ŌĂяo��
        yield return StartCoroutine(calulate());

        // �N���t�g�\�ɍĐݒ�
        canCraft = true;
        canupdate = true;
    }

    /// <summary>
    /// // �K�v�ȃA�C�e���̐����X�V����
    /// //TODO:�A�C�e����ǉ��������ɂ����̏�����ǉ�����
    /// </summary>
    public void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;
        int log_count = 0;
        int plank_count = 0;
        int mana_count = 0;
        minion_count = 0;

        // �C���x���g�����̃A�C�e�������J�E���g
        inventryitemList = InventorySystem.Instance.itemList;

        foreach (string itemname in inventryitemList)
        {
            switch (itemname)
            {
                case "�΂���":
                    stone_count += 1;
                    break;
                case "Stick":
                    stick_count += 1;
                    break;
                case "�ۑ�":
                    log_count += 1;
                    break;
                case "Plank":
                    plank_count += 1;
                    break;
                case "�}�i":
                    mana_count += 1;
                    break;
                case "�~�j�I��":
                    minion_count += 1;
                    break;
            }
        }

        //TODO: �N���t�g�A�C�e���͂����Œǉ�
        // ----Axe---- //
        AxeReq1.text = "�}�i 3 [" + (stone_count/* + quickStoneCount*/) + "]";
        AxeReq2.text = "�} 3 [" + (stick_count/* + quickStickCount*/) + "]";

        if ((stone_count/* + quickStoneCount*/) >= 3 && (stick_count /*+ quickStickCount*/) >= 3
            && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            craftAxeBTN.gameObject.SetActive(true);
        }
        else
        {
            craftAxeBTN.gameObject.SetActive(false);
        }

        // ----Pickaxe---- //
        PickaxeReq1.text = "�� 3 [" + (stone_count/* + quickStoneCount*/) + "]";
        PickaxeReq2.text = "�} 3 [" + (stick_count /*+ quickStickCount*/) + "]";

        if ((stone_count/* + quickStoneCount*/) >= 3 && (stick_count /*+ quickStickCount*/) >= 3
            && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            craftPickaxeBTN.gameObject.SetActive(true);
        }
        else
        {
            craftPickaxeBTN.gameObject.SetActive(false);
        }


        // ----Plank x2---- //
        PlankReq1.text = "1 �ۑ� [" + (log_count/* + quickLogCount*/) + "]";

        if ((log_count/* + quickLogCount*/) >= 1 && InventorySystem.Instance.CheckSlotAvailable(2))
        {
            craftPlankBTN.gameObject.SetActive(true);
        }
        else
        {
            craftPlankBTN.gameObject.SetActive(false);
        }

        // ----Foundation---- //
        foundationReq1.text = "4 �� [" + (plank_count/* + quickPlankCount*/) + "]";

        if ((plank_count /*+ quickPlankCount*/) >= 4 && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            craftfoundationBTN.gameObject.SetActive(true);
        }
        else
        {
            craftfoundationBTN.gameObject.SetActive(false);
        }

        // ----Wall---- //
        WallReq1.text = "2 �� [" + (plank_count /*+ quickPlankCount*/) + "]";

        if ((plank_count/* + quickPlankCount*/) >= 2 && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            craftWallBTN.gameObject.SetActive(true);
        }
        else
        {
            craftWallBTN.gameObject.SetActive(false);
        }

        // ----Stair---- //
        StairReq1.text = "4 �� [" + (plank_count /*+ quickPlankCount*/) + "]";

        if ((plank_count/* + quickPlankCount*/) >= 2 && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            craftStairBTN.gameObject.SetActive(true);
        }
        else
        {
            craftStairBTN.gameObject.SetActive(false);
        }


        // ---Minion---- //
        MinionReq1.text = "�}�i 1 [" + (mana_count /*+ quickManaCount*/) + "]";
        MinionReq2.text = "�~�j�I�� 1 [" + (minion_count /*+ quickMinionCount*/) + "]";

        if ((mana_count /*+ quickManaCount*/) >= 1 && (minion_count/* + quickMinionCount*/) >= 1
            && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            MinionBTN.gameObject.SetActive(true);
        }
        else
        {
            MinionBTN.gameObject.SetActive(false);
        }

        // ---normalMinionReq1---- //
        normalMinionReq1.text = "�}�i 1 [" + (mana_count /*+ quickManaCount*/) + "]";

        if ((mana_count /*+ quickManaCount*/) >= 1
            && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            normalMinionBTN.gameObject.SetActive(true);
            isMinionCraft = true;
        }
        else
        {
            normalMinionBTN.gameObject.SetActive(false);
        }


        // ---TankMinion---- //
        TankMinionReq1.text = "�}�i 1 [" + (mana_count/* + quickManaCount*/) + "]";
        TankMinionReq2.text = "�� 1 [" + (stone_count /*+ quickStoneCount*/) + "]";

        if ((mana_count/* + quickManaCount*/) >= 1 && (stone_count /*+ quickStoneCount*/) >= 1
            && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            TankMinionBTN.gameObject.SetActive(true);
            isTankMinionCraft = true;
        }
        else
        {
            TankMinionBTN.gameObject.SetActive(false);
        }

        // ---MagicMinion---- //
        MagicMinionReq1.text = "�}�i 1 [" + (mana_count /*+ quickManaCount*/) + "]";
        MagicMinionReq2.text = "�ۑ� 1 [" + (log_count /*+ quickLogCount*/) + "]";

        if ((mana_count /*+ quickManaCount*/) >= 1 && (log_count /*+ quickLogCount*/) >= 1
            && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            MagicMinionBTN.gameObject.SetActive(true);
            isMagicMinionCraft = true;
        }
        else
        {
            MagicMinionBTN.gameObject.SetActive(false);
        }


        //Chest
        ChestReq1.text = "4 �ۑ� [" + (log_count/* + quickLogCount*/) + "]";

        if ((log_count /*+ quickLogCount*/) >= 1 && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            craftChestBTN.gameObject.SetActive(true);
        }
        else
        {
            craftChestBTN.gameObject.SetActive(false);
        }


        // ---LogMana---- //
        LogReq1.text = "�ۑ� 2 [" + (log_count/* + quickLogCount*/) + "]";

        if ((log_count/* + quickLogCount*/) >= 2
            && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            craftLogManaBTN.gameObject.SetActive(true);
        }
        else
        {
            craftLogManaBTN.gameObject.SetActive(false);
        }


		// ---StoneMana---- //
		StoneReq1.text = "�΂��� 6 [" + (stone_count /*+ quickStoneCount*/) + "]";

        if ((stone_count/* + quickStoneCount*/) >= 6
            && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            craftStoneManaBTN.gameObject.SetActive(true);
        }
        else
        {
            craftStoneManaBTN.gameObject.SetActive(false);
        }

    }

    /// <summary>
    /// �A�C�e���̍Čv�Z���s���R���[�`��
    /// </summary>
    /// <returns></returns>
    public IEnumerator calulate()
    {
        yield return 0;

        InventorySystem.Instance.ReCalculeList();
        RefreshNeededItems();
    }
}
