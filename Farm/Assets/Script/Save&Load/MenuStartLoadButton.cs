using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//担当者　越浦晃生

/// <summary>
/// メニューの開始およびロードボタンを管理するクラス。
/// </summary>
public class MenuStartLoadButton : MonoBehaviour
{
    /// <summary>
    /// 初期設定を行います。
    /// </summary>
    private void Start()
    {
        SoundManager.Instance.StopSound(SoundManager.Instance.gameClearBGM);
        SoundManager.Instance.StopSound(SoundManager.Instance.gameOverBGM);
        SoundManager.Instance.PlaySound(SoundManager.Instance.startingZoneBGMMusic);
    }

    /// <summary>
    /// メニューボタンがクリックされた時の処理を行います。
    /// </summary>
    /// <param name="sceneName">シーン名</param>
    public void MunuBottun(string sceneName)
    {
        MainMenuSaveManager.Instance.StartLoadedGame(sceneName);
    }
}
