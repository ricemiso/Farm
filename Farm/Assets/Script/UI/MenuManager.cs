using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//担当者　越浦晃生

/// <summary>
/// メニューを管理するクラス。
/// </summary>
public class MenuManager : MonoBehaviour
{
    /// <summary>
    /// メニューのインスタンス。
    /// </summary>
    public static MenuManager Instance { get; set; }

    /// <summary>
    /// メニューキャンバスのゲームオブジェクト。
    /// </summary>
    public GameObject menuCanvas;

    /// <summary>
    /// UIキャンバスのゲームオブジェクト。
    /// </summary>
    public GameObject UICanvas;

    /// <summary>
    /// セーブメニューのゲームオブジェクト。
    /// </summary>
    public GameObject savemenu;

    /// <summary>
    /// 設定メニューのゲームオブジェクト。
    /// </summary>
    public GameObject settingmenu;

    /// <summary>
    /// メニューのゲームオブジェクト。
    /// </summary>
    public GameObject menu;

    /// <summary>
    /// メニューが開いているかどうかを示すフラグ。
    /// </summary>
    public bool isMenuOpen;

    /// <summary>
    /// クリスタルが動いているかどうかを示すフラグ。
    /// </summary>
    public bool isCrystalMove;

    /// <summary>
    /// シングルトンパターンを適用し、インスタンスを初期化します。
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    /// <summary>
    /// 初期設定を行います。
    /// </summary>
    private void Start()
    {
        isCrystalMove = true;
    }

    /// <summary>
    /// メニュー画面の表示
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && !isMenuOpen)
        {
            UICanvas.SetActive(false);
            menuCanvas.SetActive(true);
            savemenu.SetActive(false);

            isMenuOpen = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isCrystalMove = false;
            Time.timeScale = 0f;
            AudioListener.pause = true;
        }
        else if (Input.GetKeyDown(KeyCode.M) && isMenuOpen)
        {
            Time.timeScale = 1.0f;
            AudioListener.pause = false;
            isCrystalMove = true;

            savemenu.SetActive(false);
            settingmenu.SetActive(false);
            menu.SetActive(true);

            UICanvas.SetActive(true);
            menuCanvas.SetActive(false);

            isMenuOpen = false;

            if (!CraftingSystem.Instance.isOpen && !InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
        }
    }
}
