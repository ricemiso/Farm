using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//担当者　越浦晃生

/// <summary>
/// 状態を管理するクラス。
/// </summary>
public class GrobalState : MonoBehaviour
{
    /// <summary>
    /// GrobalStateのインスタンス。
    /// </summary>
    public static GrobalState Instance { get; set; }

    /// <summary>
    /// リソースのヘルス。
    /// </summary>
    public float resourceHelth;

    /// <summary>
    /// リソースの最大ヘルス。
    /// </summary>
    public float resourceMaxHelth;

    /// <summary>
    /// リソースのマナ。
    /// </summary>
    public float resourceMana;

    /// <summary>
    /// レベル。
    /// </summary>
    public int level;

    /// <summary>
    /// ダメージ。
    /// </summary>
    public int damage;

    // チュートリアル用の変数
    public bool isTreeChopped = false;
    public bool isStoneChopped = false;
    public bool isTutorialEnd = false;
    public bool isSkip = false;
    public bool isWater = false;
    public bool isDamage = false;
    public bool isLoot = false;
    public bool isFarm1 = false;
    public bool isDeath = false;
    public bool isManaCraft = false;
    public bool isInfoTask = false;

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

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 初期化処理を行います。
    /// </summary>
    public void Initialize()
    {
        resourceHelth = 0;
        resourceMaxHelth = 0;
        resourceMana = 0;
        level = 0;
    }
}
