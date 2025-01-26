using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �I�u�W�F�N�g�̑I�����Ǘ�����N���X�B
/// </summary>
public class SelectionManager : MonoBehaviour
{
    /// <summary>
    /// SelectionManager�̃C���X�^���X�B
    /// </summary>
    public static SelectionManager Instance { get; set; }

    /// <summary>
    /// �^�[�Q�b�g��ɂ��邩�ǂ����������t���O�B
    /// </summary>
    public bool onTarget;

    /// <summary>
    /// �I�����ꂽ�Q�[���I�u�W�F�N�g�B
    /// </summary>
    public GameObject selectgameObject;

    /// <summary>
    /// �C���^���N�V��������UI�B
    /// </summary>
    public GameObject interaction_Info_UI;

    /// <summary>
    /// �C���^���N�V�����e�L�X�g�B
    /// </summary>
    Text interaction_text;

    /// <summary>
    /// ���S�_�̉摜�B
    /// </summary>
    public Image centerDotimage;

    /// <summary>
    /// ��̃A�C�R���B
    /// </summary>
    public Image handIcon;

    /// <summary>
    /// ��̃A�C�R�����\������Ă��邩�ǂ����������t���O�B
    /// </summary>
    public bool HandIsVisible;

    /// <summary>
    /// ���������Ă��邩�ǂ����������t���O�B
    /// </summary>
    public bool Watering;

    /// <summary>
    /// �[�d���Ă��邩�ǂ����������t���O�B
    /// </summary>
    public bool Chargeing;

    /// <summary>
    /// ���x���A�b�v���Ă��邩�ǂ����������t���O�B
    /// </summary>
    public bool leveling;

    /// <summary>
    /// �C���^���N�V�������N�[���_�E�������ǂ����������t���O�B
    /// </summary>
    private bool isInteractionOnCooldown = false;

    /// <summary>
    /// �`���b�v�z���_�[�̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    public GameObject chopHolder;

    /// <summary>
    /// �I�����ꂽ�؂̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    [HideInInspector] public GameObject selectedTree;

    /// <summary>
    /// �I�����ꂽ�N���t�g�̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    [HideInInspector] public GameObject selectedCraft;

    /// <summary>
    /// �I�����ꂽ�΂̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    [HideInInspector] public GameObject selectedStone;

    /// <summary>
    /// �I�����ꂽ�N���X�^���̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    [HideInInspector] public GameObject selectedCrystal;

    /// <summary>
    /// �I�����ꂽ�~�j�N���X�^���̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    [HideInInspector] public GameObject selectedMiniCrystal;

    /// <summary>
    /// �I�����ꂽ�����̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    [HideInInspector] public GameObject selectedAnimal;

    /// <summary>
    /// �I�����ꂽ���[�{�b�N�X�̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    [HideInInspector] public GameObject selectedStorageBox;

    /// <summary>
    /// �I�����ꂽ�y��̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    [HideInInspector] public GameObject selectedSoil;

    /// <summary>
    /// �����ɕK�v�ȃ}�i�̍���
    /// </summary>
    private int unincreesemana = 0;

    /// <summary>
    /// �`���b�v�e�L�X�g�B
    /// </summary>
    private Text chopText;

    /// <summary>
    /// �_���[�W�̒x�������邩�ǂ����������t���O�B
    /// </summary>
    private bool isdamageDelay = false;

    /// <summary>
    /// �����ݒ���s���܂��B
    /// </summary>
    private void Start()
    {
        Watering = false;
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<Text>();
        chopText = chopHolder.GetComponentInChildren<Text>();
    }

    /// <summary>
    /// �V���O���g���p�^�[����K�p���A�C���X�^���X�����������܂��B
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
    /// �v���C���[���s�����ׂĂ̏����������ōs���Ă��܂�
    /// ���́A�̌@�A�U���A�}�i�`���[�W�A�E���A�_�ƁA����
    /// </summary>
    void Update()
    {
        if (Camera.main == null) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            //TODO : �j��͂����ɒǉ����Ă���
            ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();
            Choppablecraft choppableCraft = selectionTransform.GetComponent<Choppablecraft>();
            ChoppableStone choppableStone = selectionTransform.GetComponent<ChoppableStone>();
            CrystalGrowth crystal = selectionTransform.GetComponent<CrystalGrowth>();
            MiniCrystal minicrystal = selectionTransform.GetComponent<MiniCrystal>();

            //TODO:�؂�|������
            if (choppableTree && choppableTree.playerRange)
            {
                choppableTree.canBeChopped = true;
                selectedTree = choppableTree.gameObject;
                chopText.text = "��";
                chopHolder.gameObject.SetActive(true);

            }
            else if (choppableCraft && choppableCraft.playerRange)
            {
                choppableCraft.canBeChopped = true;
                selectedCraft = choppableCraft.gameObject;
                chopText.text = choppableCraft.CraftItemName();
                chopHolder.gameObject.SetActive(true);

            }
            else if (choppableStone && choppableStone.playerRange)
            {
                choppableStone.canBeChopped = true;
                selectedStone = choppableStone.gameObject;
                chopText.text = "��";
                chopHolder.gameObject.SetActive(true);

            }
            else if (crystal && crystal.playerRange)
            {
                crystal.canBeWatch = true;
                selectedCrystal = crystal.gameObject;
                chopText.text = "�}�i�N���X�^��";
                chopHolder.gameObject.SetActive(true);

                if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.IsPlayerHooldingMana() && !Chargeing)
                {

                    Chargeing = true;

                    int stackCount = EquipSystem.Instance.GetEquippedItemStackCountBySlot(EquipSystem.Instance.selectedNumber);
                    crystal.GetEnergy(stackCount);
                    StartCoroutine(DelayWatering());
                }
            }
            else if (minicrystal && minicrystal.playerRange)
            {
                minicrystal.canBeWatchs = true;
                selectedCrystal = minicrystal.gameObject;
                chopText.text = "�~�j�N���X�^��";
                chopHolder.gameObject.SetActive(true);
            }
            else
            {
                if (selectedTree != null)
                {
                    selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
                    selectedTree = null;
                    chopHolder.gameObject.SetActive(false);
                }
                else if (selectedCraft != null)
                {
                    selectedCraft.gameObject.GetComponent<Choppablecraft>().canBeChopped = false;
                    selectedCraft = null;
                    chopHolder.gameObject.SetActive(false);
                }
                else if (selectedStone != null)
                {
                    selectedStone.gameObject.GetComponent<ChoppableStone>().canBeChopped = false;
                    selectedStone = null;
                    chopHolder.gameObject.SetActive(false);
                }
                else if (selectedCrystal != null)
                {
                    //selectedCrystal.gameObject.GetComponent<CrystalGrowth>().canBeCharge = false;
                    selectedCrystal = null;
                    chopHolder.gameObject.SetActive(false);
                }
                else if (selectedMiniCrystal != null)
                {
                    //selectedMiniCrystal.gameObject.GetComponent<MiniCrystal>().canBeCharge = false;
                    selectedMiniCrystal = null;
                    chopHolder.gameObject.SetActive(false);
                }
            }


            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();
            //TODO:�E������
            if (interactable && interactable.playerRange && !isInteractionOnCooldown)
            {
                StartCoroutine(HandleInteraction());
                onTarget = true;
                selectgameObject = interactable.gameObject;

                interaction_text.text = interactable.GetItemName(selectgameObject);
                Debug.Log(selectgameObject.name);
                interaction_Info_UI.SetActive(true);

                centerDotimage.gameObject.SetActive(false);
                handIcon.gameObject.SetActive(true);


                HandIsVisible = true;

            }


            Soil soil = selectionTransform.GetComponent<Soil>();
            if (soil && soil.playerInRange)
            {
                if (soil.isEmpty && EquipSystem.Instance.IsPlayerHooldingSeed())
                {
                    string seedName = EquipSystem.Instance.selectedItem.GetComponent<InventoryItem>().thisName;
                    string onlyPlantName = seedName.Split(new string[] { "�̎�" }, StringSplitOptions.None)[0];

                    //TODO:���{��ɏC������Switch��������

                    interaction_text.text = onlyPlantName + "��A����";
                    interaction_Info_UI.SetActive(true);

                    if (Input.GetMouseButtonDown(0))
                    {
                        soil.PlantSeed();
                        GrobalState.Instance.isFarm1 = true;
                        ConstructionManager.Instance.HandleItemStack(EquipSystem.Instance.selectedItem);
                        //Destroy(EquipSystem.Instance.selecteditemModel);
                    }
                }
                else if (soil.isEmpty)
                {
                    interaction_text.text = "�y��";
                    interaction_Info_UI.SetActive(true);
                }
                else
                {
                    if (EquipSystem.Instance.IsPlayerHooldingMana())
                    {
                        if (soil.currentplant.CheckGrows())
                        {
                            interaction_text.text = soil.plantName;
                            interaction_Info_UI.SetActive(true);
                        }
                        else
                        {
                            interaction_text.text = "�}�i�������Ă�������";
                            interaction_Info_UI.SetActive(true);

                            if (Input.GetMouseButtonDown(0) && !Watering)
                            {

                                Watering = true;
                                GrobalState.Instance.isWater = true;

                                //TODO:�ύX����@(�I�[�f�B�I�N���b�v���g�p����ꍇ�͏�)
                                //SoundManager.Instance.wateringCannel.PlayoneShot(SoundManager.Instance.wateringChannel);
                                SoundManager.Instance.PlaySound(SoundManager.Instance.PutSeSound);

                                //soil.currentplant.isWatered = true;
                                soil.currentplant.Grow();

                                soil.MakeSoilWatered();
                                StartCoroutine(DelayWatering());



                            }
                        }
                    }
                    else
                    {
                        interaction_text.text = soil.plantName;
                        interaction_Info_UI.SetActive(true);
                    }

                }

                selectedSoil = soil.gameObject;

            }
            else
            {
                if (selectedSoil != null)
                {
                    selectedSoil = null;
                }
            }


            StrageBox strageBox = selectionTransform.GetComponent<StrageBox>();

            if (strageBox && strageBox.playerInRange && ConstructionManager.Instance.inConstructionMode == false)
            {
                interaction_text.text = "�J����";
                interaction_Info_UI.SetActive(true);

                selectedStorageBox = strageBox.gameObject;

                if (Input.GetMouseButtonDown(0))
                {
                    StorageManager.Instance.OpenBox(strageBox);
                }
            }
            else
            {
                if (selectedStorageBox != null)
                {
                    selectedStorageBox = null;
                }
            }




            Animal animal = selectionTransform.GetComponent<Animal>();

            if (animal && animal.playerISRange)
            {
                animal.canBeChopped = true;
                animal.canBeChopped = true;

                selectedAnimal = animal.gameObject;
                chopText.text = animal.GetAnimalName();
                chopHolder.gameObject.SetActive(true);

                if (animal.CompareTag("Enemy"))
                {
                    if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.IsHoldingWeapon())
                    {
                        if (isdamageDelay)
                        {

                            return;
                        }
                        else
                        {
                            StartCoroutine(DealDamageTo(animal, 1.2f, EquipSystem.Instance.GetWeaPonDamage()));
                            StartCoroutine(DelayedAttribute());
                        }

                    }

                }
                else if (EquipSystem.Instance.IsPlayerHooldingMana())
                {
                    if (Input.GetMouseButtonDown(0) && !leveling && animal.level <= 3)
                    {
                        leveling = true;
                        int stackCount = EquipSystem.Instance.GetEquippedItemStackCountBySlot(EquipSystem.Instance.selectedNumber);
                        unincreesemana = 3 -animal.level;
                        animal.LevelUp(stackCount);
                        StartCoroutine(DelayWatering());
                    }
                }
                else
                {
                    //interaction_Info_UI.SetActive(true);
                    centerDotimage.gameObject.SetActive(true);
                    handIcon.gameObject.SetActive(false);

                    HandIsVisible = false;
                }

            }
            else
            {
                if (selectedAnimal != null)
                {
                    selectedAnimal.gameObject.GetComponent<Animal>().canBeChopped = false;
                    selectedAnimal = null;
                    chopHolder.gameObject.SetActive(false);
                }
            }


            if (!interactable && !animal)
            {
                onTarget = false;
                HandIsVisible = false;

                centerDotimage.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);
            }


            if (!interactable && !animal && !choppableTree && !choppableCraft && !choppableStone && !soil && !strageBox && !crystal && !minicrystal)
            {
                interaction_text.text = "";
                handIcon.gameObject.SetActive(false);
                chopHolder.gameObject.SetActive(false);
            }

        }

    }


    /// <summary>
    /// �t���O�Ǘ����s���R���[�`���B
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator HandleInteraction()
    {
        isInteractionOnCooldown = true; // �N�[���_�E���J�n

        yield return new WaitForSeconds(0.5f); // 0.5�b�̒x��

        isInteractionOnCooldown = false; // �N�[���_�E���I��
    }


    /// <summary>
    /// �����̒x���������s���R���[�`���B
    /// </summary>
    IEnumerator DelayWatering()
    {
        if (Chargeing)
        {
            Destroy(EquipSystem.Instance.selectedItem);
            Destroy(EquipSystem.Instance.selecteditemModel);
        }
        else if (leveling)
        {
            if (unincreesemana >= 0)
            {
                ConstructionManager.Instance.HandleItemStack(EquipSystem.Instance.selectedItem, unincreesemana);
            }
           
        }
        else
        {
            ConstructionManager.Instance.HandleItemStack(EquipSystem.Instance.selectedItem);
        }

        yield return new WaitForSeconds(2.0f);

        Watering = false;
        Chargeing = false;
        leveling = false;
    }

    /// <summary>
    /// �����̒x���������s���R���[�`���B
    /// </summary>
    IEnumerator DelayedAttribute()
    {
        isdamageDelay = true;
        yield return new WaitForSeconds(1f);
        isdamageDelay = false;
    }

    /// <summary>
    /// �����Ƀ_���[�W��^����R���[�`���B
    /// </summary>
    /// <param name="animal">����</param>
    /// <param name="delay">�x������</param>
    /// <param name="damage">�_���[�W��</param>
    /// <returns>IEnumerator</returns>
    IEnumerator DealDamageTo(Animal animal, float delay, int damage)
    {
        yield return new WaitForSeconds(delay);

        animal.TakeDamage(damage);
        yield return new WaitForSeconds(0.5f);
        animal.canBeChopped = false;
    }

    /// <summary>
    /// �I���𖳌��ɂ��܂��B
    /// </summary>
    public void DisableSelection()
    {
        handIcon.enabled = false;
        centerDotimage.enabled = false;
        interaction_Info_UI.SetActive(false);
        selectgameObject = null;
    }

    /// <summary>
    /// �I����L���ɂ��܂��B
    /// </summary>
    public void EnableSelection()
    {
        handIcon.enabled = true;
        centerDotimage.enabled = true;
        interaction_Info_UI.SetActive(true);
    }

}