using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �N���X�^������ꂽ�ۂ̏������Ǘ�����N���X�B
/// </summary>
public class BreakCrystal : MonoBehaviour
{
    /// <summary>
    /// �����ݒ���s���܂��B
    /// </summary>
    void Start()
    {
        StartCoroutine(delaygameOver());
        PlayerState.Instance.playerBody.SetActive(false);
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
    }

    /// <summary>
    /// �Q�[���I�[�o�[�܂ł̒x���������s���R���[�`���B
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator delaygameOver()
    {
        yield return new WaitForSeconds(10);
    }
}
