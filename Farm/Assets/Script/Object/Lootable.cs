using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//担当者　越浦晃生

/// <summary>
/// 戦利品を管理するクラス。
/// </summary>
public class Lootable : MonoBehaviour
{
    /// <summary>
    /// 戦利品の可能性リスト。
    /// </summary>
    public List<LootPossibility> possibilities;

    /// <summary>
    /// 獲得した戦利品のリスト。
    /// </summary>
    public List<LootRecieved> finalLoot;

    /// <summary>
    /// 戦利品が計算されたかどうかを示すフラグ。
    /// </summary>
    public bool wasLootCalulated;
}

/// <summary>
/// 戦利品の可能性を表すクラス。
/// </summary>
[System.Serializable]
public class LootPossibility
{
    /// <summary>
    /// 戦利品のアイテム。
    /// </summary>
    public GameObject item;

    /// <summary>
    /// 戦利品の最小数量。
    /// </summary>
    public int amountMin;

    /// <summary>
    /// 戦利品の最大数量。
    /// </summary>
    public int amountMax;
}

/// <summary>
/// 獲得した戦利品を表すクラス。
/// </summary>
[System.Serializable]
public class LootRecieved
{
    /// <summary>
    /// 獲得した戦利品のアイテム。
    /// </summary>
    public GameObject item;

    /// <summary>
    /// 獲得した戦利品の数量。
    /// </summary>
    public int amount;
}
