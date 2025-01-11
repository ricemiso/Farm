using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//�S���ҁ@�z�Y�W��
//���݃Z�[�u���[�h���Ή����Ă��Ȃ�

/// <summary>
/// ���[�h�X���b�g���Ǘ�����N���X�B
/// </summary>
public class LoadSlot : MonoBehaviour
{
    /// <summary>
    /// �{�^���R���|�[�l���g�B
    /// </summary>
    public Button button;

    /// <summary>
    /// �{�^���̃e�L�X�g�R���|�[�l���g�B
    /// </summary>
    public TextMeshProUGUI buttonText;

    /// <summary>
    /// �X���b�g�ԍ��B
    /// </summary>
    public int slotNumber;

    /// <summary>
    /// �����ݒ���s���܂��B
    /// </summary>
    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// ���t���[���̍X�V�������s���܂��B
    /// </summary>
    private void Update()
    {
        if (MainMenuSaveManager.Instance.IsSlotEmpty(slotNumber))
        {
            buttonText.text = "";
        }
        else
        {
            buttonText.text = PlayerPrefs.GetString("Slot" + slotNumber + "Description");
        }
    }

    /// <summary>
    /// �{�^�����N���b�N���ꂽ���̏������s���܂��B
    /// </summary>
    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (!MainMenuSaveManager.Instance.IsSlotEmpty(slotNumber))
            {
                MainMenuSaveManager.Instance.StartLoadedGame(slotNumber);
                MainMenuSaveManager.Instance.DeselectButton();
            }
        });
    }
}
