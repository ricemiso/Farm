using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    // --- Is this item trashable --- //
    public bool isTrashable;

    // --- Item Info UI --- //
    private GameObject itemInfoUI;

    private Text itemInfoUI_itemName;
    private Text itemInfoUI_itemDescription;
    private Text itemInfoUI_itemFunctionality;

    public string thisName, thisDescription, thisFunctionality;

    // --- Consumption --- //
    private GameObject itemPendingConsumption;
    public bool isConsumable;

    public float healthEffect;
    public float caloriesEffect;
    public float hydrationEffect;


    // --- Equipping --- //
    public bool isEquippable;
    private GameObject itemPendingEquipping;
    public bool isInsideQuiqSlot;


    public bool isSelected;


    public bool isUseable;

    public int amountInventry = 1;


    private void Start()
    {
        itemInfoUI = InventorySystem.Instance.ItemInfoUI;
        itemInfoUI_itemName = itemInfoUI.transform.Find("itemName").GetComponent<Text>();
        itemInfoUI_itemDescription = itemInfoUI.transform.Find("itemDescription").GetComponent<Text>();
        itemInfoUI_itemFunctionality = itemInfoUI.transform.Find("itemFunctionality").GetComponent<Text>();
    }


    void Update()
    {
        if (isSelected)
        {
            gameObject.GetComponent<DragDrop>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<DragDrop>().enabled = true;
        }
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        itemInfoUI.SetActive(true);
        itemInfoUI_itemName.text = thisName;
        itemInfoUI_itemDescription.text = thisDescription;
        itemInfoUI_itemFunctionality.text = thisFunctionality;
    }

   
    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfoUI.SetActive(false);
    }

    
    public void OnPointerDown(PointerEventData eventData)
    {
       
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable)
            {
                
                itemPendingConsumption = gameObject;
                consumingFunction(healthEffect, caloriesEffect, hydrationEffect);
            }


            if (isEquippable && isInsideQuiqSlot == false && EquipSystem.Instance.CheckIfFull() == false)
            {
                EquipSystem.Instance.AddToQuickSlots(gameObject);
                isInsideQuiqSlot = true;
            }


            if (isUseable)
            {
                ConstructionManager.Instance.ItemToBeDestroy = gameObject;

                gameObject.SetActive(false);

                UseItem();
            }


        }

        
        
    }

   
    private void UseItem()
    {

        itemInfoUI.SetActive(false);

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
        SelectionManager.Instance.enabled = true; ;

        if (gameObject != null && !gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }


        //TODO:配置オブジェクトの追加
        switch (gameObject.name)
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


    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable && itemPendingConsumption == gameObject)
            {
               
                bool foundInQuickSlot = false;

                foreach (GameObject quickSlot in EquipSystem.Instance.quickSlotsList)
                {
                    InventrySlot inventrySlot = quickSlot.GetComponent<InventrySlot>();
                    if (inventrySlot != null && inventrySlot.quickSlot)
                    {
                        gameObject.name = InventorySystem.Instance.GetItemName(gameObject.name);
                        if (inventrySlot.itemInSlot != null && inventrySlot.itemInSlot.thisName == gameObject.name)
                        {
                            if (inventrySlot.itemInSlot.amountInventry > 1)
                            {
                                inventrySlot.itemInSlot.amountInventry--; // スタック数を減らす
                                InventorySystem.Instance.ReCalculeList(); // アイテムのUIやリストの更新
                                gameObject.name = InventorySystem.Instance.GetItemName(gameObject.name);
                            }
                            else
                            {
                                gameObject.name = InventorySystem.Instance.GetItemName(gameObject.name);
                                DestroyImmediate(gameObject);
                                InventorySystem.Instance.ReCalculeList();
                                CraftingSystem.Instance.RefreshNeededItems();
                            }

                            InventorySystem.Instance.ReCalculeList(); // UIやリストの更新
                            CraftingSystem.Instance.RefreshNeededItems();

                            foundInQuickSlot = true;
                            break; // クイックスロットでアイテムが見つかったら処理を終了
                        }
                    }
                }


                if (!foundInQuickSlot)
                {
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
                                gameObject.name = InventorySystem.Instance.GetItemName(gameObject.name);
                                if (inventrySlot.itemInSlot.thisName == gameObject.name)
                                {
                                    // スタック数が1より多ければスタック数を減らす
                                    if (inventrySlot.itemInSlot.amountInventry > 1)
                                    {
                                        inventrySlot.itemInSlot.amountInventry--; // スタック数を減らす
                                        InventorySystem.Instance.ReCalculeList(); // アイテムのUIやリストの更新
                                        gameObject.name = InventorySystem.Instance.GetItemName(gameObject.name);
                                    }
                                    else
                                    {
                                        gameObject.name = InventorySystem.Instance.GetItemName(gameObject.name);
                                        DestroyImmediate(gameObject);
                                        InventorySystem.Instance.ReCalculeList();
                                        CraftingSystem.Instance.RefreshNeededItems();

                                    }

                                    break; // 一致するアイテムが見つかったら処理を終了
                                }
                            }
                        }
                    }
                }


              
            }
        }
    }

    private void consumingFunction(float healthEffect, float caloriesEffect, float hydrationEffect)
    {
        itemInfoUI.SetActive(false);

        healthEffectCalculation(healthEffect);

        caloriesEffectCalculation(caloriesEffect);

        hydrationEffectCalculation(hydrationEffect);

        InventorySystem.Instance.isHeal = true;

        SoundManager.Instance.PlaySound(SoundManager.Instance.EatSound);
    }


    private static void healthEffectCalculation(float healthEffect)
    {
       

        float healthBeforeConsumption = PlayerState.Instance.currentHealth;
        float maxHealth = PlayerState.Instance.maxHealth;

        if (healthEffect != 0)
        {
            if ((healthBeforeConsumption + healthEffect) > maxHealth)
            {
                PlayerState.Instance.setHealth(maxHealth);
            }
            else
            {
                PlayerState.Instance.setHealth(healthBeforeConsumption + healthEffect);
            }
        }
    }


    private static void caloriesEffectCalculation(float caloriesEffect)
    {
        

        float caloriesBeforeConsumption = PlayerState.Instance.currentCalories;
        float maxCalories = PlayerState.Instance.maxCalories;

        if (caloriesEffect != 0)
        {
            if ((caloriesBeforeConsumption + caloriesEffect) > maxCalories)
            {
                PlayerState.Instance.setCalories(maxCalories);
            }
            else
            {
                PlayerState.Instance.setCalories(caloriesBeforeConsumption + caloriesEffect);
            }
        }
    }


    private static void hydrationEffectCalculation(float hydrationEffect)
    {
        

        float hydrationBeforeConsumption = PlayerState.Instance.currentHydrationPercent;
        float maxHydration = PlayerState.Instance.maxHydrationPercent;

        if (hydrationEffect != 0)
        {
            if ((hydrationBeforeConsumption + hydrationEffect) > maxHydration)
            {
                PlayerState.Instance.setHydration(maxHydration);
            }
            else
            {
                PlayerState.Instance.setHydration(hydrationBeforeConsumption - hydrationEffect);
            }
        }
    }
}