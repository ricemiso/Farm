using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �C���x���g���A�C�e���̑�����Ǘ�����N���X�B
/// </summary>
public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    /// <summary>
    /// �A�C�e�����̂Ă��邩�ǂ����������t���O
    /// </summary>
    public bool isTrashable;

    /// <summary>
    /// �A�C�e������\������UI�I�u�W�F�N�g
    /// </summary>
    private GameObject itemInfoUI;

    /// <summary>
    /// �A�C�e������\������Text�R���|�[�l���g
    /// </summary>
    private Text itemInfoUI_itemName;

    /// <summary>
    /// �A�C�e��������\������Text�R���|�[�l���g
    /// </summary>
    private Text itemInfoUI_itemDescription;

    /// <summary>
    /// �A�C�e���@�\��\������Text�R���|�[�l���g
    /// </summary>
    private Text itemInfoUI_itemFunctionality;

    /// <summary>
    /// �A�C�e�����UI���\������Ă��邩�ǂ����̏��
    /// </summary>
    private bool isvisible = false;

    /// <summary>
    /// �A�C�e���̖��O�A�����A�@�\
    /// </summary>
    public string thisName, thisDescription, thisFunctionality;

    /// <summary>
    /// ����҂��̃A�C�e��
    /// </summary>
    private GameObject itemPendingConsumption;

    /// <summary>
    /// �A�C�e��������\���ǂ���
    /// </summary>
    public bool isConsumable;

    /// <summary>
    /// �̗͉�
    /// </summary>
    public float healthEffect;

    /// <summary>
    /// �J�����[��
    /// </summary>
    public float caloriesEffect;

    /// <summary>
    /// �}�i��
    /// </summary>
    public float hydrationEffect;

    /// <summary>
    /// �A�C�e���������\���ǂ���
    /// </summary>
    public bool isEquippable;

    /// <summary>
    /// �����҂��̃A�C�e��
    /// </summary>
    private GameObject itemPendingEquipping;

    /// <summary>
    /// �N�C�b�N�X���b�g�ɓ����Ă��邩�ǂ���
    /// </summary>
    public bool isInsideQuiqSlot;

    /// <summary>
    /// �A�C�e�����I������Ă��邩�ǂ���
    /// </summary>
    public bool isSelected;

    /// <summary>
    /// �A�C�e�����g�p�\���ǂ���
    /// </summary>
    public bool isUseable;

    /// <summary>
    /// �C���x���g�����̃A�C�e���̐���
    /// </summary>
    public int amountInventry = 1;


    /// <summary>
    /// �A�C�e�����UI�̏������Ɛݒ�
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
    /// �A�C�e��������ʂ̕\��
    /// �}�i�ł̉�
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
            visivbleUI();
        }


        if (isvisible)
        {
            if (EquipSystem.Instance.currentSelectedObject != null &&
                EquipSystem.Instance.currentSelectedObject.TryGetComponent<InventoryItem>(out var inventoryItem) &&
                isvisible)
            {
                // isvisible �� true ���� InventoryItem �����݂���ꍇ�AUI ��\��
                itemInfoUI.SetActive(true);
                itemInfoUI_itemName.text = inventoryItem.thisName;
                SetText(itemInfoUI_itemDescription, inventoryItem.thisDescription);
                SetText(itemInfoUI_itemFunctionality, inventoryItem.thisFunctionality);
            }
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

        if (isConsumable && Input.GetKeyDown(KeyCode.F) && EquipSystem.Instance.currentSelectedObject.name=="Mana" && PlayerState.Instance.currentHealth < 200)
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


    }

    /// <summary>
    /// ���݂̃X���b�g�̃Q�[���I�u�W�F�N�g���擾
    /// </summary>
    /// <param name="slot">�X�g���Ɣԍ�</param>
    /// <returns></returns>
    private GameObject GetSlotItemWithInventoryCheck(int slot)
    {
        if (slot >= 0 && slot < EquipSystem.Instance.quickSlotsList.Count)
        {
            GameObject potentialObject = EquipSystem.Instance.quickSlotsList[slot];

            // InventoryItem �������Ă��邩�`�F�b�N
            if (potentialObject.GetComponent<InventoryItem>() != null)
            {
                return potentialObject; // InventoryItem �������Ă���I�u�W�F�N�g��Ԃ�
            }
            else
            {
                // �q�I�u�W�F�N�g��T��
                Transform childTransform = potentialObject.transform;
                bool found = false;

                while (childTransform.childCount > 0 && !found)
                {
                    childTransform = childTransform.GetChild(0);

                    if (childTransform.GetComponent<InventoryItem>() != null)
                    {
                        return childTransform.gameObject;
                    }
                }

                // InventoryItem ��������Ȃ���� null ��Ԃ�
                return null;
            }
        }

        // �����ȃX���b�g�ԍ��̏ꍇ�� null ��Ԃ�
        return null;
    }


    /// <summary>
    /// �A�C�e��������ʂ�\������֐�
    /// </summary>
    private void visivbleUI()
    {
        GrobalState.Instance.isInfoTask = true;
        isvisible = !isvisible;

        // UI�̕\���؂�ւ�
        float alpha = isvisible ? 1f : 0;
        itemInfoUI.GetComponent<CanvasGroup>().alpha = alpha;
    }

    /// <summary>
    /// �A�C�e�����UI���|�C���^�ɓ��������̏���
    /// </summary>
    /// <param name="eventData">�C�x���g�f�[�^</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        itemInfoUI.SetActive(true);
        itemInfoUI_itemName.text = thisName;
        itemInfoUI_itemDescription.text = thisDescription;
        itemInfoUI_itemFunctionality.text = thisFunctionality;
    }

    /// <summary>
    /// �A�C�e�����UI���|�C���^����O�ꂽ���̏���
    /// </summary>
    /// <param name="eventData">�C�x���g�f�[�^</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfoUI.SetActive(false);
    }

    /// <summary>
    /// �A�C�e�����E�N���b�N���ꂽ���̏���
    /// </summary>
    /// <param name="eventData">�C�x���g�f�[�^</param>
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
    /// �A�C�e�����E�N���b�N���ꂽ���ɍs���鏈���i���݂͉������Ȃ��j
    /// </summary>
    /// <param name="eventData">�C�x���g�f�[�^</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // ���݁A���������͍s���Ă��Ȃ�
        }
    }

    /// <summary>
    /// �A�C�e����������ۂ̏���
    /// </summary>
    /// <param name="healthEffect">�̗�</param>
    /// <param name="caloriesEffect">�J�����[</param>
    /// <param name="hydrationEffect">����</param>
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
    /// ���N�񕜌��ʂ̌v�Z
    /// </summary>
    /// <param name="healthEffect">���N����</param>
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
    /// �J�����[�񕜌��ʂ̌v�Z
    /// </summary>
    /// <param name="caloriesEffect">�J�����[����</param>
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
    /// �����񕜌��ʂ̌v�Z
    /// </summary>
    /// <param name="hydrationEffect">��������</param>
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

    public void SetText(Text self, string text)
    {
        self.text = text.Replace(" ", "\n");
    }
}
