using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//担当者　越浦晃生

/// <summary>
/// ゲーム全体のデータを管理するクラス。
/// </summary>
[System.Serializable]
public class AllGameData
{
    /// <summary>
    /// プレイヤーデータ。
    /// </summary>
    public PlayerData playerData;

    /// <summary>
    /// 環境データ。
    /// </summary>
    public EnviromentData enviromentData;

    // public ConstructionData constructionData;
}
