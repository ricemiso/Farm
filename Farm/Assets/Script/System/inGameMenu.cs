using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//担当者　越浦晃生

/// <summary>
/// ゲーム内のメニューを管理するクラス。
/// </summary>
public class inGameMenu : MonoBehaviour
{
    /// <summary>
    /// メインメニューに戻ります。
    /// </summary>
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
