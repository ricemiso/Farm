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

        if (
        (Input.GetAxis("Mouse ScrollWheel") != 0 || // �}�E�X�z�C�[���̓���
        Input.GetKeyDown(KeyCode.Alpha1) ||
        Input.GetKeyDown(KeyCode.Alpha2) ||
        Input.GetKeyDown(KeyCode.Alpha3) ||
        Input.GetKeyDown(KeyCode.Alpha4) ||
        Input.GetKeyDown(KeyCode.Alpha5) ||
        Input.GetKeyDown(KeyCode.Alpha6) ||
        Input.GetKeyDown(KeyCode.Alpha7) ||
        Input.GetKeyDown(KeyCode.Alpha8) ||
        Input.GetKeyDown(KeyCode.Alpha9)))
        {
            ConstructionManager.Instance.ItemToBeDestroy = EquipSystem.Instance.currentSelectedObject;
        }

        //
        if (isUseable && EquipSystem.Instance.selectMinion && !ConstructionManager.Instance.inConstructionMode)
        {
            ConstructionManager.Instance.ItemToBeDestroy = EquipSystem.Instance.currentSelectedObject;
            //gameObject.SetActive(false);
            itemInfoUI.SetActive(false);
            EquipSystem.Instance.UseItem(EquipSystem.Instance.selectedMinion);
        }

        if (isConsumable && Input.GetKeyDown(KeyCode.Q) && EquipSystem.Instance.selectMana)
        {
            itemPendingConsumption = gameObject;
            consumingFunction(healthEffect, caloriesEffect, hydrationEffect);
        }

        if (isConsumable && itemPendingConsumption == gameObject && Input.GetKeyUp(KeyCode.Q) && EquipSystem.Instance.selectMana)
        {
            // ���̃A�C�e�����܂ރX���b�g�����
            InventrySlot parentSlot = GetComponentInParent<InventrySlot>();
            if (parentSlot != null && parentSlot.itemInSlot != null)
            {
                // �X���b�g���̃A�C�e������v���邩�m�F
                gameObject.name = InventorySystem.Instance.GetItemName(gameObject.name);
                if (parentSlot.itemInSlot.thisName == gameObject.name)
                {
                    // �X�^�b�N����1��葽����Ό��炷
                    if (parentSlot.itemInSlot.amountInventry > 1)
                    {
                        parentSlot.itemInSlot.amountInventry--; // �X�^�b�N�������炷
                        InventorySystem.Instance.ReCalculeList(); // UI�⃊�X�g�̍X�V
                    }
                    else
                    {
                        // �X�^�b�N��0�ɂȂ�ꍇ�̓A�C�e�����폜
                        DestroyImmediate(gameObject);
                        InventorySystem.Instance.ReCalculeList();
                        CraftingSystem.Instance.RefreshNeededItems();
                    }
                    return; // ���̎��_�ŏ����I��
                }
            }

            // �X���b�g��������Ȃ��ꍇ��G���[���ɓK�؂ɏ���
            Debug.LogWarning("�A�C�e������������X���b�g��������܂���ł���");
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



            if (isEquippable && isInsideQuiqSlot == false && EquipSystem.Instance.CheckIfFull() == false)
            {
                EquipSystem.Instance.AddToQuickSlots(gameObject);
                isInsideQuiqSlot = true;
            }

        }



    }





    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {

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