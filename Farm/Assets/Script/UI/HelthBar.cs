using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//担当者　越浦晃生

/// <summary>
/// ヘルスバーを管理するクラス。
/// </summary>
public class HelthBar : MonoBehaviour
{
    /// <summary>
    /// スライダーコンポーネント。
    /// </summary>
    private Slider Slider;

    /// <summary>
    /// ヘルスカウンターのテキストコンポーネント。
    /// </summary>
    public Text healthCounter;

    /// <summary>
    /// プレイヤーの状態を示すゲームオブジェクト。
    /// </summary>
    public GameObject playerState;

    /// <summary>
    /// 現在の体力。
    /// </summary>
    private float currentHealth;

    /// <summary>
    /// 最大体力。
    /// </summary>
    private float maxHealth;

    /// <summary>
    /// 初期設定を行います。
    /// </summary>
    void Awake()
    {
        Slider = GetComponent<Slider>();
    }

    /// <summary>
    /// 体力を更新する
    /// </summary>
    void Update()
    {
        currentHealth = playerState.GetComponent<PlayerState>().currentHealth;
        maxHealth = playerState.GetComponent<PlayerState>().maxHealth;

        float fillValue = currentHealth / maxHealth;
        Slider.value = fillValue;

        healthCounter.text = $"{currentHealth}/{maxHealth}";
    }
}
