using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//担当者　越浦晃生
//現在セーブロードが対応していない

/// <summary>
/// ロードスロットを管理するクラス。
/// </summary>
public class LoadSlot : MonoBehaviour
{
    /// <summary>
    /// ボタンコンポーネント。
    /// </summary>
    public Button button;

    /// <summary>
    /// ボタンのテキストコンポーネント。
    /// </summary>
    public TextMeshProUGUI buttonText;

    /// <summary>
    /// スロット番号。
    /// </summary>
    public int slotNumber;

    /// <summary>
    /// 初期設定を行います。
    /// </summary>
    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 毎フレームの更新処理を行います。
    /// </summary>
    private void Update()
    {
        if (MainMenuSaveManager.Instance.IsSlotEmpty(slotNumber))
        {
            buttonText.text = "";
        }
        else
        {
            buttonText.text = PlayerPrefs.GetString("Slot" + slotNumber + "Description");
        }
    }

    /// <summary>
    /// ボタンがクリックされた時の処理を行います。
    /// </summary>
    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (!MainMenuSaveManager.Instance.IsSlotEmpty(slotNumber))
            {
                MainMenuSaveManager.Instance.StartLoadedGame(slotNumber);
                MainMenuSaveManager.Instance.DeselectButton();
            }
        });
    }
}
