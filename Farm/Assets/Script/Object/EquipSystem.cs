using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//担当者　越浦晃生

/// <summary>
/// 装備システムを管理するクラス。
/// </summary>
public class EquipSystem : MonoBehaviour
{
    /// <summary>
    /// 装備システムのインスタンス。
    /// </summary>
    public static EquipSystem Instance { get; set; }

    // -- UI -- //
    /// <summary>
    /// クイックスロットパネルのゲームオブジェクト。
    /// </summary>
    public GameObject quickSlotsPanel;

    /// <summary>
    /// クイックスロットのリスト。
    /// </summary>
    public List<GameObject> quickSlotsList = new List<GameObject>();

    /// <summary>
    /// 番号ホルダーのゲームオブジェクト。
    /// </summary>
    public GameObject numbersHolder;

    /// <summary>
    /// 選択された番号。
    /// </summary>
    public int selectedNumber = -1;

    /// <summary>
    /// 選択されたアイテム。
    /// </summary>
    public GameObject selectedItem;

    /// <summary>
    /// ツールホルダーのゲームオブジェクト。
    /// </summary>
    public GameObject toolHolder;

    /// <summary>
    /// 選択されたアイテムモデル。
    /// </summary>
    public GameObject selecteditemModel;

    /// <summary>
    /// 選択されたミニオン。
    /// </summary>
    public GameObject selectedMinion;

    /// <summary>
    /// スタックカウント。
    /// </summary>
    public int stackcnt;

    /// <summary>
    /// 選択された番号。
    /// </summary>
    public int selectednumber;

    /// <summary>
    /// スイング待機フラグ。
    /// </summary>
    public bool SwingWait;

    /// <summary>
    /// ミニオンが選択されているかどうか。
    /// </summary>
    public bool selectMinion;

    /// <summary>
    /// マナが選択されているかどうか。
    /// </summary>
    public bool selectMana;

    /// <summary>
    /// マナモデルが選択されているかどうか。
    /// </summary>
    public bool selectedManamodel;

    /// <summary>
    /// 現在選択されているオブジェクト。
    /// </summary>
    public GameObject currentSelectedObject;

    /// <summary>
    /// シングルトンパターンを適用し、インスタンスを初期化します。
    /// </summary>
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
    /// 初期設定を行います。
    /// </summary>
    private void Start()
    {
        PopulateSlotList();
        SwingWait = false;
        selectMinion = false;
        selectedManamodel = false;
    }

    /// <summary>
    /// マウスホイールでクイックスロットを変更
    /// </summary>
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


    /// <summary>
    /// 現在選択されているオブジェクトを更新します。
    /// </summary>
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
                currentSelectedObject = null; // スロットが空の場合
            }
        }
        else
        {
            currentSelectedObject = null; // 選択されていない場合
        }
    }

    /// <summary>
    /// マウスホイールでクイックスロットを切り替えます。
    /// </summary>
    /// <param name="direction">切り替える方向</param>
    private void CycleQuickSlot(int direction)
    {
        int currentSlot = GetCurrentQuickSlot();
        int newSlot = currentSlot;

        // 次のスロットを探す
        bool found = false;
        for (int i = 1; i <= quickSlotsList.Count; i++)
        {
            newSlot += direction;

            // スロット番号が範囲外になった場合、ループさせる
            if (newSlot < 1)
            {
                newSlot = quickSlotsList.Count;
            }
            else if (newSlot > quickSlotsList.Count)
            {
                newSlot = 1;
            }

            // 新しいスロットにアイテムがあるか確認
            if (checkIfSlotIsFull(newSlot))
            {
                found = true;
                break;
            }
        }

        // アイテムのあるスロットが見つかった場合に選択を切り替え
        if (found)
        {
            selectednumber = newSlot;
            SelectQuickSlot(newSlot);
        }
    }


    /// <summary>
    /// 現在選択されているクイックスロットを取得します。
    /// </summary>
    /// <returns>現在のクイックスロット番号</returns>
    public int GetCurrentQuickSlot()
    {
        if (selectedNumber <= 1)
        {
            selectedNumber = 1;
        }

        return selectedNumber;
    }

    /// <summary>
    /// クイックスロットを選択します。
    /// </summary>
    /// <param name="number">クイックスロット番号</param>
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

                if (selectedItem.name == "ミニオン" || selectedItem.name == "ミニオン2" || selectedItem.name == "ミニオン3" ||
                    selectedItem.name == "ミニオン(Clone)" || selectedItem.name == "ミニオン2(Clone)" || selectedItem.name == "ミニオン3(Clone)")
                {
                    selectedMinion = selectedItem;
                    selectMinion = true;
                    selectMana = false;
                }
                else if (selectedItem.name == "Mana" || selectedItem.name == "マナ")
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


    /// <summary>
    /// 配置するアイテムを使用する処理を行います。
    /// </summary>
    /// <param name="obj">使用するアイテムのゲームオブジェクト</param>
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
        SelectionManager.Instance.enabled = true;

        if (gameObject != null && !gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        // 配置オブジェクトの追加
        switch (obj.name)
        {
            case "Foundation":
                ConstructionManager.Instance.ActivateConstructionPlacement("FoundationModel");
                break;
            case "Wall":
                ConstructionManager.Instance.ActivateConstructionPlacement("WallModel");
                break;
            case "ミニオン":
                ConstructionManager.Instance.ActivateConstructionPlacement("ConstractAI2");
                break;
            case "ミニオン2":
                ConstructionManager.Instance.ActivateConstructionPlacement("TankAI2");
                break;
            case "ミニオン(タンク)":
                ConstructionManager.Instance.ActivateConstructionPlacement("TankAI2");
                break;
            case "ミニオン3":
                ConstructionManager.Instance.ActivateConstructionPlacement("LongRangeMinion 1");
                break;
            case "ミニオン(遠距離)":
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
            case "ミニオン(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("ConstractAI2");
                break;
            case "ミニオン2(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("TankAI2");
                break;
            case "ミニオン3(Clone)":
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

    /// <summary>
    /// 選択されたアイテムの武器ダメージを取得します。
    /// </summary>
    /// <returns>武器ダメージ</returns>
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

    /// <summary>
    /// プレイヤーが種を持っているかどうかを確認します。
    /// </summary>
    /// <returns>種を持っている場合はtrue、それ以外はfalse</returns>
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

    /// <summary>
    /// プレイヤーがじょうろを持っているかどうかを確認します。
    /// </summary>
    /// <returns>じょうろを持っている場合はtrue、それ以外はfalse</returns>
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


    /// <summary>
    /// プレイヤーがマナを持っているかどうかを確認します。
    /// </summary>
    /// <returns>マナを持っている場合はtrue、それ以外はfalse</returns>
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

    /// <summary>
    /// プレイヤーが武器を持っているかどうかを確認します。
    /// </summary>
    /// <returns>武器を持っている場合はtrue、それ以外はfalse</returns>
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

    /// <summary>
    /// 装備モデルを設定します。
    /// </summary>
    /// <param name="selectedItem">選択されたアイテム</param>
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
        // モデルをインスタンス化
        selecteditemModel = Instantiate(Resources.Load<GameObject>(instancename));

        // モデルをツールホルダーに追加
        selecteditemModel.transform.SetParent(toolHolder.transform, false);
    }

    /// <summary>
    /// アイテムモデルを計算します。
    /// </summary>
    /// <param name="selectedItemName">選択されたアイテムの名前</param>
    /// <returns>アイテムモデルの名前</returns>
    private string CaculateItemModel(string selectedItemName)
    {
        switch (selectedItemName)
        {
            case "Axe":
                // 音を再生
                SoundManager.Instance.PlaySound(SoundManager.Instance.PutSeSound);
                return "Axe_model";

            case "Stone":
                return "Stone_model";

            case "Pickaxe":
                // 音を再生
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
            case "マナ":
                return "Mana_Handmodel";

            default:
                return null;
        }
    }

    /// <summary>
    /// スイングロックがあるかどうかを確認します。
    /// </summary>
    /// <returns>スイングロックがある場合はtrue、それ以外はfalse</returns>
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

    /// <summary>
    /// 選択されたアイテムを取得します。
    /// </summary>
    /// <param name="slotnumber">スロット番号</param>
    /// <returns>選択されたアイテム</returns>
    GameObject GetSelectedItem(int slotnumber)
    {
        return quickSlotsList[slotnumber - 1].transform.GetChild(0).gameObject;
    }

    /// <summary>
    /// スロットが満杯かどうかを確認します。
    /// </summary>
    /// <param name="slotNumber">スロット番号</param>
    /// <returns>スロットが満杯の場合はtrue、それ以外はfalse</returns>
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

    /// <summary>
    /// スロットリストを生成します。
    /// </summary>
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


    /// <summary>
    /// アイテムをクイックスロットに追加します。
    /// </summary>
    /// <param name="itemToEquip">装備するアイテムのゲームオブジェクト</param>
    public void AddToQuickSlots(GameObject itemToEquip)
    {
        GameObject availableSlot = FindNextEmptySlot();

        itemToEquip.transform.SetParent(availableSlot.transform, false);
        itemToEquip.transform.SetAsFirstSibling();
        InventorySystem.Instance.ReCalculeList();
        CraftingSystem.Instance.RefreshNeededItems();
    }

    /// <summary>
    /// 次の空きスロットを見つけます。
    /// </summary>
    /// <returns>空きスロットのゲームオブジェクト</returns>
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

    /// <summary>
    /// 指定されたスロットの装備されたアイテムのスタック数を取得します。
    /// </summary>
    /// <param name="slot">スロットのゲームオブジェクト</param>
    /// <param name="itemName">アイテム名</param>
    /// <returns>スタック数</returns>
    public int GetEquippedItemStackCount(GameObject slot, string itemName)
    {
        int totalStackCount = 0;

        // 引数で渡されたスロットにアイテムがあれば
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

        return totalStackCount;
    }

    /// <summary>
    /// 指定されたスロット番号の装備されたアイテムのスタック数を取得します。
    /// </summary>
    /// <param name="slotNumber">スロット番号</param>
    /// <returns>スタック数</returns>
    public int GetEquippedItemStackCountBySlot(int slotNumber)
    {
        if (slotNumber > 0 && slotNumber <= quickSlotsList.Count)
        {
            GameObject slot = quickSlotsList[slotNumber - 1];
            string itemName = selectedItem?.GetComponent<InventoryItem>()?.thisName;

            // スロットとアイテム名が確認できたら、スタック数を取得
            if (itemName != null)
            {
                return GetEquippedItemStackCount(slot, itemName);
            }
        }
        return 0; // 無効なスロット番号やアイテムがない場合は0を返す
    }

    /// <summary>
    /// 指定されたアイテム名のスタック数を取得します。
    /// </summary>
    /// <param name="itemName">アイテム名</param>
    /// <returns>スタック数</returns>
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

    /// <summary>
    /// クイックスロットが満杯かどうかを確認します。
    /// </summary>
    /// <returns>満杯の場合はtrue、それ以外はfalse</returns>
    public bool CheckIfFull()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount >= 2)  // スロットに2つ以上のアイテムがあるか確認
            {
                return true;  // 満杯
            }
        }
        return false;  // 空きがある
    }

    /// <summary>
    /// クイックスロットからアイテムを削除します。
    /// </summary>
    /// <param name="itemName">アイテム名</param>
    /// <param name="amountToRemove">削除する数量</param>
    public void RemoveItemFromQuickSlots(string itemName, int amountToRemove)
    {
        int counter = amountToRemove;

        // quickSlotsListをスキャンしてアイテムを削除
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

                    // アイテムの残量が0になったら削除
                    if (item.amountInventry == 0)
                    {
                        Destroy(item.gameObject);
                        inventrySlot.itemInSlot = null;
                    }

                    itemName = InventorySystem.Instance.GetReturnItemName(itemName);
                }
            }

            // 必要な数だけ削除できた場合はループを抜ける
            if (counter == 0)
                break;
        }

        InventorySystem.Instance.ReCalculeList();
        CraftingSystem.Instance.RefreshNeededItems();

        // アイテム削除後の処理
    }

}