using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//担当者　越浦晃生

/// <summary>
/// クラフトを行う際の素材を管理する
/// </summary>
public class BluePrint : MonoBehaviour
{
    /// <summary>
    /// 生産されるアイテムの名前。
    /// </summary>
    public string itemName;

    /// <summary>
    /// 必要な素材1の名前。
    /// </summary>
    public string Req1;

    /// <summary>
    /// 必要な素材2の名前。
    /// </summary>
    public string Req2;

    /// <summary>
    /// 必要な素材1の量。
    /// </summary>
    public int Req1amount;

    /// <summary>
    /// 必要な素材2の量。
    /// </summary>
    public int Req2amount;

    /// <summary>
    /// 必要な素材の種類の数。
    /// </summary>
    public int numOfRequirement;

    /// <summary>
    /// 生産されるアイテムの数。
    /// </summary>
    public int numberOfItemsToProduce;

    /// <summary>
    /// BluePrintクラスのコンストラクタ。
    /// </summary>
    /// <param name="name">アイテムの名前。</param>
    /// <param name="producedItems">生産されるアイテムの数。</param>
    /// <param name="reqNum">必要な素材の種類の数。</param>
    /// <param name="R1">必要な素材1の名前。</param>
    /// <param name="R1Num">必要な素材1の量。</param>
    /// <param name="R2">必要な素材2の名前。</param>
    /// <param name="R2Num">必要な素材2の量。</param>
    public BluePrint(string name, int producedItems, int reqNum, string R1, int R1Num, string R2, int R2Num)
    {
        itemName = name;
        numOfRequirement = reqNum;
        numberOfItemsToProduce = producedItems;
        Req1 = R1;
        Req2 = R2;
        Req1amount = R1Num;
        Req2amount = R2Num;
    }
}
