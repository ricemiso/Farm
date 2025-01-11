using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//�S���ҁ@�z�Y�W��
//�C���x���g���̔p�~�ɔ������ݎg�p���Ă��Ȃ�

/// <summary>
/// �S�~���X���b�g���Ǘ�����N���X�B
/// </summary>
public class TrashSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// �S�~���A���[�gUI
    /// </summary>
    public GameObject trashAlertUI;

    /// <summary>
    /// �e�L�X�g��ύX���邽�߂�Text�R���|�[�l���g
    /// </summary>
    private Text textToModify;

    /// <summary>
    /// �S�~�������Ă���Ƃ��̃X�v���C�g
    /// </summary>
    public Sprite trash_closed;

    /// <summary>
    /// �S�~�����J���Ă���Ƃ��̃X�v���C�g
    /// </summary>
    public Sprite trash_opened;

    /// <summary>
    /// �C���[�W�R���|�[�l���g
    /// </summary>
    private Image imageComponent;

    /// <summary>
    /// "Yes"�{�^��
    /// </summary>
    private Button YesBTN;

    /// <summary>
    /// "No"�{�^��
    /// </summary>
    private Button NoBTN;

    /// <summary>
    /// �h���b�O���ꂽ�A�C�e�����擾����v���p�e�B
    /// </summary>
    private GameObject draggedItem
    {
        get
        {
            return DragDrop.itemBeingDragged;
        }
    }

    /// <summary>
    /// �폜�����A�C�e��
    /// </summary>
    private GameObject itemToBeDeleted;

    /// <summary>
    /// �A�C�e�������擾����v���p�e�B
    /// </summary>
    private string itemName
    {
        get
        {
            string name = itemToBeDeleted.name;
            string toRemove = "(Clone)";
            string result = name.Replace(toRemove, "");
            return result;
        }
    }

    /// <summary>
    /// �����ݒ�B
    /// </summary>
    private void Start()
    {
        imageComponent = transform.Find("background").GetComponent<Image>();
        textToModify = trashAlertUI.transform.Find("Text").GetComponent<Text>();
        YesBTN = trashAlertUI.transform.Find("yes").GetComponent<Button>();
        YesBTN.onClick.AddListener(delegate { DeleteItem(); });

        NoBTN = trashAlertUI.transform.Find("no").GetComponent<Button>();
        NoBTN.onClick.AddListener(delegate { CancelDeletion(); });
    }

    /// <summary>
    /// �A�C�e�����h���b�v���ꂽ���̏������s���܂��B
    /// </summary>
    /// <param name="eventData">�C�x���g�f�[�^</param>
    public void OnDrop(PointerEventData eventData)
    {
        if (draggedItem.GetComponent<InventoryItem>().isTrashable == true)
        {
            itemToBeDeleted = draggedItem.gameObject;
            StartCoroutine(notifyBeforeDeletion());
        }
    }

    /// <summary>
    /// �폜�O�ɒʒm���s���R���[�`���B
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator notifyBeforeDeletion()
    {
        trashAlertUI.SetActive(true);
        textToModify.text = itemName + "���̂ĂĂ���낵���ł���?";
        yield return new WaitForSeconds(1f);
    }

    /// <summary>
    /// �폜���L�����Z�����܂��B
    /// </summary>
    private void CancelDeletion()
    {
        imageComponent.sprite = trash_closed;
        trashAlertUI.SetActive(false);
    }

    /// <summary>
    /// �A�C�e�����폜���܂��B
    /// </summary>
    private void DeleteItem()
    {
        imageComponent.sprite = trash_closed;
        DestroyImmediate(itemToBeDeleted.gameObject);
        InventorySystem.Instance.ReCalculeList();
        CraftingSystem.Instance.RefreshNeededItems();
        trashAlertUI.SetActive(false);
    }

    /// <summary>
    /// �|�C���^���S�~���X���b�g�ɓ��������̏������s���܂��B
    /// </summary>
    /// <param name="eventData">�C�x���g�f�[�^</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (draggedItem != null && draggedItem.GetComponent<InventoryItem>().isTrashable == true)
        {
            imageComponent.sprite = trash_opened;
        }
    }

    /// <summary>
    /// �|�C���^���S�~���X���b�g����o�����̏������s���܂��B
    /// </summary>
    /// <param name="eventData">�C�x���g�f�[�^</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (draggedItem != null && draggedItem.GetComponent<InventoryItem>().isTrashable == true)
        {
            imageComponent.sprite = trash_closed;
        }
    }
}
