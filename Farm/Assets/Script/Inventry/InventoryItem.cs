using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//担当者　越浦晃生

/// <summary>
/// インベントリアイテムの操作を管理するクラス。
/// </summary>
public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    /// <summary>
    /// アイテムが捨てられるかどうかを示すフラグ
    /// </summary>
    public bool isTrashable;

    /// <summary>
    /// アイテム情報を表示するUIオブジェクト
    /// </summary>
    private GameObject itemInfoUI;

    /// <summary>
    /// アイテム名を表示するTextコンポーネント
    /// </summary>
    private Text itemInfoUI_itemName;

    /// <summary>
    /// アイテム説明を表示するTextコンポーネント
    /// </summary>
    private Text itemInfoUI_itemDescription;

    /// <summary>
    /// アイテム機能を表示するTextコンポーネント
    /// </summary>
    private Text itemInfoUI_itemFunctionality;

    /// <summary>
    /// アイテム情報UIが表示されているかどうかの状態
    /// </summary>
    private bool isvisible = false;

    /// <summary>
    /// アイテムの名前、説明、機能
    /// </summary>
    public string thisName, thisDescription, thisFunctionality;

    /// <summary>
    /// 消費待ちのアイテム
    /// </summary>
    private GameObject itemPendingConsumption;

    /// <summary>
    /// アイテムが消費可能かどうか
    /// </summary>
    public bool isConsumable;

    /// <summary>
    /// 体力回復
    /// </summary>
    public float healthEffect;

    /// <summary>
    /// カロリー回復
    /// </summary>
    public float caloriesEffect;

    /// <summary>
    /// マナ回復
    /// </summary>
    public float hydrationEffect;

    /// <summary>
    /// アイテムが装備可能かどうか
    /// </summary>
    public bool isEquippable;

    /// <summary>
    /// 装備待ちのアイテム
    /// </summary>
    private GameObject itemPendingEquipping;

    /// <summary>
    /// クイックスロットに入っているかどうか
    /// </summary>
    public bool isInsideQuiqSlot;

    /// <summary>
    /// アイテムが選択されているかどうか
    /// </summary>
    public bool isSelected;

    /// <summary>
    /// アイテムが使用可能かどうか
    /// </summary>
    public bool isUseable;

    /// <summary>
    /// インベントリ内のアイテムの数量
    /// </summary>
    public int amountInventry = 1;

    /// <summary>
    /// アイテム情報UIの初期化と設定
    /// </summary>
    private void Start()
    {
        itemInfoUI = InventorySystem.Instance.ItemInfoUI;
        itemInfoUI_itemName = itemInfoUI.transform.Find("itemName").GetComponent<Text>();
        itemInfoUI_itemDescription = itemInfoUI.transform.Find("itemDescription").GetComponent<Text>();
        itemInfoUI_itemFunctionality = itemInfoUI.transform.Find("itemFunctionality").GetComponent<Text>();
        itemInfoUI.SetActive(false);
    }

    /// <summary>
    /// アイテム説明画面の表示
    /// マナでの回復
    /// </summary>
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

        if (Input.GetKeyDown(KeyCode.H))
        {
            isvisible = !isvisible;

            // UIの表示切り替え
            float alpha = isvisible ? 1f : 0;
            itemInfoUI.GetComponent<CanvasGroup>().alpha = alpha;
        }

        if (EquipSystem.Instance.currentSelectedObject == gameObject && isvisible)
        {
            // isvisible が true の場合、UI を表示
            itemInfoUI.SetActive(true);
            itemInfoUI_itemName.text = thisName;
            itemInfoUI_itemDescription.text = thisDescription;
            itemInfoUI_itemFunctionality.text = thisFunctionality;
        }

        if (isUseable && EquipSystem.Instance.selectMinion && !ConstructionManager.Instance.inConstructionMode)
        {
            ConstructionManager.Instance.ItemToBeDestroy = EquipSystem.Instance.currentSelectedObject;
            itemInfoUI.SetActive(false);

            if (EquipSystem.Instance.selectedMinion != null)
            {
                EquipSystem.Instance.UseItem(EquipSystem.Instance.selectedMinion);
            }
        }

        if (isConsumable && Input.GetKeyDown(KeyCode.F) && EquipSystem.Instance.selectMana && PlayerState.Instance.currentHealth<200)
        {
            itemPendingConsumption = gameObject;

            InventrySlot parentSlot = GetComponentInParent<InventrySlot>();
            if (parentSlot != null && parentSlot.itemInSlot != null)
            {
                consumingFunction(healthEffect, caloriesEffect, hydrationEffect);
                if (parentSlot.itemInSlot.amountInventry > 1)
                {
                    parentSlot.itemInSlot.amountInventry--;
                    InventorySystem.Instance.ReCalculeList();
                }
                else
                {
                    DestroyImmediate(gameObject);
                    InventorySystem.Instance.ReCalculeList();
                    CraftingSystem.Instance.RefreshNeededItems();
                }
                return;
            }
        }

        //if (isConsumable && itemPendingConsumption == gameObject && Input.GetKeyUp(KeyCode.Q) && EquipSystem.Instance.selectMana)
        //{
        //    InventrySlot parentSlot = GetComponentInParent<InventrySlot>();
        //    if (parentSlot != null && parentSlot.itemInSlot != null)
        //    {
        //        gameObject.name = InventorySystem.Instance.GetItemName(gameObject.name);
        //        if (parentSlot.itemInSlot.thisName == gameObject.name)
        //        {
        //            if (parentSlot.itemInSlot.amountInventry > 1)
        //            {
        //                parentSlot.itemInSlot.amountInventry--;
        //                InventorySystem.Instance.ReCalculeList();
        //            }
        //            else
        //            {
        //                DestroyImmediate(gameObject);
        //                InventorySystem.Instance.ReCalculeList();
        //                CraftingSystem.Instance.RefreshNeededItems();
        //            }
        //            return;
        //        }
        //    }
        //    Debug.LogWarning("アイテムが所属するスロットが見つかりませんでした");
        //}
    }

    /// <summary>
    /// アイテム情報UIがポインタに入った時の処理
    /// </summary>
    /// <param name="eventData">イベントデータ</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        itemInfoUI.SetActive(true);
        itemInfoUI_itemName.text = thisName;
        itemInfoUI_itemDescription.text = thisDescription;
        itemInfoUI_itemFunctionality.text = thisFunctionality;
    }

    /// <summary>
    /// アイテム情報UIがポインタから外れた時の処理
    /// </summary>
    /// <param name="eventData">イベントデータ</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfoUI.SetActive(false);
    }

    /// <summary>
    /// アイテムが右クリックされた時の処理
    /// </summary>
    /// <param name="eventData">イベントデータ</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isEquippable && isInsideQuiqSlot == false && EquipSystem.Instance.CheckIfFull() == false)
            {
                EquipSystem.Instance.AddToQuickSlots(gameObject);
                isInsideQuiqSlot = true;
            }
        }
    }

    /// <summary>
    /// アイテムが右クリックされた時に行われる処理（現在は何もしない）
    /// </summary>
    /// <param name="eventData">イベントデータ</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // 現在、何も処理は行っていない
        }
    }

    /// <summary>
    /// アイテムを消費した際の処理
    /// </summary>
    /// <param name="healthEffect">体力</param>
    /// <param name="caloriesEffect">カロリー</param>
    /// <param name="hydrationEffect">水分</param>
    private void consumingFunction(float healthEffect, float caloriesEffect, float hydrationEffect)
    {
        itemInfoUI.SetActive(false);
        healthEffectCalculation(healthEffect);
        caloriesEffectCalculation(caloriesEffect);
        hydrationEffectCalculation(hydrationEffect);
        InventorySystem.Instance.isHeal = true;
        SoundManager.Instance.PlaySound(SoundManager.Instance.EatSound);
    }

    /// <summary>
    /// 健康回復効果の計算
    /// </summary>
    /// <param name="healthEffect">健康効果</param>
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

    /// <summary>
    /// カロリー回復効果の計算
    /// </summary>
    /// <param name="caloriesEffect">カロリー効果</param>
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

    /// <summary>
    /// 水分回復効果の計算
    /// </summary>
    /// <param name="hydrationEffect">水分効果</param>
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
                PlayerState.Instance.setHydration(hydrationBeforeConsumption + hydrationEffect);
            }
        }
    }
}
