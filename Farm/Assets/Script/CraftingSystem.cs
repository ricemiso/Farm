using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem Instance { get; set; }

    public GameObject craftingScreenUI;
    public GameObject toolScreenUI, survivalScreenUI, refineScreenUI;

    public List<string> inventryitemList = new List<string>();

    Button toolsBTN, survivalBTN, refineBTN;
    Button craftAxeBTN, craftPlankBTN;

    Text AxeReq1, AxeReq2, PlankReq1;

    public bool isOpen;


    [HideInInspector] public BluePrint AxeBLP;
    [HideInInspector] public BluePrint PlankBLP;


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
        AxeBLP = new BluePrint("Axe", 1, 2, "Stone", 3, "Stick", 3);
        PlankBLP = new BluePrint("Plank", 2, 1, "Log", 1, "", 0);

        isOpen = false;

        toolsBTN = craftingScreenUI.transform.Find("ToolButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate { OpenToolsCategory(); });

        survivalBTN = craftingScreenUI.transform.Find("SurvivalButton").GetComponent<Button>();
        survivalBTN.onClick.AddListener(delegate { OpenSurvivalCategory(); });

        refineBTN = craftingScreenUI.transform.Find("RefineButton").GetComponent<Button>();
        refineBTN.onClick.AddListener(delegate { OpenRefineCategory(); });

        //Axe
        AxeReq1 = toolScreenUI.transform.Find("Axe").transform.Find("rec1").GetComponent<Text>();
        AxeReq2 = toolScreenUI.transform.Find("Axe").transform.Find("rec2").GetComponent<Text>();

        craftAxeBTN = toolScreenUI.transform.Find("Axe").transform.Find("AxeButton").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });


        //Plank
        PlankReq1 = refineScreenUI.transform.Find("Plank").transform.Find("rec1").GetComponent<Text>();

        craftPlankBTN = refineScreenUI.transform.Find("Plank").transform.Find("PlankButton").GetComponent<Button>();
        craftPlankBTN.onClick.AddListener(delegate { CraftAnyItem(PlankBLP); });

    }

    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        toolScreenUI.SetActive(true);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
    }

    void OpenSurvivalCategory()
    {
        toolScreenUI.SetActive(false);
        craftingScreenUI.SetActive(false);
        survivalScreenUI.SetActive(true);
        refineScreenUI.SetActive(false);
    }


    void OpenRefineCategory()
    {
        toolScreenUI.SetActive(false);
        craftingScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(true);
    }




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
            survivalScreenUI.SetActive(false);
            refineScreenUI.SetActive(false);

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

    

    private void CraftAnyItem(BluePrint blueprintToCraft)
    {

        SoundManager.Instance.PlaySound(SoundManager.Instance.craftingSound);


        for (var i = 0; i < blueprintToCraft.numberOfItemsToProduce; i++)
        {
            InventorySystem.Instance.AddToinventry(blueprintToCraft.itemName);
        }
        


        // 現在のインベントリ内の素材の数を取得
        int currentAmountReq1 = InventorySystem.Instance.GetItemCount(blueprintToCraft.Req1);
        int currentAmountReq2 = blueprintToCraft.numOfRequirement == 2 ? InventorySystem.Instance.GetItemCount(blueprintToCraft.Req2) : 0;

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

        // インベントリから必要なアイテムを削除する
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
        StartCoroutine(calulate());
    }


    public void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;
        int log_count = 0;

        // インベントリ内のアイテム数をカウント
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
                case "Log":
                    log_count += 1;
                    break;
            }
        }

        // クイックスロット内のアイテムもカウント
        int quickStoneCount = 0;
        int quickStickCount = 0;
        int quickLogCount = 0;

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
                else if (itemName == "Log")
                {
                    quickLogCount++;
                }
            }
        }

        // ----Axe---- //
        AxeReq1.text = "3 Stone [" + (stone_count + quickStoneCount) + "]";
        AxeReq2.text = "3 Stick [" + (stick_count + quickStickCount) + "]";

        if ((stone_count + quickStoneCount) >= 3 && (stick_count + quickStickCount) >= 3
            && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            craftAxeBTN.gameObject.SetActive(true);
        }
        else
        {
            craftAxeBTN.gameObject.SetActive(false);
        }


        // ----Plank x2---- //
        PlankReq1.text = "1 Log [" + (log_count + quickLogCount) + "]";

        if ((log_count + quickLogCount) >= 1 && InventorySystem.Instance.CheckSlotAvailable(2))
        {
            craftPlankBTN.gameObject.SetActive(true);
        }
        else
        {
            craftPlankBTN.gameObject.SetActive(false);
        }
    }


    public IEnumerator calulate()
    {
        yield return 0;

        InventorySystem.Instance.ReCalculeList();
        RefreshNeededItems();
    }
}
