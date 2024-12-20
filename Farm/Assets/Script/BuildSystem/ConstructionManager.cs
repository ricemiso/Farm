using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionManager : MonoBehaviour
{
    public static ConstructionManager Instance { get; set; }

    public GameObject itemToBeConstructed;
    public bool inConstructionMode = false;
    public GameObject constructionHoldingSpot;

    public bool isValidPlacement;

    public bool selectingAGhost;
    public GameObject selectedGhost;


    public Material ghostSelectedMat;
    public Material ghostSemiTransparentMat;
    public Material ghostFullTransparentMat;


    public List<GameObject> allGhostsInExistence = new List<GameObject>();

    public GameObject ItemToBeDestroy;
    public GameObject ConstructionUI;

    public GameObject player;
    public GameObject placementHoldingSpot;
    public GameObject placeStorageHoldingSpot;


    [HideInInspector] public bool isConstruction = false;

    private GameObject obj;
    public bool isput;

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

    public void ActivateConstructionPlacement(string itemToConstruct)
    {
        if (constructionHoldingSpot.transform.childCount > 0) return;
       

        GameObject item = Instantiate(Resources.Load<GameObject>(itemToConstruct));


        item.name = itemToConstruct;

        item.transform.SetParent(constructionHoldingSpot.transform, false);
        itemToBeConstructed = item;
        itemToBeConstructed.gameObject.tag = "activeConstructable";


        itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = false;


        inConstructionMode = true;
    }

    public void GetAllGhosts(GameObject itemToBeConstructed)
    {
        List<GameObject> ghostlist = itemToBeConstructed.gameObject.GetComponent<Constructable>().ghostList;

        foreach (GameObject ghost in ghostlist)
        {
            Debug.Log($"Found ghost: {ghost.name}");
            allGhostsInExistence.Add(ghost);
        }

        Debug.Log($"Total ghosts in existence: {allGhostsInExistence.Count}");
    }

    private void PerformGhostDeletionScan()
    {

        foreach (GameObject ghost in allGhostsInExistence)
        {
            if (ghost != null)
            {
                if (ghost.GetComponent<GhostItem>().hasSamePosition == false)
                {
                    

                    foreach (GameObject ghostX in allGhostsInExistence)
                    {

                        if (ghost.gameObject != ghostX.gameObject)
                        {

                            if (XPositionToAccurateFloat(ghost) == XPositionToAccurateFloat(ghostX) && ZPositionToAccurateFloat(ghost) == ZPositionToAccurateFloat(ghostX))
                            {
                                if (ghost != null && ghostX != null)
                                {

                                    ghostX.GetComponent<GhostItem>().hasSamePosition = true;
                                    break;
                                }

                            }

                        }

                    }

                }
            }
        }




        foreach (GameObject ghost in allGhostsInExistence)
        {
            if (ghost != null)
            {
                if (ghost.GetComponent<GhostItem>().hasSamePosition)
                {
                    DestroyImmediate(ghost);
                }

                
            }

        }

       
    }

    private float XPositionToAccurateFloat(GameObject ghost)
    {
        if (ghost != null)
        {

            Vector3 targetPosition = ghost.gameObject.transform.position;
            float pos = targetPosition.x;
            float xFloat = Mathf.Round(pos * 100f) / 100f;
            return xFloat;
        }
        return 0;
    }

    private float ZPositionToAccurateFloat(GameObject ghost)
    {

        if (ghost != null)
        {

            Vector3 targetPosition = ghost.gameObject.transform.position;
            float pos = targetPosition.z;
            float zFloat = Mathf.Round(pos * 100f) / 100f;
            return zFloat;

        }
        return 0;
    }

    private void Update()
    {

        if (inConstructionMode)
        {
            ConstructionUI.SetActive(true);
        }
        else
        {
            ConstructionUI.SetActive(false);
        }


        if (itemToBeConstructed != null && inConstructionMode)
        {
            //TODO:配置する物の条件の追加
            if (itemToBeConstructed.name == "FoundationModel" || itemToBeConstructed.name == "ConstractAI2" 
                || itemToBeConstructed.name == "StairsWoodemodel" || itemToBeConstructed.name == "Chestmodel"
                || itemToBeConstructed.name == "TankAI2" || itemToBeConstructed.name == "LongRangeMinion 1")
            {
                if (itemToBeConstructed.name == "ConstractAI2")
                {
                    itemToBeConstructed.GetComponent<Rigidbody>().useGravity = false;
                    itemToBeConstructed.GetComponent<SupportAI_Movement>().enabled = false;
                }

                if (itemToBeConstructed.name == "TankAI2")
                {
                    itemToBeConstructed.GetComponent<Rigidbody>().useGravity = false;
                    itemToBeConstructed.GetComponent<SupportAI_Movement>().enabled = false;
                }

                if (itemToBeConstructed.name == "LongRangeMinion 1")
                {
                    itemToBeConstructed.GetComponent<Rigidbody>().useGravity = false;
                    itemToBeConstructed.GetComponent<LongRangeMinion>().enabled = false;
                }

                if (CheckValidConstructionPosition())
                {
                    isValidPlacement = true;
                    itemToBeConstructed.GetComponent<Constructable>().SetValidColor();
                }
                else
                {
                    isValidPlacement = false;
                    itemToBeConstructed.GetComponent<Constructable>().SetInvalidColor();
                }
            }
            else
            {
                itemToBeConstructed.GetComponent<Constructable>().SetInvalidColor();
            }

            


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;


            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log($"Hit: {hit.transform.name}");
                var selectionTransform = hit.transform;
                if (selectionTransform.gameObject.CompareTag("ghost") && itemToBeConstructed.name == "FoundationModel")
                {
                    itemToBeConstructed.SetActive(false);
                    selectingAGhost = true;
                    selectedGhost = selectionTransform.gameObject;

                }
                else if(selectionTransform.gameObject.CompareTag("wallghost") && itemToBeConstructed.name == "WallModel")
                {
                    itemToBeConstructed.SetActive(false);
                    selectingAGhost = true;
                    selectedGhost = selectionTransform.gameObject;   

                }
                else
                {
                    itemToBeConstructed.SetActive(true);
                    selectedGhost = null;
                    selectingAGhost = false;
                    

                }

            }
        }

        

        //TODO:配置アイテムの追加
        if (Input.GetMouseButtonDown(0) && inConstructionMode )
        {
            obj = ItemToBeDestroy;
            isput = true;

            if (isValidPlacement && selectedGhost == false && itemToBeConstructed.name == "FoundationModel")
            {
				if (SoundManager.Instance != null)
					SoundManager.Instance.PlaySound(SoundManager.Instance.PutSeSound);
                PlaceItemFreeStyle();
                HandleItemStack2(ItemToBeDestroy);
                


            }
            else if (isValidPlacement && selectedGhost == false && itemToBeConstructed.name == "ConstractAI2")
            {
                //TODO:修正する
                if(SoundManager.Instance != null)
                    SoundManager.Instance.PlaySound(SoundManager.Instance.PutSeSound);
                itemToBeConstructed.GetComponent<Rigidbody>().useGravity = true;
                itemToBeConstructed.GetComponent<SupportAI_Movement>().enabled = true;
                AIPlaceItemFreeStyle();
                obj.name = "ミニオン";

                HandleItemStack2(obj);
                isConstruction = true;
            }
            else if (isValidPlacement && selectedGhost == false && itemToBeConstructed.name == "LongRangeMinion 1")
            {
				//TODO:修正する
				if (SoundManager.Instance != null)
					SoundManager.Instance.PlaySound(SoundManager.Instance.PutSeSound);
                itemToBeConstructed.GetComponent<Rigidbody>().useGravity = true;
                itemToBeConstructed.GetComponent<LongRangeMinion>().enabled = true;
                AIPlaceItemFreeStyle();
                obj.name = "ミニオン3";
                HandleItemStack2(obj);
                isConstruction = true;
            }
            else if (isValidPlacement && selectedGhost == false && itemToBeConstructed.name == "TankAI2")
            {
				//TODO:修正する
				if (SoundManager.Instance != null)
					SoundManager.Instance.PlaySound(SoundManager.Instance.PutSeSound);
                itemToBeConstructed.GetComponent<Rigidbody>().useGravity = true;
                itemToBeConstructed.GetComponent<SupportAI_Movement>().enabled = true;
                AIPlaceItemFreeStyle();
                //TODO:配置するときはここに追加しないとスタック数が減らない
                ItemToBeDestroy.name = "ミニオン2";
                HandleItemStack2(ItemToBeDestroy);
                isConstruction = true;
            }
            else if (isValidPlacement && selectedGhost == false && itemToBeConstructed.name == "StairsWoodemodel")
            {
				if (SoundManager.Instance != null)
					SoundManager.Instance.PlaySound(SoundManager.Instance.PutSeSound);
                PlaceItemFreeStyle();
                HandleItemStack2(ItemToBeDestroy);

            }
            else if (isValidPlacement && selectedGhost == false && itemToBeConstructed.name == "Chestmodel")
            {
				if (SoundManager.Instance != null)
					SoundManager.Instance.PlaySound(SoundManager.Instance.PutSeSound);
                AIPlaceItemFreeStyle();
                HandleItemStack2(ItemToBeDestroy);
            }


            if (selectingAGhost)
            {
				if (SoundManager.Instance != null)
					SoundManager.Instance.PlaySound(SoundManager.Instance.PutSeSound);
                PlaceItemInGhostPosition(selectedGhost);
                DestroyItem(ItemToBeDestroy);
            }
        }

        if (itemToBeConstructed == null) return;

        if ((Input.GetKeyDown(KeyCode.X) && inConstructionMode) ||
     ((Input.GetAxis("Mouse ScrollWheel") != 0 || // マウスホイールの入力
       Input.GetKeyDown(KeyCode.Alpha1) ||
       Input.GetKeyDown(KeyCode.Alpha2) ||
       Input.GetKeyDown(KeyCode.Alpha3) ||
       Input.GetKeyDown(KeyCode.Alpha4) ||
       Input.GetKeyDown(KeyCode.Alpha5) ||
       Input.GetKeyDown(KeyCode.Alpha6) ||
       Input.GetKeyDown(KeyCode.Alpha7) ||
       Input.GetKeyDown(KeyCode.Alpha8) ||
       Input.GetKeyDown(KeyCode.Alpha9)) && inConstructionMode))
        {
            // ItemToBeDestroy が null でないかチェック
            if (ItemToBeDestroy != null)
            {
                ItemToBeDestroy.SetActive(true);
                ItemToBeDestroy = null; // 使用後に初期化
            }
            else
            {
                Debug.LogWarning("ItemToBeDestroy is null. Skipping SetActive.");
            }

            // 残りの処理
            DestroyItem(itemToBeConstructed);
            itemToBeConstructed = null;
            selectedGhost = null;
            inConstructionMode = false;
        }

    }

    // アイテムのスタック数を確認し、処理するメソッド
    public void HandleItemStack(GameObject item,int num =1)
    {
        var inventoryItem = item.GetComponent<InventoryItem>(); // アイテムのスタック数を持つコンポーネントを取得 

        if (inventoryItem != null)
        {
            if(num != 1)
            {
                if (inventoryItem.amountInventry <= num)
                {
                    DestroyItem(item);
                    Destroy(EquipSystem.Instance.selecteditemModel);
                }

                inventoryItem.amountInventry -= num; // スタック数を減らす

                InventorySystem.Instance.ReCalculeList();
            }
            else
            {
                // スタック数が1より大きい場合は減らす
                if (inventoryItem.amountInventry > 1)
                {
                    inventoryItem.amountInventry--; // スタック数を減らす

                    InventorySystem.Instance.ReCalculeList();
                }
                else
                {
                    // スタック数が1の場合、アイテムを削除
                    DestroyItem(item);
                    Destroy(EquipSystem.Instance.selecteditemModel);
                }
            }
            
        }
        else
        {
            Debug.LogWarning("InventoryItem コンポーネントが見つかりません");
        }
    }

    public void HandleItemStack2(GameObject item)
    {
        var itemName = ItemName(item.name); // アイテム名を取得
        string selectedItemName = itemName.Replace("(Clone)", "");

        bool itemFoundInInventory = false;

        // InventorySystemのスロットを確認
        foreach (GameObject slot in InventorySystem.Instance.slotlist)
        {
            InventrySlot inventrySlot = slot.GetComponent<InventrySlot>();

            if (inventrySlot != null)
            {
                GameObject childObject = slot.transform.GetChild(0).gameObject; // スロットの子オブジェクト

                inventrySlot.itemInSlot = inventrySlot.CheckInventryItem();

                if (inventrySlot.itemInSlot != null && inventrySlot.itemInSlot.thisName == selectedItemName)
                {
                    itemFoundInInventory = true; // インベントリにアイテムが見つかった

                    
                    if (childObject != null && !childObject.activeSelf)
                    {
                        childObject.SetActive(true);
                    }

                    if (inventrySlot.itemInSlot.amountInventry > 1)
                    {
                        inventrySlot.itemInSlot.amountInventry--; // スタック数を減らす
                        InventorySystem.Instance.ReCalculeList(); // UIやリストを更新
                    }
                    else
                    {
                        GameObject itemObject = inventrySlot.itemInSlot.gameObject;
                        if (itemObject != null)
                        {
                            InventoryItem itemComponent = itemObject.GetComponentInChildren<InventoryItem>();
                            if (itemComponent != null)
                            {
                                Destroy(itemComponent.gameObject); // 子オブジェクトを削除
                            }
                        }
                    }

                    if (childObject != null && !childObject.activeSelf)
                    {
                        childObject.SetActive(true);
                    }
                    break; // 一致するアイテムが見つかったら処理を終了
                }
            }
        }

        // インベントリにアイテムがない場合、quickSlotsListを確認
        if (!itemFoundInInventory)
        {
            foreach (GameObject slot in EquipSystem.Instance.quickSlotsList)
            {
                InventrySlot quickSlot = slot.GetComponent<InventrySlot>();
                selectedItemName = ItemName(selectedItemName);
                if (quickSlot != null && quickSlot.itemInSlot != null && quickSlot.itemInSlot.thisName == selectedItemName)
                {
                    // quickSlotsListのスロット内のアイテムを削除
                    if (quickSlot.itemInSlot.amountInventry > 1)
                    {
                        quickSlot.itemInSlot.amountInventry--;
                        InventorySystem.Instance.ReCalculeList(); // quickSlotsListのUIやリストの更新
                    }
                    else
                    {
                        GameObject itemObject = quickSlot.itemInSlot.gameObject;
                        if (itemObject != null)
                        {
                            InventoryItem itemComponent = itemObject.GetComponentInChildren<InventoryItem>();
                            if (itemComponent != null)
                            {
                                Destroy(itemComponent.gameObject);
                            }
                        }
                    }
                    break;
                }
            }
        }
    }




    private string ItemName(string itemname)
    {
        switch (itemname)
        {
            case "FoundationModel":
                itemname = "Foundation";
                break;
            case "ミニオン3(Clone)":
                itemname = "ミニオン(遠距離)";
                break;
            case "ミニオン3":
                itemname = "ミニオン(遠距離)";
                break;
            case "ミニオン2(Clone)":
                itemname = "ミニオン(タンク)";
                break;
            case "ミニオン2":
                itemname = "ミニオン(タンク)";
                break;
            case "WallModel":
                itemname = "Wall";
                break;
            case "ConstractAI2":
                itemname = "ミニオン";
                break;
            case "TankAI2":
                itemname = "ミニオン2";
                break;
            case "LongRangeMinion 1":
                itemname = "ミニオン3";
                break;
            case "StairsWoodemodel":
                itemname = "Stairs";
                break;
            case "Chestmodel":
                itemname = "Chest";
                break;
            case "Foundation(Clone)":
                itemname = "FoundationModel";
                break;
            case "Wall(Clone)":
                itemname = "WallModel";
                break;
            case "ConstractAI2(Clone)":
                itemname = "ミニオン";
                break;
            case "TankAI2(Clone)":
                itemname = "ミニオン2";
                break;
            case "LongRangeMinion 1(Clone)":
                itemname = "ミニオン3";
                break;
            case "StairsWoodemodel(Clone)":
                itemname = "Stairs";
                break;
            case "Chestmodel(Clone)":
                itemname = "Chest";
                break;
        }

        return itemname;
    }

    private void PlaceItemInGhostPosition(GameObject copyOfGhost)
    {

        Vector3 ghostPosition = copyOfGhost.transform.position;
        Quaternion ghostRotation = copyOfGhost.transform.rotation;

        selectedGhost.gameObject.SetActive(false);

        itemToBeConstructed.gameObject.SetActive(true);

        itemToBeConstructed.transform.SetParent(placementHoldingSpot.transform, true);


        var randomOffset = UnityEngine.Random.Range(0.01f, 0.03f);


        itemToBeConstructed.transform.position = new Vector3(ghostPosition.x,ghostPosition.y,ghostPosition.z+randomOffset);
        itemToBeConstructed.transform.rotation = ghostRotation;

        itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = true;


        itemToBeConstructed.GetComponent<Constructable>().SetDefaultColor();

        if(itemToBeConstructed.name == "FoundationModel")
        {
            itemToBeConstructed.GetComponent<Constructable>().ExtractGhostMembers();
            itemToBeConstructed.tag = "placedFoundation";
            GetAllGhosts(itemToBeConstructed);
            PerformGhostDeletionScan();
        }
        else
        {
            itemToBeConstructed.GetComponent<Constructable>().ExtractGhostMembers();
            itemToBeConstructed.tag = "placeWall";
            GetAllGhosts(itemToBeConstructed);
            DestroyItem(selectedGhost);
        }

        itemToBeConstructed = null;

        inConstructionMode = false;
    }


    void DestroyItem(GameObject item)
    {

        DestroyImmediate(item);
        InventorySystem.Instance.ReCalculeList();
        CraftingSystem.Instance.RefreshNeededItems();

    }


    private void PlaceItemFreeStyle()
    {

        itemToBeConstructed.transform.SetParent(placementHoldingSpot.transform, true);

        itemToBeConstructed.GetComponent<Constructable>().ExtractGhostMembers();

        itemToBeConstructed.GetComponent<Constructable>().SetDefaultColor();

        if(itemToBeConstructed.name == "FoundationModel")
        {
            itemToBeConstructed.tag = "placedFoundation";
        }
        else
        {
            itemToBeConstructed.tag = "placeStairs";
        }
        
        itemToBeConstructed.GetComponent<Constructable>().enabled = false;

        itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = true;


        GetAllGhosts(itemToBeConstructed);
        PerformGhostDeletionScan();

        itemToBeConstructed = null;

        inConstructionMode = false;
    }

    private void AIPlaceItemFreeStyle()
    {
       

        // デフォルトの色に設定
        itemToBeConstructed.GetComponent<Constructable>().SetDefaultColor();

        // タグを設定
        if(itemToBeConstructed.name == "ConstractAI2"|| itemToBeConstructed.name == "LongRangeMinion 1" || itemToBeConstructed.name == "TankAI2")
        {
            itemToBeConstructed.tag = "SupportUnit";
            // アイテムを親の下に移動
            itemToBeConstructed.transform.SetParent(placementHoldingSpot.transform, true);
        }
        else
        {
            itemToBeConstructed.tag = "Strage";
            // アイテムを親の下に移動
            itemToBeConstructed.transform.SetParent(placeStorageHoldingSpot.transform, true);
        }
        

        // solidCollider を有効にする
        itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = true;

        // アイテムを配置した後に、itemToBeConstructed を null に設定
        itemToBeConstructed = null;

        StartCoroutine(delayMode());
    }

    IEnumerator delayMode()
    {

        yield return new WaitForSeconds(1.0f);
        // 建設モードを終了
        inConstructionMode = false;
        isput = false;
    }


    private bool CheckValidConstructionPosition()
    {
        if (itemToBeConstructed != null)
        {
            return itemToBeConstructed.GetComponent<Constructable>().isValidToBeBuilt;
        }

        return false;
    }
}