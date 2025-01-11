using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//担当者　越浦晃生

/// <summary>
/// マナバーを管理するクラス。
/// </summary>
public class HyderatinBar : MonoBehaviour
{
    /// <summary>
    /// スライダーコンポーネント。
    /// </summary>
    private Slider Slider;

    /// <summary>
    /// マナカウンターのテキストコンポーネント。
    /// </summary>
    public Text hydrationCounter;

    /// <summary>
    /// プレイヤーの状態を示すゲームオブジェクト。
    /// </summary>
    public GameObject playerState;

    /// <summary>
    /// 現在のマナ。
    /// </summary>
    private float currenthydration;

    /// <summary>
    /// 最大マナ。
    /// </summary>
    private float maxhydration;

    /// <summary>
    /// 初期設定を行います。
    /// </summary>
    void Awake()
    {
        Slider = GetComponent<Slider>();
    }

    /// <summary>
    /// マナ量の更新
    /// </summary>
    void Update()
    {
        currenthydration = playerState.GetComponent<PlayerState>().currentHydrationPercent;
        maxhydration = playerState.GetComponent<PlayerState>().maxHydrationPercent;

        float fillValue = currenthydration / maxhydration;
        Slider.value = fillValue;

        if (Slider.value <= 0)
        {
            currenthydration = 0;
        }

        hydrationCounter.text = $"{currenthydration}%";
    }
}
