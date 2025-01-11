using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//担当者　越浦晃生

/// <summary>
/// カロリーバーを管理するクラス。
/// </summary>
public class CalodesBar : MonoBehaviour
{
    /// <summary>
    /// スライダーコンポーネント。
    /// </summary>
    private Slider Slider;

    /// <summary>
    /// カロリーカウンターのテキストコンポーネント。
    /// </summary>
    public Text caloriesCounter;

    /// <summary>
    /// プレイヤーの状態を示すゲームオブジェクト。
    /// </summary>
    public GameObject playerState;

    /// <summary>
    /// 現在のカロリー。
    /// </summary>
    private float currentcalories;

    /// <summary>
    /// 最大カロリー。
    /// </summary>
    private float maxcalories;

    /// <summary>
    /// 通常の速度。
    /// </summary>
    float normalSpeedRate = 1.0f;

    /// <summary>
    /// 弱体化した時の速度。
    /// </summary>
    public float weakSpeedRate;

    /// <summary>
    /// 初期設定を行います。
    /// </summary>
    void Awake()
    {
        Slider = GetComponent<Slider>();
    }

    /// <summary>
    /// カロリーを更新し続ける
    /// </summary>
    void Update()
    {
        currentcalories = playerState.GetComponent<PlayerState>().currentCalories;
        maxcalories = playerState.GetComponent<PlayerState>().maxCalories;

        float fillValue = currentcalories / maxcalories;
        Slider.value = fillValue;

        if (Slider.value >= 0.3f)
        {
            PlayerState.Instance.setPlayerSpeedRate(normalSpeedRate);
        }
        else
        {
            PlayerState.Instance.setPlayerSpeedRate(weakSpeedRate);
        }

        // デバッグ用
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerState.Instance.currentCalories = 1;
        }

        if (Slider.value <= 0)
        {
            currentcalories = 0;
        }

        caloriesCounter.text = $"{currentcalories}/{maxcalories}";
    }
}
