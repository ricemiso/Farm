using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �X�g���[�W���Ǘ�����N���X�B
/// </summary>
public class StorageManager : MonoBehaviour
{
    /// <summary>
    /// StorageManager�̃C���X�^���X�B
    /// </summary>
    public static StorageManager Instance { get; set; }

    /// <summary>
    /// �����Ȏ��[�{�b�N�X��UI�B
    /// </summary>
    [SerializeField] GameObject storageBoxSmallUI;

    /// <summary>
    /// �I�����ꂽ���[�{�b�N�X�B
    /// </summary>
    [SerializeField] StrageBox selectedStorage;

    /// <summary>
    /// ���[UI���J����Ă��邩�ǂ����������t���O�B
    /// </summary>
    public bool storageUIOpen;

    /// <summary>
    /// �V���O���g���p�^�[����K�p���A�C���X�^���X�����������܂��B
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
    /// ���[�{�b�N�X���J���܂��B
    /// </summary>
    /// <param name="storage">�J�����[�{�b�N�X</param>
    public void OpenBox(StrageBox storage)
    {
        SetSelectedStorage(storage);
        PopulateStorage(GetRelevantUI(selectedStorage));

        GetRelevantUI(selectedStorage).SetActive(true);
        storageUIOpen = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SelectionManager.Instance.DisableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
    }

    /// <summary>
    /// ���[UI�ɃA�C�e����z�u���܂��B
    /// </summary>
    /// <param name="storageUI">���[UI</param>
    private void PopulateStorage(GameObject storageUI)
    {
        // UI�̑S�ẴX���b�g���擾
        List<GameObject> uiSlots = new List<GameObject>();

        foreach (Transform child in storageUI.transform)
        {
            uiSlots.Add(child.gameObject);
        }

        // �v���n�u���C���X�^���X�����A���ꂼ���GameObject�̎q�Ƃ��Đݒ�
        foreach (string name in selectedStorage.items)
        {
            foreach (GameObject slot in uiSlots)
            {
                if (slot.transform.childCount < 1)
                {
                    var resource = Resources.Load<GameObject>(name);
                    if (resource == null)
                    {
                        Debug.Log($"Failed to load resource: {name}");
                        continue;  // ���̃A�C�e����
                    }

                    var itemToAdd = Instantiate(resource, slot.transform.position, slot.transform.rotation);
                    itemToAdd.name = name;
                    itemToAdd.transform.SetParent(slot.transform);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// ���[�{�b�N�X����܂��B
    /// </summary>
    public void CloseBox()
    {
        RecalculateStorage(GetRelevantUI(selectedStorage));

        GetRelevantUI(selectedStorage).SetActive(false);
        storageUIOpen = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SelectionManager.Instance.EnableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
    }

    /// <summary>
    /// ���[�f�[�^���Čv�Z���܂��B
    /// </summary>
    /// <param name="storageUI">���[UI</param>
    private void RecalculateStorage(GameObject storageUI)
    {
        List<GameObject> uiList = new List<GameObject>();

        foreach (Transform child in storageUI.transform)
        {
            uiList.Add(child.gameObject);
        }

        selectedStorage.items.Clear();
        List<GameObject> toBeDeleted = new List<GameObject>();

        foreach (GameObject slot in uiList)
        {
            if (slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name;
                string str2 = "(Clone)";
                string result = name.Replace(str2, "");

                selectedStorage.items.Add(result);
                toBeDeleted.Add(slot.transform.GetChild(0).gameObject);
            }
        }

        foreach (GameObject obj in toBeDeleted)
        {
            Destroy(obj);
        }
    }

    /// <summary>
    /// �I�����ꂽ���[��ݒ肵�܂��B
    /// </summary>
    /// <param name="storage">�I�����ꂽ���[</param>
    public void SetSelectedStorage(StrageBox storage)
    {
        selectedStorage = storage;
    }

    /// <summary>
    /// �֘A����UI���擾���܂��B
    /// </summary>
    /// <param name="storage">���[</param>
    /// <returns>�֘A����UI��GameObject</returns>
    private GameObject GetRelevantUI(StrageBox storage)
    {
        // ���̃^�C�v�̃X�C�b�`���쐬
        return storageBoxSmallUI;
    }
}
