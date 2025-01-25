using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//担当者　越浦晃生

/// <summary>
/// クラフトメニューの操作を管理
/// UI要素の表示/非表示切り替え、ボタンのクリックイベント設定、
/// およびクラフト可能アイテムのBlueprintデータ管理
/// </summary>
public class CraftingSystem : MonoBehaviour
{
    /// <summary>
    /// クラフティングシステムのインスタンス。
    /// </summary>
    public static CraftingSystem Instance { get; set; }

    /// <summary>
    /// クラフティング画面のUI。
    /// </summary>
    public GameObject craftingScreenUI;

    /// <summary>
    /// ツール画面のUI。
    /// </summary>
    public GameObject toolScreenUI;

    /// <summary>
    /// サバイバル画面のUI。
    /// </summary>
    public GameObject survivalScreenUI;

    /// <summary>
    /// 精錬画面のUI。
    /// </summary>
    public GameObject refineScreenUI;

    /// <summary>
    /// 建築画面のUI。
    /// </summary>
    public GameObject constractionScreenUI;

    /// <summary>
    /// インベントリアイテムリスト。
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
    /// クラフティング画面が開いているかどうか。
    /// </summary>
    public bool isOpen;

    /// <summary>
    /// レベルアップしているかどうか。
    /// </summary>
    public bool islevelUp;

    private bool canCraft = true;

    /// <summary>
    /// アップデート可能かどうか。
    /// </summary>
    public bool canupdate = true;

    /// <summary>
    /// アイテムが増えたかどうか。
    /// </summary>
    public bool itemIncreased = false;

    /// <summary>
    /// ミニオンのカウント。
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

    // チュートリアル用の変数
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
    /// クラフティングシステムの初期設定を行います。
    /// 必要なボタンやUIコンポーネントを取得し、リスナーを追加します。
    /// クラフトのレシピなどもここで初期化
    /// </summary>
    void Start()
    {
        inventryitemList = new List<string>();

        AxeBLP = new BluePrint("Axe", 1, 2, "Stone", 3, "Stick", 3);
        PickaxeBLP = new BluePrint("Pickaxe", 1, 2, "Stone", 3, "Stick", 3);
        PlankBLP = new BluePrint("Plank", 2, 1, "Log", 1, "", 0);
        foundationBLP = new BluePrint("Foundation", 1, 1, "Plank", 4, "", 0);
        WallBLP = new BluePrint("Wall", 1, 1, "Plank", 2, "", 0);
        MinionBLP = new BluePrint("ミニオン", 1, 2, "Mana", 1, "ミニオン", 1);
        StairBLP = new BluePrint("Stairs", 1, 1, "Plank", 4, "", 0);
        NormalMinionBLP = new BluePrint("MinionSeed", 1, 1, "Mana", 1, "", 0);
        TankMinionBLP = new BluePrint("Minion2Seed", 1, 2, "Mana", 1, "Stone", 1);
        MagicMinionBLP = new BluePrint("Minion3Seed", 1, 2, "Mana", 1, "Log", 1);
        ChestBLP = new BluePrint("Chest", 1, 1, "Log", 4, "", 0);
        LogManaBLP = new BluePrint("Mana", 1, 1, "Log", 2, "", 0);
        StoneManaBLP = new BluePrint("Mana", 2, 1, "Stone", 6, "", 0);

        isOpen = false;


        //選択画面
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


        //----クラフト-----//
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
    /// ツールカテゴリー画面を閉じ、クラフティング画面を表示します。
    /// </summary>
    void CloseToolsCategory()
    {
        toolScreenUI.SetActive(false);
        craftingScreenUI.SetActive(true);
    }

    /// <summary>
    /// ツールカテゴリー画面を開き、他のカテゴリー画面を非表示にします。
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
    /// サバイバルカテゴリー画面を開き、他のカテゴリー画面を非表示にします。
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
    /// 精錬カテゴリー画面を閉じ、クラフティング画面を表示します。
    /// </summary>
    void CloseRefineCategory()
    {
        refineScreenUI.SetActive(false);
        craftingScreenUI.SetActive(true);
    }

    /// <summary>
    /// 精錬カテゴリー画面を開き、他のカテゴリー画面を非表示にします。
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
    /// 建築カテゴリー画面を閉じ、クラフティング画面を表示します。
    /// </summary>
    void CloseConstractionCategory()
    {
        constractionScreenUI.SetActive(false);
        craftingScreenUI.SetActive(true);
    }

    /// <summary>
    /// 建築カテゴリー画面を開き、他のカテゴリー画面を非表示にします。
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
    /// クラフトスクリーンをキー入力で出力
    /// </summary>
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
        {
            //TODO:一旦妖精の合成しかないため直接合成スクリーンを出す
            //アルファ版は戻す
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
    /// クラフト待機する
    /// </summary>
    /// <param name="blueprintToCraft">クラフトするブループリント</param>
    private void CraftAnyItem(BluePrint blueprintToCraft)
    {
        if (!canCraft) return;
        StartCoroutine(CraftWithCooldown(blueprintToCraft));
    }

    /// <summary>
    /// アイテムの生成と消去処理
    /// </summary>
    /// <param name="blueprintToCraft">クラフトするブループリント</param>
    /// <returns></returns>
    private IEnumerator CraftWithCooldown(BluePrint blueprintToCraft)
    {
        canCraft = false;

        SoundManager.Instance.PlaySound(SoundManager.Instance.craftingSound);

        for (var i = 0; i < blueprintToCraft.numberOfItemsToProduce; i++)
        {

            if (blueprintToCraft.itemName == "ミニオン")
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
                            // スロットからアイテムを取得
                            inventrySlot.itemInSlot = inventrySlot.CheckInventryItem();

                            // スロット内にアイテムがある場合
                            if (inventrySlot.itemInSlot != null)
                            {
                                var itemName = InventorySystem.Instance.GetItemName(blueprintToCraft.itemName);
                                // itemList 内の各アイテム名とスロット内のアイテム名が一致するか確認
                                foreach (string itemNameInList in InventorySystem.Instance.itemList)
                                {
                                    if (inventrySlot.itemInSlot.thisName == itemName && !itemIncreased)
                                    {

                                        // 一致するアイテムが見つかった場合、数量を増やす
                                        inventrySlot.SetItemInSlot(); // アイテム情報を更新
                                        inventrySlot.itemInSlot.amountInventry++;
                                        itemIncreased = true;
                                        break; // 一致するアイテムが見つかったら次のアイテム名へ
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

        // 現在のインベントリ内の素材の数を取得
        int currentAmountReq1 = InventorySystem.Instance.GetInventryItemCount(blueprintToCraft.Req1);
        int currentAmountReq2 = blueprintToCraft.numOfRequirement == 2 ? InventorySystem.Instance.GetInventryItemCount(blueprintToCraft.Req2) : 0;

        // 不足している数を計算
        int req1Shortage = Mathf.Max(0, blueprintToCraft.Req1amount - currentAmountReq1);
        int req2Shortage = blueprintToCraft.numOfRequirement == 2 ? Mathf.Max(0, blueprintToCraft.Req2amount - currentAmountReq2) : 0;



        // クイックスロットのアイテム削除処理
        if (req1Shortage > 0)
        {
            EquipSystem.Instance.RemoveItemFromQuickSlots(blueprintToCraft.Req1, req1Shortage);
        }

        if (req2Shortage > 0)
        {
            EquipSystem.Instance.RemoveItemFromQuickSlots(blueprintToCraft.Req2, req2Shortage);
        }

        // インベントリから必要なアイテムを削除
        if (blueprintToCraft.numOfRequirement == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
        }
        else if (blueprintToCraft.numOfRequirement == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2amount);
        }

        // 再計算とUIの更新をコルーチンで呼び出し
        yield return StartCoroutine(calulate());

        // クラフト可能に再設定
        canCraft = true;
        canupdate = true;
    }

    /// <summary>
    /// // 必要なアイテムの数を更新する
    /// //TODO:アイテムを追加した時にここの条件を追加する
    /// </summary>
    public void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;
        int log_count = 0;
        int plank_count = 0;
        int mana_count = 0;
        minion_count = 0;

        // インベントリ内のアイテム数をカウント
        inventryitemList = InventorySystem.Instance.itemList;

        foreach (string itemname in inventryitemList)
        {
            switch (itemname)
            {
                case "石ころ":
                    stone_count += 1;
                    break;
                case "Stick":
                    stick_count += 1;
                    break;
                case "丸太":
                    log_count += 1;
                    break;
                case "Plank":
                    plank_count += 1;
                    break;
                case "マナ":
                    mana_count += 1;
                    break;
                case "ミニオン":
                    minion_count += 1;
                    break;
            }
        }

        //TODO: クラフトアイテムはここで追加
        // ----Axe---- //
        AxeReq1.text = "マナ 3 [" + (stone_count/* + quickStoneCount*/) + "]";
        AxeReq2.text = "枝 3 [" + (stick_count/* + quickStickCount*/) + "]";

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
        PickaxeReq1.text = "石 3 [" + (stone_count/* + quickStoneCount*/) + "]";
        PickaxeReq2.text = "枝 3 [" + (stick_count /*+ quickStickCount*/) + "]";

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
        PlankReq1.text = "1 丸太 [" + (log_count/* + quickLogCount*/) + "]";

        if ((log_count/* + quickLogCount*/) >= 1 && InventorySystem.Instance.CheckSlotAvailable(2))
        {
            craftPlankBTN.gameObject.SetActive(true);
        }
        else
        {
            craftPlankBTN.gameObject.SetActive(false);
        }

        // ----Foundation---- //
        foundationReq1.text = "4 板 [" + (plank_count/* + quickPlankCount*/) + "]";

        if ((plank_count /*+ quickPlankCount*/) >= 4 && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            craftfoundationBTN.gameObject.SetActive(true);
        }
        else
        {
            craftfoundationBTN.gameObject.SetActive(false);
        }

        // ----Wall---- //
        WallReq1.text = "2 板 [" + (plank_count /*+ quickPlankCount*/) + "]";

        if ((plank_count/* + quickPlankCount*/) >= 2 && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            craftWallBTN.gameObject.SetActive(true);
        }
        else
        {
            craftWallBTN.gameObject.SetActive(false);
        }

        // ----Stair---- //
        StairReq1.text = "4 板 [" + (plank_count /*+ quickPlankCount*/) + "]";

        if ((plank_count/* + quickPlankCount*/) >= 2 && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            craftStairBTN.gameObject.SetActive(true);
        }
        else
        {
            craftStairBTN.gameObject.SetActive(false);
        }


        // ---Minion---- //
        MinionReq1.text = "マナ 1 [" + (mana_count /*+ quickManaCount*/) + "]";
        MinionReq2.text = "ミニオン 1 [" + (minion_count /*+ quickMinionCount*/) + "]";

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
        normalMinionReq1.text = "マナ 1 [" + (mana_count /*+ quickManaCount*/) + "]";

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
        TankMinionReq1.text = "マナ 1 [" + (mana_count/* + quickManaCount*/) + "]";
        TankMinionReq2.text = "石 1 [" + (stone_count /*+ quickStoneCount*/) + "]";

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
        MagicMinionReq1.text = "マナ 1 [" + (mana_count /*+ quickManaCount*/) + "]";
        MagicMinionReq2.text = "丸太 1 [" + (log_count /*+ quickLogCount*/) + "]";

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
        ChestReq1.text = "4 丸太 [" + (log_count/* + quickLogCount*/) + "]";

        if ((log_count /*+ quickLogCount*/) >= 1 && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            craftChestBTN.gameObject.SetActive(true);
        }
        else
        {
            craftChestBTN.gameObject.SetActive(false);
        }


        // ---LogMana---- //
        LogReq1.text = "丸太 2 [" + (log_count/* + quickLogCount*/) + "]";

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
		StoneReq1.text = "石ころ 6 [" + (stone_count /*+ quickStoneCount*/) + "]";

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
    /// アイテムの再計算を行うコルーチン
    /// </summary>
    /// <returns></returns>
    public IEnumerator calulate()
    {
        yield return 0;

        InventorySystem.Instance.ReCalculeList();
        RefreshNeededItems();
    }
}
