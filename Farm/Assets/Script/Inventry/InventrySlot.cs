using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �C���x���g���X���b�g���Ǘ�����N���X�B
/// </summary>
public class InventrySlot : MonoBehaviour
{
    /// <summary>
    /// �A�C�e���̐��ʂ�\������e�L�X�g�R���|�[�l���g�B
    /// </summary>
    public Text amountTXT;

    /// <summary>
    /// �X���b�g���̃C���x���g���A�C�e���B
    /// </summary>
    public InventoryItem itemInSlot;

    /// <summary>
    /// �N�C�b�N�X���b�g���ǂ����������t���O�B
    /// </summary>
    public bool quickSlot;

    /// <summary>
    /// ��\���I�u�W�F�N�g��ݒ肷�邽�߂̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    [SerializeField] private GameObject alphaobject;

    /// <summary>
    /// �X�^�b�N�̌���0�Ȃ炷�������I�u�W�F�N�g���o��������
    /// �X�^�b�N��������ΐ������o��
    /// </summary>
    private void Update()
    {
        InventoryItem item = CheckInventryItem();

        if (item != null)
        {
            itemInSlot = item;
            if (alphaobject != null)
            {
                alphaobject.SetActive(false);
            }
        }
        else
        {
            itemInSlot = null;
            if (alphaobject != null)
            {
                alphaobject.SetActive(true);
            }
        }

        if (itemInSlot != null && itemInSlot.amountInventry >= 2 && itemInSlot.amountInventry<= InventorySystem.Instance.stackLimit)
        {
            amountTXT.gameObject.SetActive(true);
            amountTXT.text = $"{itemInSlot.amountInventry}";
            amountTXT.transform.SetAsLastSibling();
        }
        else
        {
            amountTXT.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// �X���b�g���̃C���x���g���A�C�e�����`�F�b�N���܂��B
    /// </summary>
    /// <returns>�X���b�g���̃C���x���g���A�C�e��</returns>
    public InventoryItem CheckInventryItem()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<InventoryItem>())
            {
                return child.GetComponent<InventoryItem>();
            }
        }
        return null;
    }

    /// <summary>
    /// �X���b�g���̃C���x���g���A�C�e����ݒ肷��B
    /// </summary>
    public void SetItemInSlot()
    {
        itemInSlot = CheckInventryItem();
    }
}
