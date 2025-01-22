using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//担当者　越浦晃生

/// <summary>
/// インベントリを管理するプログラム
/// </summary>
public class InventorySystem : MonoBehaviour
{
    /// <summary>
    /// インベントリシステムのインスタンス
    /// </summary>
    public static InventorySystem Instance { get; set; }

    /// <summary>
    /// アイテム情報UI
    /// </summary>
    public GameObject ItemInfoUI;

    /// <summary>
    /// インベントリ画面UI
    /// </summary>
    public GameObject inventoryScreenUI;

    /// <summary>
    /// インベントリスロットのリスト
    /// </summary>
    public List<GameObject> slotlist = new List<GameObject>();

    /// <summary>
    /// アイテムリスト
    /// </summary>
    public List<string> itemList = new List<string>();

    /// <summary>
    /// 追加するアイテム
    /// </summary>
    private GameObject itemToAdd;

    /// <summary>
    /// 装備するスロット
    /// </summary>
    private GameObject whatSlotToEquip;

    /// <summary>
    /// インベントリが開いているかどうか
    /// </summary>
    public bool isOpen;

    /// <summary>
    /// インベントリが更新されたかどうか
    /// </summary>
    public bool inventoryUpdated;

    /// <summary>
    /// 取得したアイテムのリスト
    /// </summary>
    public List<string> itemsPickedup = new List<string>();

    /// <summary>
    /// スタック制限
    /// </summary>
    public int stackLimit = 999;

    /// <summary>
    /// アイテムがスタックしているかどうか
    /// </summary>
    public bool isStacked = false;

    //チュートリアル用の変数
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
    /// 初期化
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
    /// インベントリスロットリストの生成。
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
    /// アイテムの個数の計算
    /// インベントリ(現在廃止中)、クラフトスクリーンの開閉
    /// スロットにアイテムを適応させる
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
                // スロットからアイテムを取得
                inventrySlot.itemInSlot = inventrySlot.CheckInventryItem();

                // スロット内にアイテムがある場合
                if (inventrySlot.itemInSlot != null)
                {
                    inventrySlot.SetItemInSlot();
                }
            }
        }
    }

    /// <summary>
    /// インベントリアイテムを更新する関数。
    /// </summary>
    /// <param name="itemName">アイテム名</param>
    public void UpdateInventoryItems(string itemName)
    {
        if (!CraftingSystem.Instance.canupdate) return;

        bool itemAdded = false; // アイテムが追加されたかどうかを記録するフラグ

        // インベントリスロットリストを検索
        foreach (GameObject slot in slotlist)
        {
            InventrySlot inventrySlot = slot.GetComponent<InventrySlot>();
            if (inventrySlot != null)
            {
                // スロットからアイテムを取得
                inventrySlot.itemInSlot = inventrySlot.CheckInventryItem();

                // スロット内にアイテムがある場合
                if (inventrySlot.itemInSlot != null)
                {
                    // itemList 内の各アイテム名とスロット内のアイテム名が一致するか確認
                    foreach (string itemNameInList in itemList)
                    {
                        if (inventrySlot.itemInSlot.thisName == itemNameInList &&
                            !itemAdded &&
                            inventrySlot.itemInSlot.thisName == itemName)
                        {
                            // 一致するアイテムが見つかった場合、数量を増やす
                            inventrySlot.SetItemInSlot(); // アイテム情報を更新
                            inventrySlot.itemInSlot.amountInventry++;

                            // アイテムが追加されたことを記録し、処理を終了
                            itemAdded = true;
                            break;
                        }
                    }

                    // アイテムが追加された場合はループを終了
                    if (itemAdded)
                    {
                        return;
                    }
                }
            }
        }

        // クイックスロットリストを検索
        foreach (GameObject quickSlot in EquipSystem.Instance.quickSlotsList)
        {
            InventrySlot inventrySlot = quickSlot.GetComponent<InventrySlot>();
            if (inventrySlot != null)
            {
                // スロットからアイテムを取得
                inventrySlot.itemInSlot = inventrySlot.CheckInventryItem();

                // スロット内にアイテムがある場合
                if (inventrySlot.itemInSlot != null)
                {
                    // itemList 内の各アイテム名とスロット内のアイテム名が一致するか確認
                    foreach (string itemNameInList in itemList)
                    {
                        if (inventrySlot.itemInSlot.thisName == itemNameInList &&
                            !itemAdded &&
                            inventrySlot.itemInSlot.thisName == itemName)
                        {
                            // 一致するアイテムが見つかった場合、数量を増やす
                            inventrySlot.SetItemInSlot(); // アイテム情報を更新
                            inventrySlot.itemInSlot.amountInventry++;

                            // アイテムが追加されたことを記録し、処理を終了
                            itemAdded = true;
                            break;
                        }
                    }

                    // アイテムが追加された場合はループを終了
                    if (itemAdded)
                    {
                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// インベントリにアイテムを追加する関数。
    /// </summary>
    /// <param name="itemName">アイテム名</param>
    /// <param name="shoodStack">スタックするかどうか</param>
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

            //インベントリの場合はこっち
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
            if (itemName == "ミニオン3" || itemName == "ミニオン2" || itemName == "ミニオン")
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

        // アイテム追加後にリストを更新
        CraftingSystem.Instance.RefreshNeededItems();
    }

    /// <summary>
    /// スタック状態を設定する関数。
    /// </summary>
    /// <param name="state">スタック状態</param>
    public void SetStackState(bool state)
    {
        isStacked = state;
    }

    /// <summary>
    /// 指定したアイテムのスタックが存在するかどうかを確認する関数。
    /// </summary>
    /// <param name="itemName">アイテム名</param>
    /// <returns>条件を満たすスロットのGameObject</returns>
    private GameObject CheckIfStackExists(string itemName)
    {
        // クイックスロットリストを検索
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
                    return quickSlot; // 条件を満たしたらすぐに返す
                }
            }
        }

        // インベントリスロットリストを検索
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
                    return slot; // 条件を満たしたらすぐに返す
                }
            }
        }
        // 条件を満たすスロットが見つからなかった場合
        return null;
    }


    /// <summary>
    /// アイテム名を日本語に変換する関数。
    /// </summary>
    /// <param name="objectname">元のアイテム名</param>
    /// <returns>変換後のアイテム名</returns>
    public string GetItemName(string objectname)
    {
        switch (objectname)
        {
            case "Mana":
                objectname = "マナ";
                break;
            case "Stone":
                objectname = "石ころ";
                break;
            case "Log":
                objectname = "丸太";
                break;
            case "Axe":
                objectname = "斧";
                break;
            case "ミニオン3":
                objectname = "遠距離ミニオン";
                break;
            case "ミニオン2":
                objectname = "タンクミニオン";
                break;
            case "タンクミニオン":
                objectname = "ミニオン(タンク)";
                break;
            case "ミニオン3(Clone)":
                objectname = "遠距離ミニオン";
                break;
            case "ミニオン2(Clone)":
                objectname = "タンクミニオン";
                break;
            case "遠距離ミニオン":
                objectname = "ミニオン(遠距離)";
                break;
            case "Minion3Seed":
                objectname = "遠距離ミニオンの種";
                break;
            case "Minion2Seed":
                objectname = "タンクミニオンの種";
                break;
            case "MinionSeed":
                objectname = "ミニオンの種";
                break;
            case "Mana(Clone)":
                objectname = "マナ";
                break;
            case "Stone(Clone)":
                objectname = "石ころ";
                break;
            case "Log(Clone)":
                objectname = "丸太";
                break;
            case "Axe(Clone)":
                objectname = "斧";
                break;
            default:
                return objectname;
        }

        return objectname;
    }

    /// <summary>
    /// アイテム名を英語に戻す関数。
    /// </summary>
    /// <param name="objectname">変換後のアイテム名</param>
    /// <returns>元のアイテム名</returns>
    public string GetReturnItemName(string objectname)
    {
        switch (objectname)
        {
            case "マナ":
                objectname = "Mana";
                break;
            case "石ころ":
                objectname = "Stone";
                break;
            case "丸太":
                objectname = "Log";
                break;
            case "斧":
                objectname = "Axe";
                break;
            case "遠距離ミニオン":
                objectname = "ミニオン3";
                break;
            case "タンクミニオン":
                objectname = "ミニオン2";
                break;
            case "遠距離ミニオンの種":
                objectname = "Minion3Seed";
                break;
            case "タンクミニオンの種":
                objectname = "Minion2Seed";
                break;
            case "ミニオンの種":
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
    /// インベントリにアイテムをロードする関数。
    /// </summary>
    /// <param name="itemName">アイテム名</param>
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
    /// 次の空きスロットを見つける関数。
    /// </summary>
    /// <returns>次の空きスロットのGameObject</returns>
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
    /// クイックスロットから次の空きスロットを見つける関数。
    /// </summary>
    /// <returns>次の空きスロットのGameObject</returns>
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
    /// クイックスロットから特定のアイテム名に基づいて次の空きスロットを見つける関数。
    /// </summary>
    /// <param name="itemName">アイテム名</param>
    /// <returns>次の空きスロットのGameObject</returns>
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
            case "ミニオン2":
                slotIndex = 3;
                break;
            case "ミニオン3":
                slotIndex = 4;
                break;
            case "ミニオン":
                slotIndex = 2;
                break;
            default:
                break;
        }

        GameObject slot = EquipSystem.Instance.quickSlotsList[slotIndex];

        return slot;
    }

    /// <summary>
    /// 指定された空きスロットの数が利用可能かどうかを確認する関数。
    /// </summary>
    /// <param name="emptyNeeded">必要な空きスロットの数</param>
    /// <returns>空きスロットが利用可能な場合はtrue、そうでない場合はfalse</returns>
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
    /// 指定されたアイテムをインベントリから削除する関数。
    /// </summary>
    /// <param name="itemName">アイテム名</param>
    /// <param name="amountToRemove">削除する数量</param>
    public void RemoveItem(string itemName, int amountToRemove)
    {
        int remainingAmountToRemove = amountToRemove;

        // スロットリストを反復処理し、削除するアイテムがある限りインベントリを減らす
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

                    // スロット内の数量が0になった場合、アイテムを削除
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
                break; // すべてのアイテムが削除されたらループを早期終了
        }
    }


    /// <summary>
    /// インベントリリストを再計算する関数。
    /// </summary>
    public void ReCalculeList()
    {
        itemList.Clear();  // リストをクリア

        // インベントリスロットを更新
        foreach (GameObject slot in slotlist)
        {
            if (slot.GetComponent<InventrySlot>())
            {
                InventoryItem item = slot.GetComponent<InventrySlot>().itemInSlot;

                if (item != null && item.amountInventry > 0)
                {
                    // アイテムのスタック分だけリストに追加
                    for (int i = 0; i < item.amountInventry; i++)
                    {
                        itemList.Add(item.thisName);
                    }
                }
            }
        }

        // クイックスロットも更新
        foreach (GameObject quickSlot in EquipSystem.Instance.quickSlotsList)
        {
            if (quickSlot.GetComponent<InventrySlot>())
            {
                InventoryItem item = quickSlot.GetComponent<InventrySlot>().itemInSlot;

                if (item != null && item.amountInventry > 0)
                {
                    // アイテムのスタック分だけリストに追加
                    for (int i = 0; i < item.amountInventry; i++)
                    {
                        itemList.Add(item.thisName);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 指定されたアイテムの総数を取得する関数。
    /// </summary>
    /// <param name="itemName">アイテム名</param>
    /// <returns>アイテムの総数</returns>
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
    /// 指定されたアイテムのインベントリ内の数量を取得する関数。
    /// </summary>
    /// <param name="itemName">アイテム名</param>
    /// <returns>インベントリ内のアイテムの数量</returns>
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