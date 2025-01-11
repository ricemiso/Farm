using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//担当者　越浦晃生

/// <summary>
/// アラートダイアログを管理するクラス。
/// </summary>
public class AlertDialogManager : MonoBehaviour
{
    /// <summary>
    /// ダイアログボックスのゲームオブジェクト。
    /// </summary>
    public GameObject dialogBox;

    /// <summary>
    /// メッセージを表示するテキストコンポーネント。
    /// </summary>
    public Text messageText;

    /// <summary>
    /// "OK"ボタン。
    /// </summary>
    public Button okBTN;

    /// <summary>
    /// "キャンセル"ボタン。
    /// </summary>
    public Button cancelBTN;

    /// <summary>
    /// 応答コールバック。
    /// </summary>
    private System.Action<bool> responceCallback;

    /// <summary>
    /// 初期設定を行います。
    /// </summary>
    private void Start()
    {
        dialogBox.SetActive(false);

        okBTN.onClick.AddListener(() => HandleRespose(true));
        cancelBTN.onClick.AddListener(() => HandleRespose(false));
    }

    /// <summary>
    /// ダイアログを表示します。
    /// </summary>
    /// <param name="message">メッセージ</param>
    /// <param name="callback">コールバック</param>
    public void ShowDialog(string message, System.Action<bool> callback)
    {
        responceCallback = callback;
        messageText.text = message;
        dialogBox.SetActive(true);
    }

    /// <summary>
    /// 応答を処理します。
    /// </summary>
    /// <param name="responce">応答</param>
    private void HandleRespose(bool responce)
    {
        dialogBox.SetActive(false);
        responceCallback?.Invoke(responce);
    }
}
