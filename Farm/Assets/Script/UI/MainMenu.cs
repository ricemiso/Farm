using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//担当者　越浦晃生

/// <summary>
/// メニュー画面を管理するクラス。
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// ゲームをロードするボタン。
    /// </summary>
    public Button LoadGameBTN;

    /// <summary>
    /// 新しいゲームを開始します。
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
    /// メインメニューに戻ります。
    /// </summary>
    public void ReturnMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// ゲームを終了します。
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
