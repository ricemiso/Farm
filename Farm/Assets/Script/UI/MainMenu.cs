using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//�S���ҁ@�z�Y�W��

/// <summary>
/// ���j���[��ʂ��Ǘ�����N���X�B
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// �Q�[�������[�h����{�^���B
    /// </summary>
    public Button LoadGameBTN;

    /// <summary>
    /// �V�����Q�[�����J�n���܂��B
    /// </summary>
    public void NewGame()
    {
        if (GrobalState.Instance.isTutorialEnd)
        {
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            SceneManager.LoadScene("TutorialScene");
        }
    }

    /// <summary>
    /// ���C�����j���[�ɖ߂�܂��B
    /// </summary>
    public void ReturnMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// �Q�[�����I�����܂��B
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
