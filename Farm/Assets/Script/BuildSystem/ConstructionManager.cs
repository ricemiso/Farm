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
                || itemToBeConstructed.name == "StairsWoodemodel" || itemToBeConstructed.name == "Chestmodel")
            {
                if(itemToBeConstructed.name == "ConstractAI2")
                {
                    itemToBeConstructed.GetComponent<Rigidbody>().useGravity = false;
                    itemToBeConstructed.GetComponent<SupportAI_Movement>().enabled = false;
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
                SoundManager.Instance.PlaySound(SoundManager.Instance.PutSeSound);
                PlaceItemFreeStyle();
                DestroyItem(ItemToBeDestroy);


            }else if (isValidPlacement && selectedGhost == false && itemToBeConstructed.name == "ConstractAI2")
            {
                //TODO:修正する
                SoundManager.Instance.PlaySound(SoundManager.Instance.PutSeSound);
                itemToBeConstructed.GetComponent<Rigidbody>().useGravity = true;
                itemToBeConstructed.GetComponent<SupportAI_Movement>().enabled = true;
                AIPlaceItemFreeStyle();
                
                DestroyItem(ItemToBeDestroy);
            }
            else if (isValidPlacement && selectedGhost == false && itemToBeConstructed.name == "StairsWoodemodel")
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.PutSeSound);
                PlaceItemFreeStyle();
                DestroyItem(ItemToBeDestroy);

            }else if (isValidPlacement && selectedGhost == false && itemToBeConstructed.name == "Chestmodel")
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.PutSeSound);
                AIPlaceItemFreeStyle();
                DestroyItem(ItemToBeDestroy);
            }


            if (selectingAGhost)
            {
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

        DestroyImmediate(item);

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