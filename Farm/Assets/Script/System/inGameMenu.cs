using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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
		//停止中にExitを押したら、MenuManager.csで以下の処理が行われずにメインメニューに戻るため、ここで処理を実行する
		Time.timeScale = 1.0f;
		AudioListener.pause = false;
		MenuManager.Instance.isCrystalMove = true;

		MenuManager.Instance.savemenu.SetActive(false);
		MenuManager.Instance.settingmenu.SetActive(false);
		MenuManager.Instance.menu.SetActive(true);

		MenuManager.Instance.UICanvas.SetActive(true);
		MenuManager.Instance.menuCanvas.SetActive(false);

		MenuManager.Instance.isMenuOpen = false;

		SelectionManager.Instance.EnableSelection();
		SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;

		SceneManager.LoadScene("MainMenu");
    }
}
