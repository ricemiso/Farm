using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �A���[�g�_�C�A���O���Ǘ�����N���X�B
/// </summary>
public class AlertDialogManager : MonoBehaviour
{
    /// <summary>
    /// �_�C�A���O�{�b�N�X�̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    public GameObject dialogBox;

    /// <summary>
    /// ���b�Z�[�W��\������e�L�X�g�R���|�[�l���g�B
    /// </summary>
    public Text messageText;

    /// <summary>
    /// "OK"�{�^���B
    /// </summary>
    public Button okBTN;

    /// <summary>
    /// "�L�����Z��"�{�^���B
    /// </summary>
    public Button cancelBTN;

    /// <summary>
    /// �����R�[���o�b�N�B
    /// </summary>
    private System.Action<bool> responceCallback;

    /// <summary>
    /// �����ݒ���s���܂��B
    /// </summary>
    private void Start()
    {
        dialogBox.SetActive(false);

        okBTN.onClick.AddListener(() => HandleRespose(true));
        cancelBTN.onClick.AddListener(() => HandleRespose(false));
    }

    /// <summary>
    /// �_�C�A���O��\�����܂��B
    /// </summary>
    /// <param name="message">���b�Z�[�W</param>
    /// <param name="callback">�R�[���o�b�N</param>
    public void ShowDialog(string message, System.Action<bool> callback)
    {
        responceCallback = callback;
        messageText.text = message;
        dialogBox.SetActive(true);
    }

    /// <summary>
    /// �������������܂��B
    /// </summary>
    /// <param name="responce">����</param>
    private void HandleRespose(bool responce)
    {
        dialogBox.SetActive(false);
        responceCallback?.Invoke(responce);
    }
}
