using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�S���ҁ@�z�Y�W��

/// <summary>
/// ���j���[�̊J�n����у��[�h�{�^�����Ǘ�����N���X�B
/// </summary>
public class MenuStartLoadButton : MonoBehaviour
{
    /// <summary>
    /// �����ݒ���s���܂��B
    /// </summary>
    private void Start()
    {
        SoundManager.Instance.StopSound(SoundManager.Instance.gameClearBGM);
        SoundManager.Instance.StopSound(SoundManager.Instance.gameOverBGM);
        SoundManager.Instance.PlaySound(SoundManager.Instance.startingZoneBGMMusic);
    }

    /// <summary>
    /// ���j���[�{�^�����N���b�N���ꂽ���̏������s���܂��B
    /// </summary>
    /// <param name="sceneName">�V�[����</param>
    public void MunuBottun(string sceneName)
    {
        MainMenuSaveManager.Instance.StartLoadedGame(sceneName);
    }
}
