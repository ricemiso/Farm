using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �Q�[�����̃��j���[���Ǘ�����N���X�B
/// </summary>
public class inGameMenu : MonoBehaviour
{
    /// <summary>
    /// ���C�����j���[�ɖ߂�܂��B
    /// </summary>
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
