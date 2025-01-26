using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//担当者　越浦晃生

/// <summary>
/// オブジェクトの選択を管理するクラス。
/// </summary>
public class SelectionManager : MonoBehaviour
{
    /// <summary>
    /// SelectionManagerのインスタンス。
    /// </summary>
    public static SelectionManager Instance { get; set; }

    /// <summary>
    /// ターゲット上にいるかどうかを示すフラグ。
    /// </summary>
    public bool onTarget;

    /// <summary>
    /// 選択されたゲームオブジェクト。
    /// </summary>
    public GameObject selectgameObject;

    /// <summary>
    /// インタラクション情報のUI。
    /// </summary>
    public GameObject interaction_Info_UI;

    /// <summary>
    /// インタラクションテキスト。
    /// </summary>
    Text interaction_text;

    /// <summary>
    /// 中心点の画像。
    /// </summary>
    public Image centerDotimage;

    /// <summary>
    /// 手のアイコン。
    /// </summary>
    public Image handIcon;

    /// <summary>
    /// 手のアイコンが表示されているかどうかを示すフラグ。
    /// </summary>
    public bool HandIsVisible;

    /// <summary>
    /// 水やりをしているかどうかを示すフラグ。
    /// </summary>
    public bool Watering;

    /// <summary>
    /// 充電しているかどうかを示すフラグ。
    /// </summary>
    public bool Chargeing;

    /// <summary>
    /// レベルアップしているかどうかを示すフラグ。
    /// </summary>
    public bool leveling;

    /// <summary>
    /// インタラクションがクールダウン中かどうかを示すフラグ。
    /// </summary>
    private bool isInteractionOnCooldown = false;

    /// <summary>
    /// チョップホルダーのゲームオブジェクト。
    /// </summary>
    public GameObject chopHolder;

    /// <summary>
    /// 選択された木のゲームオブジェクト。
    /// </summary>
    [HideInInspector] public GameObject selectedTree;

    /// <summary>
    /// 選択されたクラフトのゲームオブジェクト。
    /// </summary>
    [HideInInspector] public GameObject selectedCraft;

    /// <summary>
    /// 選択された石のゲームオブジェクト。
    /// </summary>
    [HideInInspector] public GameObject selectedStone;

    /// <summary>
    /// 選択されたクリスタルのゲームオブジェクト。
    /// </summary>
    [HideInInspector] public GameObject selectedCrystal;

    /// <summary>
    /// 選択されたミニクリスタルのゲームオブジェクト。
    /// </summary>
    [HideInInspector] public GameObject selectedMiniCrystal;

    /// <summary>
    /// 選択された動物のゲームオブジェクト。
    /// </summary>
    [HideInInspector] public GameObject selectedAnimal;

    /// <summary>
    /// 選択された収納ボックスのゲームオブジェクト。
    /// </summary>
    [HideInInspector] public GameObject selectedStorageBox;

    /// <summary>
    /// 選択された土壌のゲームオブジェクト。
    /// </summary>
    [HideInInspector] public GameObject selectedSoil;

    /// <summary>
    /// 強化に必要なマナの差分
    /// </summary>
    private int unincreesemana = 0;

    /// <summary>
    /// チョップテキスト。
    /// </summary>
    private Text chopText;

    /// <summary>
    /// ダメージの遅延があるかどうかを示すフラグ。
    /// </summary>
    private bool isdamageDelay = false;

    /// <summary>
    /// 初期設定を行います。
    /// </summary>
    private void Start()
    {
        Watering = false;
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<Text>();
        chopText = chopHolder.GetComponentInChildren<Text>();
    }

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
    /// プレイヤーが行うすべての処理をここで行っています
    /// 伐採、採掘、攻撃、マナチャージ、拾う、農業、強化
    /// </summary>
    void Update()
    {
        if (Camera.main == null) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            //TODO : 破壊はここに追加していく
            ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();
            Choppablecraft choppableCraft = selectionTransform.GetComponent<Choppablecraft>();
            ChoppableStone choppableStone = selectionTransform.GetComponent<ChoppableStone>();
            CrystalGrowth crystal = selectionTransform.GetComponent<CrystalGrowth>();
            MiniCrystal minicrystal = selectionTransform.GetComponent<MiniCrystal>();

            //TODO:切り倒す処理
            if (choppableTree && choppableTree.playerRange)
            {
                choppableTree.canBeChopped = true;
                selectedTree = choppableTree.gameObject;
                chopText.text = "木";
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
                chopText.text = "石";
                chopHolder.gameObject.SetActive(true);

            }
            else if (crystal && crystal.playerRange)
            {
                crystal.canBeWatch = true;
                selectedCrystal = crystal.gameObject;
                chopText.text = "マナクリスタル";
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
                chopText.text = "ミニクリスタル";
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
            //TODO:拾う処理
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
                    string onlyPlantName = seedName.Split(new string[] { "の種" }, StringSplitOptions.None)[0];

                    //TODO:日本語に修正するSwitch文を書く

                    interaction_text.text = onlyPlantName + "を植える";
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
                    interaction_text.text = "土壌";
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
                            interaction_text.text = "マナをあげてください";
                            interaction_Info_UI.SetActive(true);

                            if (Input.GetMouseButtonDown(0) && !Watering)
                            {

                                Watering = true;
                                GrobalState.Instance.isWater = true;

                                //TODO:変更する　(オーディオクリップを使用する場合は上)
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
                interaction_text.text = "開ける";
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
    /// フラグ管理を行うコルーチン。
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator HandleInteraction()
    {
        isInteractionOnCooldown = true; // クールダウン開始

        yield return new WaitForSeconds(0.5f); // 0.5秒の遅延

        isInteractionOnCooldown = false; // クールダウン終了
    }


    /// <summary>
    /// 水やりの遅延処理を行うコルーチン。
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
    /// 属性の遅延処理を行うコルーチン。
    /// </summary>
    IEnumerator DelayedAttribute()
    {
        isdamageDelay = true;
        yield return new WaitForSeconds(1f);
        isdamageDelay = false;
    }

    /// <summary>
    /// 動物にダメージを与えるコルーチン。
    /// </summary>
    /// <param name="animal">動物</param>
    /// <param name="delay">遅延時間</param>
    /// <param name="damage">ダメージ量</param>
    /// <returns>IEnumerator</returns>
    IEnumerator DealDamageTo(Animal animal, float delay, int damage)
    {
        yield return new WaitForSeconds(delay);

        animal.TakeDamage(damage);
        yield return new WaitForSeconds(0.5f);
        animal.canBeChopped = false;
    }

    /// <summary>
    /// 選択を無効にします。
    /// </summary>
    public void DisableSelection()
    {
        handIcon.enabled = false;
        centerDotimage.enabled = false;
        interaction_Info_UI.SetActive(false);
        selectgameObject = null;
    }

    /// <summary>
    /// 選択を有効にします。
    /// </summary>
    public void EnableSelection()
    {
        handIcon.enabled = true;
        centerDotimage.enabled = true;
        interaction_Info_UI.SetActive(true);
    }

}