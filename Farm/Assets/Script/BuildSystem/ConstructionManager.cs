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
            if (isValidPlacement && selectedGhost == false && itemToBeConstructed.name == "FoundationModel")
            {
				if (SoundManager.Instance != null)
					SoundManager.Instance.PlaySound(SoundManager.Instance.PutSeSound);
                PlaceItemFreeStyle();
                HandleItemStack2(ItemToBeDestroy);


            }else if (isValidPlacement && selectedGhost == false && itemToBeConstructed.name == "ConstractAI2")
            {
                //TODO:修正する
                if(SoundManager.Instance != null)
                    SoundManager.Instance.PlaySound(SoundManager.Instance.PutSeSound);
                itemToBeConstructed.GetComponent<Rigidbody>().useGravity = true;
                itemToBeConstructed.GetComponent<SupportAI_Movement>().enabled = true;
                AIPlaceItemFreeStyle();

                HandleItemStack2(ItemToBeDestroy);
            }
            else if (isValidPlacement && selectedGhost == false && itemToBeConstructed.name == "LongRangeMinion 1")
            {
				//TODO:修正する
				if (SoundManager.Instance != null)
					SoundManager.Instance.PlaySound(SoundManager.Instance.PutSeSound);
                itemToBeConstructed.GetComponent<Rigidbody>().useGravity = true;
                itemToBeConstructed.GetComponent<LongRangeMinion>().enabled = true;
                AIPlaceItemFreeStyle();

                HandleItemStack2(ItemToBeDestroy);
            }
            else if (isValidPlacement && selectedGhost == false && itemToBeConstructed.name == "TankAI2")
            {
				//TODO:修正する
				if (SoundManager.Instance != null)
					SoundManager.Instance.PlaySound(SoundManager.Instance.PutSeSound);
                itemToBeConstructed.GetComponent<Rigidbody>().useGravity = true;
                itemToBeConstructed.GetComponent<SupportAI_Movement>().enabled = true;
                AIPlaceItemFreeStyle();

                HandleItemStack2(ItemToBeDestroy);
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
       
        if (Input.GetKeyDown(KeyCode.X) && inConstructionMode)
        {    

            ItemToBeDestroy.SetActive(true);
            ItemToBeDestroy = null;

            DestroyItem(itemToBeConstructed);
            itemToBeConstructed = null;
            selectedGhost = null;
            inConstructionMode = false;

        }
    }

    // アイテムのスタック数を確認し、処理するメソッド
    public void HandleItemStack(GameObject item)
    {
        var inventoryItem = item.GetComponent<InventoryItem>(); // アイテムのスタック数を持つコンポーネントを取得 

        if (inventoryItem != null)
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
            }
        }
        else
        {
            Debug.LogWarning("InventoryItem コンポーネントが見つかりません");
        }
    }

    public void HandleItemStack2(GameObject item)
    {
        var itemName = ItemName(item); // アイテム名を取得
        string selectedItemName = itemName.Replace("(Clone)", "");

        foreach (GameObject slot in InventorySystem.Instance.slotlist)
        {
            InventrySlot inventrySlot = slot.GetComponent<InventrySlot>();

            if (inventrySlot != null)
            {
                // スロットの子オブジェクトを取得
                GameObject childObject = slot.transform.GetChild(0).gameObject; // 子オブジェクトが1つだけだと仮定している場合

              

                // スロットからアイテムを取得
                inventrySlot.itemInSlot = inventrySlot.CheckInventryItem();

                // スロット内にアイテムがある場合
                if (inventrySlot.itemInSlot != null)
                {
                    // スロット内のアイテム名が一致するか確認
                    if (inventrySlot.itemInSlot.thisName == selectedItemName)
                    {
                        // 子オブジェクトが非アクティブであれば、アクティブにする
                        if (childObject != null && !childObject.activeSelf)
                        {
                            childObject.SetActive(true);
                        }
                        // スタック数が1より多ければスタック数を減らす
                        if (inventrySlot.itemInSlot.amountInventry > 1)
                        {
                            inventrySlot.itemInSlot.amountInventry--; // スタック数を減らす
                            InventorySystem.Instance.ReCalculeList(); // アイテムのUIやリストの更新
                        }
                        else
                        {
                            // スタック数が1の場合、アイテムを削除
                            // アイテムの子オブジェクトにアクセスして削除
                            GameObject itemObject = inventrySlot.itemInSlot.gameObject;

                            // アイテムに関連する子オブジェクト（InventoryItemコンポーネントを持つオブジェクト）を探して削除
                            if (itemObject != null)
                            {
                                // InventoryItemコンポーネントを持つ子オブジェクトを削除
                                InventoryItem itemComponent = itemObject.GetComponentInChildren<InventoryItem>();
                                if (itemComponent != null)
                                {
                                    Destroy(itemComponent.gameObject); // 子オブジェクトを削除
                                }
                            }
                        }
                        // 子オブジェクトが非アクティブであれば、アクティブにする
                        if (childObject != null && !childObject.activeSelf)
                        {
                            childObject.SetActive(true);
                        }
                        break; // 一致するアイテムが見つかったら処理を終了
                    }
                }

                
            }
        }
    }



    private string ItemName(GameObject itemname)
    {
        switch (itemname.name)
        {
            case "FoundationModel":
                itemname.name = "Foundation";
                break;
            case "ミニオン3(Clone)":
                itemname.name = "ミニオン(遠距離)";
                break;
            case "ミニオン2(Clone)":
                itemname.name = "ミニオン(タンク)";
                break;
            case "WallModel":
                itemname.name = "Wall";
                break;
            case "ConstractAI2":
                itemname.name = "ミニオン";
                break;
            case "TankAI2":
                itemname.name = "ミニオン2";
                break;
            case "LongRangeMinion 1":
                itemname.name = "ミニオン3";
                break;
            case "StairsWoodemodel":
                itemname.name = "Stairs";
                break;
            case "Chestmodel":
                itemname.name = "Chest";
                break;
            case "Foundation(Clone)":
                itemname.name = "FoundationModel";
                break;
            case "Wall(Clone)":
                itemname.name = "WallModel";
                break;
            case "ConstractAI2(Clone)":
                itemname.name = "ミニオン";
                break;
            case "TankAI2(Clone)":
                itemname.name = "ミニオン2";
                break;
            case "LongRangeMinion 1(Clone)":
                itemname.name = "ミニオン3";
                break;
            case "StairsWoodemodel(Clone)":
                itemname.name = "Stairs";
                break;
            case "Chestmodel(Clone)":
                itemname.name = "Chest";
                break;
        }

        return itemname.name;
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
        if(itemToBeConstructed.name == "ConstractAI2")
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