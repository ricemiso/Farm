using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//担当者　越浦晃生

/// <summary>
/// ゲーム内の日数を管理するクラス。
/// </summary>
public class TimeManager : MonoBehaviour
{
    /// <summary>
    /// TimeManagerのインスタンス。
    /// </summary>
    public static TimeManager Instance { get; set; }

    /// <summary>
    /// 1日が経過したときのイベント。
    /// </summary>
    public UnityEvent oneDayPass = new UnityEvent();

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
    /// ゲーム内の日数。
    /// </summary>
    public int dayInGame = 1;

    /// <summary>
    /// 日数表示用のテキストUI。
    /// </summary>
    public Text dayUI;

    /// <summary>
    /// 初期設定を行います。
    /// </summary>
    private void Start()
    {
        dayUI.text = $"{dayInGame}日目";
    }

    /// <summary>
    /// 次の日をトリガーします。
    /// </summary>
    public void TriggerNextDay()
    {
        dayInGame += 1;
        dayUI.text = $"{dayInGame}日目";

        oneDayPass.Invoke();
    }
}
