using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�S���ҁ@�z�Y�W��

/// <summary>
/// ���z�Ǘ����s���N���X�B���z���[�h�̊Ǘ��⌚�z���A�S�[�X�g�I�u�W�F�N�g�̑���B
/// </summary>
public class ConstructionManager : MonoBehaviour
{
    /// <summary>
    /// �N���X�̃C���X�^���X��ێ�����V���O���g���v���p�e�B�B
    /// </summary>
    public static ConstructionManager Instance { get; set; }

    /// <summary>
    /// ���݌��z���̃A�C�e���B
    /// </summary>
    public GameObject itemToBeConstructed;

    /// <summary>
    /// ���z���[�h���L�����ǂ����������܂��B
    /// </summary>
    public bool inConstructionMode = false;

    /// <summary>
    /// ���z���̃A�C�e����ێ�����X�|�b�g�B
    /// </summary>
    public GameObject constructionHoldingSpot;

    /// <summary>
    /// ���݂̔z�u���L�����ǂ����������܂��B
    /// </summary>
    public bool isValidPlacement;

    /// <summary>
    /// �S�[�X�g�I�u�W�F�N�g��I�𒆂��ǂ����������܂��B
    /// </summary>
    public bool selectingAGhost;

    /// <summary>
    /// ���ݑI������Ă���S�[�X�g�I�u�W�F�N�g�B
    /// </summary>
    public GameObject selectedGhost;

    /// <summary>
    /// �I�����ꂽ�S�[�X�g�̃}�e���A���B
    /// </summary>
    public Material ghostSelectedMat;

    /// <summary>
    /// �������̃S�[�X�g�̃}�e���A���B
    /// </summary>
    public Material ghostSemiTransparentMat;

    /// <summary>
    /// ���S�����̃S�[�X�g�̃}�e���A���B
    /// </summary>
    public Material ghostFullTransparentMat;

    /// <summary>
    /// ���݂��邷�ׂẴS�[�X�g�I�u�W�F�N�g�̃��X�g�B
    /// </summary>
    public List<GameObject> allGhostsInExistence = new List<GameObject>();

    /// <summary>
    /// �폜�Ώۂ̃A�C�e���B
    /// </summary>
    public GameObject ItemToBeDestroy;

    /// <summary>
    /// ���z�֘A��UI�B
    /// </summary>
    public GameObject ConstructionUI;

    /// <summary>
    /// �v���C���[�̃I�u�W�F�N�g�B
    /// </summary>
    public GameObject player;

    /// <summary>
    /// �z�u���̃I�u�W�F�N�g��ێ�����X�|�b�g�B
    /// </summary>
    public GameObject placementHoldingSpot;

    /// <summary>
    /// �X�g���[�W�p�̕ێ��X�|�b�g�B
    /// </summary>
    public GameObject placeStorageHoldingSpot;

    /// <summary>
    /// ���z���i�s�����ǂ����������t���O�B
    /// </summary>
    [HideInInspector] public bool isConstruction = false;

    /// <summary>
    /// �ꎞ�I�Ɏg�p����Q�[���I�u�W�F�N�g�B
    /// </summary>
    private GameObject obj;

    /// <summary>
    /// �z�u���������Ă��邩�������t���O�B
    /// </summary>
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

    /// <summary>
    /// �w�肳�ꂽ�A�C�e�������z���[�h�Őݒu�\�ȏ�Ԃɂ���B
    /// </summary>
    /// <param name="itemToConstruct">�z�u����A�C�e���̖��O</param>
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


    /// <summary>
    /// �w�肳�ꂽ�A�C�e���Ɋ֘A�t����ꂽ�S�ẴS�[�X�g�I�u�W�F�N�g���擾���A���X�g�ɒǉ����܂��B
    /// </summary>
    /// <param name="itemToBeConstructed">���z�Ώۂ̃A�C�e��</param>
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

    /// <summary>
    /// �S�[�X�g�I�u�W�F�N�g���X�L�������A�����ʒu�ɂ���d������S�[�X�g���폜���܂��B
    /// </summary>
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


    /// <summary>
    /// �S�[�X�g�I�u�W�F�N�g��X���W�������_��2�ʂ܂Ő��m�ɐ؂�̂Ă��l��Ԃ��܂��B
    /// </summary>
    /// <param name="ghost">�Ώۂ̃S�[�X�g�I�u�W�F�N�g</param>
    /// <returns>�؂�̂Č��X���W</returns>
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

    /// <summary>
    /// �S�[�X�g�I�u�W�F�N�g��Z���W�������_��2�ʂ܂Ő��m�ɐ؂�̂Ă��l��Ԃ��܂��B
    /// </summary>
    /// <param name="ghost">�Ώۂ̃S�[�X�g�I�u�W�F�N�g</param>
    /// <returns>�؂�̂Č��Z���W</returns>
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


    /// <summary>
    /// �I�u�W�F�N�g���z�u�ł�������Ȃ�ΑI�΂�Ă���I�u�W�F�N�g�����z���[�h�Ƃ��Ĕz�u����
    /// </summary>
    private void Update()
    {
        if (constructionHoldingSpot.transform.childCount > 0) inConstructionMode = true;


        //if (inConstructionMode)
        //{
        //    ConstructionUI.SetActive(true);
        //}
        //else
        //{
        //    ConstructionUI.SetActive(false);
        //}

        if (itemToBeConstructed != null && inConstructionMode)
        {
            //TODO:�z�u���镨�̏����̒ǉ�
            if (itemToBeConstructed.name == "FoundationModel" || itemToBeConstructed.name == "ConstractAI2" 
                || itemToBeConstructed.name == "StairsWoodemodel" || itemToBeConstructed.name == "Chestmodel"
                || itemToBeConstructed.name == "TankAI2" || itemToBeConstructed.name == "LongRangeMinion 1")
            {
                if (itemToBeConstructed.name == "ConstractAI2")
                {
                    itemToBeConstructed.GetComponent<Rigidbody>().useGravity = false;
                    itemToBeConstructed.GetComponent<Rigidbody>().isKinematic = true;
                    itemToBeConstructed.GetComponent<SupportAI_Movement>().enabled = false;
                    itemToBeConstructed.GetComponent<Animation>().enabled = false;
                }

                if (itemToBeConstructed.name == "TankAI2")
                {
                    itemToBeConstructed.GetComponent<Rigidbody>().useGravity = false;
                    itemToBeConstructed.GetComponent<SupportAI_Movement>().enabled = false;
                    itemToBeConstructed.GetComponent<Animation>().enabled = false;
                    itemToBeConstructed.GetComponent<Rigidbody>().isKinematic = true;
                }

                if (itemToBeConstructed.name == "LongRangeMinion 1")
                {
                    itemToBeConstructed.GetComponent<Rigidbody>().useGravity = false;
                    itemToBeConstructed.GetComponent<LongRangeMinion>().enabled = false;
                    itemToBeConstructed.GetComponent<Animation>().enabled = false;
                    itemToBeConstructed.GetComponent<Rigidbody>().isKinematic = true;
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

        

        //TODO:�z�u�A�C�e���̒ǉ�
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
                //TODO:�C������
                if(SoundManager.Instance != null)
                    SoundManager.Instance.PlaySound(SoundManager.Instance.PutSeSound);
                //itemToBeConstructed.GetComponent<Rigidbody>().useGravity = true;
                itemToBeConstructed.GetComponent<Rigidbody>().isKinematic = false;
                itemToBeConstructed.GetComponent<SupportAI_Movement>().enabled = true;
                AIPlaceItemFreeStyle();
                obj.name = "�~�j�I��";

                HandleItemStack2(obj);
                isConstruction = true;
            }
            else if (isValidPlacement && selectedGhost == false && itemToBeConstructed.name == "LongRangeMinion 1")
            {
				//TODO:�C������
				if (SoundManager.Instance != null)
					SoundManager.Instance.PlaySound(SoundManager.Instance.PutSeSound);
                //itemToBeConstructed.GetComponent<Rigidbody>().useGravity = true;
                itemToBeConstructed.GetComponent<Rigidbody>().isKinematic = false;
                itemToBeConstructed.GetComponent<LongRangeMinion>().enabled = true;
                AIPlaceItemFreeStyle();
                obj.name = "�~�j�I��3";
                HandleItemStack2(obj);
                isConstruction = true;
            }
            else if (isValidPlacement && selectedGhost == false && itemToBeConstructed.name == "TankAI2")
            {
				//TODO:�C������
				if (SoundManager.Instance != null)
					SoundManager.Instance.PlaySound(SoundManager.Instance.PutSeSound);
                //itemToBeConstructed.GetComponent<Rigidbody>().useGravity = true;
                itemToBeConstructed.GetComponent<Rigidbody>().isKinematic = false;
                itemToBeConstructed.GetComponent<SupportAI_Movement>().enabled = true;
                AIPlaceItemFreeStyle();
                //TODO:�z�u����Ƃ��͂����ɒǉ����Ȃ��ƃX�^�b�N��������Ȃ�
                ItemToBeDestroy.name = "�~�j�I��2";
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
     ((Input.GetAxis("Mouse ScrollWheel") != 0 || // �}�E�X�z�C�[���̓���
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
            // ItemToBeDestroy �� null �łȂ����`�F�b�N
            if (ItemToBeDestroy != null)
            {
                ItemToBeDestroy.SetActive(true);
                ItemToBeDestroy = null; // �g�p��ɏ�����
            }
            else
            {
                Debug.LogWarning("ItemToBeDestroy is null. Skipping SetActive.");
            }

            // �c��̏���
            DestroyItem(itemToBeConstructed);
            itemToBeConstructed = null;
            selectedGhost = null;
            inConstructionMode = false;
        }

    }

    /// <summary>
    /// �A�C�e���̃X�^�b�N�����m�F���A�폜�������郁�\�b�h
    /// </summary>
    /// <param name="item">�X�^�b�N���Ă���A�C�e��</param>
    /// <param name="num">���炷��</param>
    public void HandleItemStack(GameObject item,int num =1)
    {
        var inventoryItem = item.GetComponent<InventoryItem>(); // �A�C�e���̃X�^�b�N�������R���|�[�l���g���擾 

        if (inventoryItem != null)
        {
            if(num != 1)
            {
                if (inventoryItem.amountInventry <= num)
                {
                    DestroyItem(item);
                    Destroy(EquipSystem.Instance.selecteditemModel);
                }

                inventoryItem.amountInventry -= num; // �X�^�b�N�������炷

                InventorySystem.Instance.ReCalculeList();
            }
            else
            {
                // �X�^�b�N����1���傫���ꍇ�͌��炷
                if (inventoryItem.amountInventry > 1)
                {
                    inventoryItem.amountInventry--; // �X�^�b�N�������炷

                    InventorySystem.Instance.ReCalculeList();
                }
                else
                {
                    // �X�^�b�N����1�̏ꍇ�A�A�C�e�����폜
                    DestroyItem(item);
                    Destroy(EquipSystem.Instance.selecteditemModel);
                }
            }
            
        }
        else
        {
            Debug.LogWarning("InventoryItem �R���|�[�l���g��������܂���");
        }
    }

    /// <summary>
    /// �X�^�b�N���ꂽ�A�C�e�����폜����
    /// �C���x���g���ƃN�C�b�N�X���b�g�œ����A�C�e��������ꍇ�A�Е��̂ݍ폜����
    /// </summary>
    /// <param name="item">�폜����A�C�e��</param>
    public void HandleItemStack2(GameObject item)
    {
        var itemName = ItemName(item.name); // �A�C�e�������擾
        string selectedItemName = itemName.Replace("(Clone)", "");

        bool itemFoundInInventory = false;

        // InventorySystem�̃X���b�g���m�F
        foreach (GameObject slot in InventorySystem.Instance.slotlist)
        {
            InventrySlot inventrySlot = slot.GetComponent<InventrySlot>();

            if (inventrySlot != null)
            {
                GameObject childObject = slot.transform.GetChild(0).gameObject; // �X���b�g�̎q�I�u�W�F�N�g

                inventrySlot.itemInSlot = inventrySlot.CheckInventryItem();

                if (inventrySlot.itemInSlot != null && inventrySlot.itemInSlot.thisName == selectedItemName)
                {
                    itemFoundInInventory = true; // �C���x���g���ɃA�C�e������������

                    
                    if (childObject != null && !childObject.activeSelf)
                    {
                        childObject.SetActive(true);
                    }

                    if (inventrySlot.itemInSlot.amountInventry > 1)
                    {
                        inventrySlot.itemInSlot.amountInventry--; // �X�^�b�N�������炷
                        InventorySystem.Instance.ReCalculeList(); // UI�⃊�X�g���X�V
                    }
                    else
                    {
                        GameObject itemObject = inventrySlot.itemInSlot.gameObject;
                        if (itemObject != null)
                        {
                            InventoryItem itemComponent = itemObject.GetComponentInChildren<InventoryItem>();
                            if (itemComponent != null)
                            {
                                Destroy(itemComponent.gameObject); // �q�I�u�W�F�N�g���폜
                            }
                        }
                    }

                    if (childObject != null && !childObject.activeSelf)
                    {
                        childObject.SetActive(true);
                    }
                    break; // ��v����A�C�e�������������珈�����I��
                }
            }
        }

        // �C���x���g���ɃA�C�e�����Ȃ��ꍇ�AquickSlotsList���m�F
        if (!itemFoundInInventory)
        {
            foreach (GameObject slot in EquipSystem.Instance.quickSlotsList)
            {
                InventrySlot quickSlot = slot.GetComponent<InventrySlot>();
                selectedItemName = ItemName(selectedItemName);
                if (quickSlot != null && quickSlot.itemInSlot != null && quickSlot.itemInSlot.thisName == selectedItemName)
                {
                    // quickSlotsList�̃X���b�g���̃A�C�e�����폜
                    if (quickSlot.itemInSlot.amountInventry > 1)
                    {
                        quickSlot.itemInSlot.amountInventry--;
                        InventorySystem.Instance.ReCalculeList(); // quickSlotsList��UI�⃊�X�g�̍X�V
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



    /// <summary>
    /// �A�C�e������ϊ�����֐�
    /// </summary>
    /// <param name="itemname">�A�C�e����</param>
    /// <returns></returns>
    private string ItemName(string itemname)
    {
        switch (itemname)
        {
            case "FoundationModel":
                itemname = "Foundation";
                break;
            case "�~�j�I��3(Clone)":
                itemname = "�~�j�I��(������)";
                break;
            case "�~�j�I��3":
                itemname = "�~�j�I��(������)";
                break;
            case "�~�j�I��2(Clone)":
                itemname = "�~�j�I��(�^���N)";
                break;
            case "�~�j�I��2":
                itemname = "�~�j�I��(�^���N)";
                break;
            case "WallModel":
                itemname = "Wall";
                break;
            case "ConstractAI2":
                itemname = "�~�j�I��";
                break;
            case "TankAI2":
                itemname = "�~�j�I��2";
                break;
            case "LongRangeMinion 1":
                itemname = "�~�j�I��3";
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
                itemname = "�~�j�I��";
                break;
            case "TankAI2(Clone)":
                itemname = "�~�j�I��2";
                break;
            case "LongRangeMinion 1(Clone)":
                itemname = "�~�j�I��3";
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

    /// <summary>
    /// �A�C�e�����S�[�X�g�ʒu�ɔz�u�B  
    /// �S�[�X�g�I�u�W�F�N�g�̈ʒu�Ɖ�]���擾���A�����_���ȃI�t�Z�b�g�������Ĕz�u�B
    /// �A�C�e���̎�ނɉ����ĈقȂ�^�O��ݒ肵�A�S�[�X�g�̃����o�[�𒊏o�A�폜�������s���B
    /// </summary>
    /// <param name="copyOfGhost">�S�[�X�g�I�u�W�F�N�g</param>
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



    /// <summary>
    /// �w�肳�ꂽ�A�C�e���𑦎��ɍ폜����B  
    /// �A�C�e�����폜���A�C���x���g���ƃN���t�g�V�X�e�����X�V����B
    /// </summary>
    /// <param name="item">�폜����I�u�W�F�N�g</param>
    void DestroyItem(GameObject item)
    {

        DestroyImmediate(item);
        InventorySystem.Instance.ReCalculeList();
        CraftingSystem.Instance.RefreshNeededItems();

    }

    /// <summary>
    /// �A�C�e����z�u����֐��B  
    /// </summary>
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

    /// <summary>
    /// �~�j�I����z�u����֐�
    /// </summary>
    private void AIPlaceItemFreeStyle()
    {
        // �f�t�H���g�̐F�ɐݒ�
        itemToBeConstructed.GetComponent<Constructable>().SetDefaultColor();

        // �^�O��ݒ�
        if(itemToBeConstructed.name == "ConstractAI2"|| itemToBeConstructed.name == "LongRangeMinion 1" || itemToBeConstructed.name == "TankAI2")
        {
            itemToBeConstructed.tag = "SupportUnit";
            // �A�C�e����e�̉��Ɉړ�
            itemToBeConstructed.transform.SetParent(placementHoldingSpot.transform, true);
        }
        else
        {
            itemToBeConstructed.tag = "Strage";
            // �A�C�e����e�̉��Ɉړ�
            itemToBeConstructed.transform.SetParent(placeStorageHoldingSpot.transform, true);
        }
        

        // solidCollider ��L���ɂ���
        itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = true;
        itemToBeConstructed.GetComponent<Animation>().enabled = true;

        // �A�C�e����z�u������ɁAitemToBeConstructed �� null �ɐݒ�
        itemToBeConstructed = null;

        StartCoroutine(delayMode());
    }

    /// <summary>
    /// ��莞�Ԍ�Ɍ��݃��[�h���I������B  
    /// �x����݂��Č��݃��[�h�̃t���O���X�V����B
    /// </summary>
    IEnumerator delayMode()
    {
        yield return new WaitForSeconds(1.0f);
        // ���݃��[�h���I��
        inConstructionMode = false;
        isput = false;
    }

    /// <summary>
    /// �L���Ȍ��݈ʒu���ǂ������m�F����B  
    /// �A�C�e�����L���ɔz�u�\�����`�F�b�N����B
    /// </summary>
    /// <returns>�A�C�e�����L���Ȍ��݈ʒu�ɂ���ꍇ��true�A����ȊO��false</returns>
    private bool CheckValidConstructionPosition()
    {
        if (itemToBeConstructed != null)
        {
            return itemToBeConstructed.GetComponent<Constructable>().isValidToBeBuilt;
        }

        return false;
    }
}