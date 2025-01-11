using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//担当者　越浦晃生

/// <summary>
/// 環境を管理するクラス。
/// </summary>
public class EnviromentManager : MonoBehaviour
{
    /// <summary>
    /// EnviromentManagerのインスタンス。
    /// </summary>
    public static EnviromentManager Instance { get; private set; }

    /// <summary>
    /// 全てのアイテムのゲームオブジェクト。
    /// </summary>
    public GameObject allItems;

    /// <summary>
    /// 全ての木のゲームオブジェクト。
    /// </summary>
    public GameObject allTrees;

    /// <summary>
    /// 全ての配置アイテムのゲームオブジェクト。
    /// </summary>
    public GameObject allPlaceItem;

    /// <summary>
    /// 全ての石のゲームオブジェクト。
    /// </summary>
    public GameObject allStones;

    /// <summary>
    /// クリスタルのゲームオブジェクト。
    /// </summary>
    public GameObject Crystal;

    /// <summary>
    /// 収納のゲームオブジェクト。
    /// </summary>
    public GameObject Storage;

    /// <summary>
    /// シングルトンパターンを適用し、インスタンスを初期化します。
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
