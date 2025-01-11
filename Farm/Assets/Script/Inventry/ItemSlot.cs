using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �A�C�e���X���b�g���Ǘ�����N���X�B
/// </summary>
public class ItemSlot : MonoBehaviour, IDropHandler
{
    /// <summary>
    /// �h���b�v���ꂽ�A�C�e������������֐��B
    /// </summary>
    /// <param name="eventData">�C�x���g�f�[�^</param>
    public void OnDrop(PointerEventData eventData)
    {
        InventorySystem.Instance.inventoryUpdated = true;

        if (transform.childCount <= 1)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.dropItemSound);

            DragDrop.itemBeingDragged.transform.SetParent(transform);
            DragDrop.itemBeingDragged.transform.localPosition = new Vector2(0, 0);

            if (transform.CompareTag("QuickSlot") == false)
            {
                DragDrop.itemBeingDragged.GetComponent<InventoryItem>().isInsideQuiqSlot = false;
                InventorySystem.Instance.ReCalculeList();
                CraftingSystem.Instance.RefreshNeededItems();
            }

            if (transform.CompareTag("QuickSlot"))
            {
                DragDrop.itemBeingDragged.GetComponent<InventoryItem>().isInsideQuiqSlot = true;
                InventorySystem.Instance.ReCalculeList();
                CraftingSystem.Instance.RefreshNeededItems();
            }
        }
        else
        {
            InventoryItem dragedItem = DragDrop.itemBeingDragged.GetComponent<InventoryItem>();

            var itemName = InventorySystem.Instance.GetItemName(dragedItem.thisName);
            if (dragedItem.thisName == GetStoredItem().thisName && IsLimitExceded(dragedItem) == false)
            {
                GetStoredItem().amountInventry += dragedItem.amountInventry;
                DestroyImmediate(DragDrop.itemBeingDragged);
            }
            else
            {
                DragDrop.itemBeingDragged.transform.SetParent(transform);
            }
        }
    }

    /// <summary>
    /// �X���b�g���ɕۑ�����Ă���A�C�e�����擾����֐��B
    /// </summary>
    /// <returns>�C���x���g���A�C�e��</returns>
    InventoryItem GetStoredItem()
    {
        return transform.GetChild(0).GetComponent<InventoryItem>();
    }

    /// <summary>
    /// �A�C�e���̃X�^�b�N�����𒴂��Ă��邩�ǂ������m�F����֐��B
    /// </summary>
    /// <param name="draggItem">�h���b�O���ꂽ�A�C�e��</param>
    /// <returns>�����𒴂��Ă���ꍇ��true�A����ȊO��false</returns>
    bool IsLimitExceded(InventoryItem draggItem)
    {
        if ((draggItem.amountInventry + GetStoredItem().amountInventry) > InventorySystem.Instance.stackLimit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
