using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 植物を制御するクラス。成長や収穫の管理を行う。  
/// 土壌の状態（空か、植えられているか）や植物の成長を監視し、ユーザーが植物に水やりをする処理を管理
/// </summary>
public class Soil : MonoBehaviour
{
    /// <summary>
    /// 土壌が空かどうかを示すフラグ。初期状態では空。
    /// </summary>
    public bool isEmpty = true;

    /// <summary>
    /// プレイヤーが土壌の近くにいるかどうかを示すフラグ。
    /// </summary>
    public bool playerInRange;

    /// <summary>
    /// 現在植えられている植物の名前。
    /// </summary>
    public string plantName;

    /// <summary>
    /// 現在植えられている植物のインスタンス。
    /// </summary>
    public Plant currentplant;

    /// <summary>
    /// 土壌のデフォルトのマテリアル。
    /// </summary>
    public Material defaltMaterial;

    /// <summary>
    /// 水を与えられた際の土壌のマテリアル。
    /// </summary>
    public Material waterMaterial;

    /// <summary>
    /// プレイヤーと土壌の間の距離を測るための変数。
    /// </summary>
    float distance = 0;

    /// <summary>
    /// プレイヤーの位置に基づいて土壌との距離を計算し、プレイヤーが土壌に近いかどうかを判断する。
    /// </summary>
    private void Update()
    {
        if (PlayerState.Instance.currentHealth <= 0) return;

        // プレイヤーとの距離を測定
        if (PlayerState.Instance.playerBody != null)
        {
            distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);
        }

        // プレイヤーが土壌の近くにいるかを判断
        if (distance < 10f)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }
    }

    /// <summary>
    /// プレイヤーが選択した種を土壌に植える。
    /// </summary>
    internal void PlantSeed()
    {
        // 選択された種アイテムを取得
        InventoryItem selectedSeed = EquipSystem.Instance.selectedItem.GetComponent<InventoryItem>();
        isEmpty = false; // 土壌が埋まったことを示すフラグを設定

        // 種の名前から「の種」を削除して植物名を取得
        string onlyPlantName = selectedSeed.thisName.Split(new string[] { "の種" }, StringSplitOptions.None)[0];

        plantName = onlyPlantName;

        // 植物を生成して土壌に配置
        GameObject instancePlant = Instantiate(Resources.Load($"{onlyPlantName}Plant") as GameObject);

        instancePlant.transform.parent = gameObject.transform;
        Vector3 plantPos = Vector3.zero;
        plantPos.y = 0f;
        instancePlant.transform.localPosition = plantPos;

        // 植物のインスタンスを取得し、植えた日を設定
        currentplant = instancePlant.GetComponent<Plant>();
        currentplant.dayOfPlanting = TimeManager.Instance.dayInGame;
    }

    /// <summary>
    /// 土壌に水を与えた状態にする。
    /// </summary>
    internal void MakeSoilWatered()
    {
        GetComponent<Renderer>().material = waterMaterial; // 水を与えた状態のマテリアルに変更
    }

    /// <summary>
    /// 土壌に水を与えていない状態に戻す。
    /// </summary>
    internal void MakeSoilNotWatered()
    {
        GetComponent<Renderer>().material = defaltMaterial; // デフォルトのマテリアルに戻す
    }
}

