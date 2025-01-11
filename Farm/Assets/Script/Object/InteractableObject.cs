using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �E����I�u�W�F�N�g���Ǘ�����N���X�B
/// </summary>
public class InteractableObject : MonoBehaviour
{
    /// <summary>
    /// �A�C�e���̖��O�B
    /// </summary>
    public string ItemName;

    /// <summary>
    /// �C���x���g���ɒǉ������A�C�e���̖��O�B
    /// </summary>
    [SerializeField] string InventryName;

    /// <summary>
    /// �v���C���[���͈͓��ɂ��邩�ǂ����������t���O�B
    /// </summary>
    public bool playerRange;

    /// <summary>
    /// �����Ƀs�b�N�A�b�v���邩�ǂ����������t���O�B
    /// </summary>
    [SerializeField] bool FastPickup = false;

    /// <summary>
    /// ����͈͂̋����B
    /// </summary>
    [SerializeField] float ditectionRange = 10f;

    /// <summary>
    /// �Q�[���I�u�W�F�N�g�̖��O�Ɋ�Â��ăA�C�e�������擾���܂��B
    /// </summary>
    /// <param name="objectname">�Q�[���I�u�W�F�N�g</param>
    /// <returns>�A�C�e����</returns>
    public string GetItemName(GameObject objectname)
    {
        switch (objectname.name)
        {
            case "Mana_model":
                ItemName = "�}�i";
                break;
            case "Stone_model":
                ItemName = "�΂���";
                break;
            case "Log_model":
                ItemName = "�ۑ�";
                break;
            case "Mana_model(Clone)":
                ItemName = "�}�i";
                break;
            case "Stone_model(Clone)":
                ItemName = "�΂���";
                break;
            case "Log_model(Clone)":
                ItemName = "�ۑ�";
                break;
        }

        return ItemName;
    }

    /// <summary>
    /// ���t���[���̍X�V�������s���܂��B
    /// </summary>
    private void Update()
    {
        // �����������Ŕ��肷��
        if (PlayerState.Instance.playerBody != null && PlayerState.Instance.playerBody.gameObject != null)
        {
            float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

            if (distance < ditectionRange)
            {
                playerRange = true;
            }
            else
            {
                playerRange = false;
            }
        }

        if (playerRange)
        {
            // FastPickup��true���A�I��������N���b�N������
            if ((Input.GetKeyDown(KeyCode.Mouse0) && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectgameObject == gameObject)
                || FastPickup)
            {
                if (InventorySystem.Instance.CheckSlotAvailable(1))
                {
                    InventorySystem.Instance.AddToinventry(InventryName, true);
                    InventorySystem.Instance.itemsPickedup.Add(gameObject.name);

                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("inventry is full");
                }
            }
        }
    }
}
