using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �h���b�O���h���b�v�@�\����������N���X�B
/// </summary>
public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    /// <summary>
    /// �A�C�e�����h���b�O�����ǂ����������Q�[���I�u�W�F�N�g�B
    /// </summary>
    public static GameObject itemBeingDragged;

    /// <summary>
    /// �A�C�e���̏����ʒu�B
    /// </summary>
    Vector3 startPosition;

    /// <summary>
    /// �A�C�e���̏����̐e�I�u�W�F�N�g�B
    /// </summary>
    Transform startParent;

    /// <summary>
    /// �A�C�e�����X�^�b�N���Ă��邩�ǂ����B
    /// </summary>
    public bool isStack = false;

    /// <summary>
    /// �A�C�e����RectTransform�R���|�[�l���g�B
    /// </summary>
    private RectTransform rectTransform;

    /// <summary>
    /// �A�C�e����CanvasGroup�R���|�[�l���g�B
    /// </summary>
    private CanvasGroup canvasGroup;

    /// <summary>
    /// �R���|�[�l���g�����������܂��B
    /// </summary>
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// �h���b�O�J�n���̏����B
    /// </summary>
    /// <param name="eventData">�C�x���g�f�[�^�B</param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
        startPosition = transform.position;
        startParent = transform.parent;
        transform.SetParent(transform.root);
        itemBeingDragged = gameObject;
    }

    /// <summary>
    /// �h���b�O���̏����B
    /// </summary>
    /// <param name="eventData">�C�x���g�f�[�^�B</param>
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta /*/canvas.scaaleFactor*/;
    }

    /// <summary>
    /// �h���b�O�I�����̏����B
    /// </summary>
    /// <param name="eventData">�C�x���g�f�[�^�B</param>
    public void OnEndDrag(PointerEventData eventData)
    {
        var termpItemReference = itemBeingDragged;
        itemBeingDragged = null;

        if (termpItemReference.transform.parent == termpItemReference.transform.root)
        {
            // �A�C�e���̃h���b�v����
            if (termpItemReference.name == "Mana(Clone)" || termpItemReference.name == "Stone(Clone)" || termpItemReference.name == "Stick(Clone)" || termpItemReference.name == "Log(Clone)")
            {
                termpItemReference.SetActive(false);
                AlertDialogManager dialogManager = FindObjectOfType<AlertDialogManager>();
                dialogManager.ShowDialog("�h���b�v���܂����H", (responce) =>
                {
                    if (responce)
                    {
                        DropItemIntoTheWorld(termpItemReference);
                    }
                    else
                    {
                        CancelDragging(termpItemReference);
                    }
                });
            }
            else
            {
                CancelDragging(termpItemReference);
            }
        }

        if (termpItemReference.transform.parent == startParent)
        {
            CancelDragging(termpItemReference);
        }

        if (termpItemReference.transform.parent != termpItemReference.transform.root
            && termpItemReference.transform.parent != startParent)
        {
            if (termpItemReference.transform.parent.childCount > 2)
            {
                CancelDragging(termpItemReference);
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    isStack = true;
                    DivdeStack(termpItemReference);
                }
                else
                {
                    CraftingSystem.Instance.RefreshNeededItems();
                }
                isStack = false;
            }
        }

        termpItemReference.transform.SetAsFirstSibling();

        // ���X�g�̍Čv�Z�ƃA�C�e���̍X�V
        InventorySystem.Instance.ReCalculeList();
        CraftingSystem.Instance.RefreshNeededItems();

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// �A�C�e���̃X�^�b�N���������B
    /// </summary>
    /// <param name="termpItemReference">�h���b�O���̃A�C�e���B</param>
    private void DivdeStack(GameObject termpItemReference)
    {
        InventoryItem item = termpItemReference.GetComponent<InventoryItem>();

        if (item.amountInventry > 1)
        {
            //TODO:�X�^�b�N��������������Ă��邪�ύX����H
            item.amountInventry--;  // ���̃X�^�b�N�̐������炷
            InventorySystem.Instance.AddToinventry(item.thisName, false);  // �V�����X�^�b�N��ǉ�
            InventorySystem.Instance.ReCalculeList();  // �X�^�b�N�ύX��Ƀ��X�g���Čv�Z
            CraftingSystem.Instance.RefreshNeededItems();  // �K�v�ȃA�C�e�����X�V

            InventorySystem.Instance.SetStackState(true);
        }
    }

    /// <summary>
    /// �h���b�O�L�����Z�������B
    /// </summary>
    /// <param name="termpItemReference">�h���b�O���̃A�C�e���B</param>
    void CancelDragging(GameObject termpItemReference)
    {
        transform.position = startPosition;
        transform.SetParent(startParent);
        termpItemReference.SetActive(true);
    }

    /// <summary>
    /// �A�C�e�������[���h�Ƀh���b�v���鏈���B
    /// </summary>
    /// <param name="termpItemReference">�h���b�O���̃A�C�e���B</param>
    private void DropItemIntoTheWorld(GameObject termpItemReference)
    {
        string cleanName = termpItemReference.name.Split(new string[] { "(Clone)" }, StringSplitOptions.None)[0];
        Debug.Log(cleanName);

        int stackCount = termpItemReference.GetComponent<InventoryItem>().amountInventry;

        for (int i = 0; i < stackCount; i++)
        {
            GameObject item = Instantiate(Resources.Load<GameObject>(cleanName + "_model"));
            item.transform.position = Vector3.zero;

            var dropSpawnPosition = PlayerState.Instance.playerBody.transform.Find("DropSpawn").transform.position;
            item.transform.localPosition = new Vector3(dropSpawnPosition.x, dropSpawnPosition.y, dropSpawnPosition.z);

            var itemsObject = FindObjectOfType<EnviromentManager>().gameObject.transform.Find("[ITEMS]");
            item.transform.SetParent(itemsObject.transform);
        }

        DestroyImmediate(termpItemReference.gameObject);
        InventorySystem.Instance.ReCalculeList();
        CraftingSystem.Instance.RefreshNeededItems();
    }
}
