using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �Z�[�u�X���b�g���Ǘ�����N���X�B
/// </summary>
public class SaveSlot : MonoBehaviour
{
    /// <summary>
    /// �{�^���R���|�[�l���g�B
    /// </summary>
    private Button button;

    /// <summary>
    /// �{�^���̃e�L�X�g�R���|�[�l���g�B
    /// </summary>
    private TextMeshProUGUI buttonText;

    /// <summary>
    /// �X���b�g�ԍ��B
    /// </summary>
    public int slotNumber;

    /// <summary>
    /// �A���[�gUI�̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    public GameObject alertUI;

    /// <summary>
    /// "Yes"�{�^���B
    /// </summary>
    Button yesBTN;

    /// <summary>
    /// "No"�{�^���B
    /// </summary>
    Button noBTN;

    /// <summary>
    /// �����ݒ���s���܂��B
    /// </summary>
    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        yesBTN = alertUI.transform.Find("YesButton").GetComponent<Button>();
        noBTN = alertUI.transform.Find("NoButton").GetComponent<Button>();
    }

    /// <summary>
    /// �{�^�����N���b�N���ꂽ���̏������s���܂��B
    /// </summary>
    public void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (MainMenuSaveManager.Instance.IsSlotEmpty(slotNumber))
            {
                SaveGameCinfig();
            }
            else
            {
                DisplayOverrideWarning();
            }
        });
    }

    /// <summary>
    /// ���t���[���̍X�V�������s���܂��B
    /// </summary>
    private void Update()
    {
        if (MainMenuSaveManager.Instance.IsSlotEmpty(slotNumber))
        {
            buttonText.text = "Empty";
        }
        else
        {
            buttonText.text = PlayerPrefs.GetString("Slot" + slotNumber + "Description");
        }
    }

    /// <summary>
    /// �㏑���x����\�����܂��B
    /// </summary>
    public void DisplayOverrideWarning()
    {
        alertUI.SetActive(true);

        yesBTN.onClick.AddListener(() =>
        {
            SaveGameCinfig();
            alertUI.SetActive(false);
        });

        noBTN.onClick.AddListener(() =>
        {
            alertUI.SetActive(false);
        });
    }

    /// <summary>
    /// �Q�[���̃Z�[�u�ݒ���s���܂��B
    /// </summary>
    private void SaveGameCinfig()
    {
        MainMenuSaveManager.Instance.SaveGame(slotNumber);

        DateTime dt = DateTime.Now;
        string time = dt.ToString("yyyy-MM-dd HH:mm");

        string description = "Save Game " + slotNumber + " | " + time;

        buttonText.text = description;

        PlayerPrefs.SetString("Slot" + slotNumber + "Description", description);
        Debug.Log("Saved Description: " + description); // �f�o�b�O���O��ǉ�

        MainMenuSaveManager.Instance.DeselectButton();
    }
}
