using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

//担当者　越浦晃生

/// <summary>
/// セーブスロットを管理するクラス。
/// </summary>
public class SaveSlot : MonoBehaviour
{
    /// <summary>
    /// ボタンコンポーネント。
    /// </summary>
    private Button button;

    /// <summary>
    /// ボタンのテキストコンポーネント。
    /// </summary>
    private TextMeshProUGUI buttonText;

    /// <summary>
    /// スロット番号。
    /// </summary>
    public int slotNumber;

    /// <summary>
    /// アラートUIのゲームオブジェクト。
    /// </summary>
    public GameObject alertUI;

    /// <summary>
    /// "Yes"ボタン。
    /// </summary>
    Button yesBTN;

    /// <summary>
    /// "No"ボタン。
    /// </summary>
    Button noBTN;

    /// <summary>
    /// 初期設定を行います。
    /// </summary>
    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        yesBTN = alertUI.transform.Find("YesButton").GetComponent<Button>();
        noBTN = alertUI.transform.Find("NoButton").GetComponent<Button>();
    }

    /// <summary>
    /// ボタンがクリックされた時の処理を行います。
    /// </summary>
    public void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (MainMenuSaveManager.Instance.IsSlotEmpty(slotNumber))
            {
                SaveGameCinfig();
            }
            else
            {
                DisplayOverrideWarning();
            }
        });
    }

    /// <summary>
    /// 毎フレームの更新処理を行います。
    /// </summary>
    private void Update()
    {
        if (MainMenuSaveManager.Instance.IsSlotEmpty(slotNumber))
        {
            buttonText.text = "Empty";
        }
        else
        {
            buttonText.text = PlayerPrefs.GetString("Slot" + slotNumber + "Description");
        }
    }

    /// <summary>
    /// 上書き警告を表示します。
    /// </summary>
    public void DisplayOverrideWarning()
    {
        alertUI.SetActive(true);

        yesBTN.onClick.AddListener(() =>
        {
            SaveGameCinfig();
            alertUI.SetActive(false);
        });

        noBTN.onClick.AddListener(() =>
        {
            alertUI.SetActive(false);
        });
    }

    /// <summary>
    /// ゲームのセーブ設定を行います。
    /// </summary>
    private void SaveGameCinfig()
    {
        MainMenuSaveManager.Instance.SaveGame(slotNumber);

        DateTime dt = DateTime.Now;
        string time = dt.ToString("yyyy-MM-dd HH:mm");

        string description = "Save Game " + slotNumber + " | " + time;

        buttonText.text = description;

        PlayerPrefs.SetString("Slot" + slotNumber + "Description", description);
        Debug.Log("Saved Description: " + description); // デバッグログを追加

        MainMenuSaveManager.Instance.DeselectButton();
    }
}
