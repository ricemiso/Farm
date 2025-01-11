using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//担当者　越浦晃生

/// <summary>
/// プレイヤーデータを管理するクラス。
/// </summary>
[System.Serializable]
public class PlayerData
{
    /// <summary>
    /// プレイヤーのステータスを格納する配列。
    /// </summary>
    public float[] playerStats;

    /// <summary>
    /// プレイヤーの位置と回転を格納する配列。
    /// </summary>
    public float[] playerPositionAndRotation;

    /// <summary>
    /// インベントリの内容を格納する配列。
    /// </summary>
    public string[] inventortContent;

    /// <summary>
    /// クイックスロットの内容を格納する配列。
    /// </summary>
    public string[] quickSlotContent;

    /// <summary>
    /// PlayerDataクラスのコンストラクタ。
    /// </summary>
    /// <param name="PlayerState">プレイヤーのステータス配列。</param>
    /// <param name="PlayerPosAndRot">プレイヤーの位置と回転配列。</param>
    /// <param name="InventortContent">インベントリの内容配列。</param>
    /// <param name="QuickSlotContent">クイックスロットの内容配列。</param>
    public PlayerData(float[] PlayerState, float[] PlayerPosAndRot, string[] InventortContent, string[] QuickSlotContent)
    {
        playerStats = PlayerState;
        playerPositionAndRotation = PlayerPosAndRot;
        inventortContent = InventortContent;
        quickSlotContent = QuickSlotContent;
    }
}
