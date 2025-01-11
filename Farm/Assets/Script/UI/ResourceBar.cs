using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//担当者　越浦晃生
    
/// <summary>
/// リソースバーを管理するクラス。
/// </summary>
public class ResourceBar : MonoBehaviour
{
    /// <summary>
    /// スライダーコンポーネント。
    /// </summary>
    private Slider Slider;

    /// <summary>
    /// 現在のヘルス。
    /// </summary>
    private float currentHealth;

    /// <summary>
    /// 最大ヘルス。
    /// </summary>
    private float maxHealth;

    /// <summary>
    /// グローバルステートを示すゲームオブジェクト。
    /// </summary>
    public GameObject globalState;

    /// <summary>
    /// 初期設定を行います。
    /// </summary>
    private void Awake()
    {
        Slider = GetComponent<Slider>();
    }

    /// <summary>
    /// 初期設定を行います。
    /// </summary>
    private void Start()
    {
        if (globalState == null)
        {
            globalState = GameObject.Find("GrobalState");
        }
    }

    /// <summary>
    ///体力を更新する
    /// </summary>
    private void Update()
    {
        if (gameObject != null && globalState.TryGetComponent<GrobalState>(out var grobalState))
        {
            currentHealth = grobalState.resourceHelth;
            maxHealth = grobalState.resourceMaxHelth;
        }

        float fillValue = currentHealth / maxHealth;
        Slider.value = fillValue;

        if (Slider.value <= 0)
        {
            currentHealth = 0;
        }
    }
}
