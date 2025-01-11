using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//担当者　越浦晃生

/// <summary>
/// 環境データを管理するクラス。
/// </summary>
[System.Serializable]
public class EnviromentData
{
    /// <summary>
    /// 拾ったアイテムのリスト。
    /// </summary>
    public List<string> pickedupItems;

    /// <summary>
    /// 木のデータのリスト。
    /// </summary>
    public List<TreeData> TreeData;

    /// <summary>
    /// 石のデータのリスト。
    /// </summary>
    public List<StoneData> StoneData;

    /// <summary>
    /// 配置されたアイテムのデータリスト。
    /// </summary>
    public List<ConstructionData> PlaceItems;

    /// <summary>
    /// 収納データのリスト。
    /// </summary>
    public List<StorageData> storages;

    /// <summary>
    /// クリスタルのデータのリスト。
    /// </summary>
    public List<CrystalData> crystalData;

    /// <summary>
    /// EnviromentDataクラスのコンストラクタ。
    /// </summary>
    /// <param name="_PickedupItems">拾ったアイテムのリスト</param>
    /// <param name="_TreeData">木のデータのリスト</param>
    /// <param name="_PlaceItems">配置されたアイテムのデータリスト</param>
    /// <param name="_StoneData">石のデータのリスト</param>
    /// <param name="_storages">収納データのリスト</param>
    /// <param name="_crystalData">クリスタルのデータのリスト</param>
    public EnviromentData(List<string> _PickedupItems, List<TreeData> _TreeData, List<ConstructionData> _PlaceItems, List<StoneData> _StoneData, List<StorageData> _storages, List<CrystalData> _crystalData)
    {
        pickedupItems = _PickedupItems;
        TreeData = _TreeData;
        PlaceItems = _PlaceItems;
        StoneData = _StoneData;
        storages = _storages;
        crystalData = _crystalData;
    }
}

/// <summary>
/// 木のデータを管理するクラス。
/// </summary>
[System.Serializable]
public class TreeData
{
    /// <summary>
    /// 名前。
    /// </summary>
    public string name;

    /// <summary>
    /// 位置。
    /// </summary>
    public Vector3 position;

    /// <summary>
    /// 回転。
    /// </summary>
    public Vector3 rotation;

    /// <summary>
    /// 現在のHP。
    /// </summary>
    public float currentHP;
}

/// <summary>
/// クリスタルのデータを管理するクラス。
/// </summary>
[System.Serializable]
public class CrystalData
{
    /// <summary>
    /// 名前。
    /// </summary>
    public string name;

    /// <summary>
    /// 位置。
    /// </summary>
    public Vector3 position;

    /// <summary>
    /// 回転。
    /// </summary>
    public Vector3 rotation;

    /// <summary>
    /// 現在のHP。
    /// </summary>
    public float currentHP;
}

/// <summary>
/// 石のデータを管理するクラス。
/// </summary>
[System.Serializable]
public class StoneData
{
    /// <summary>
    /// 名前。
    /// </summary>
    public string name;

    /// <summary>
    /// 位置。
    /// </summary>
    public Vector3 position;

    /// <summary>
    /// 回転。
    /// </summary>
    public Vector3 rotation;

    /// <summary>
    /// 現在のHP。
    /// </summary>
    public float currentHP;
}

/// <summary>
/// 建造物のデータを管理するクラス。
/// </summary>
[System.Serializable]
public class ConstructionData
{
    /// <summary>
    /// 名前。
    /// </summary>
    public string name;

    /// <summary>
    /// 位置。
    /// </summary>
    public Vector3 position;

    /// <summary>
    /// 回転。
    /// </summary>
    public Vector3 rotation;

    /// <summary>
    /// 現在のHP。
    /// </summary>
    public float currentHP;
}

/// <summary>
/// 収納データを管理するクラス。
/// </summary>
[System.Serializable]
public class StorageData
{
    /// <summary>
    /// アイテム名のリスト。
    /// </summary>
    public List<string> itemsname;

    /// <summary>
    /// 位置。
    /// </summary>
    public Vector3 position;

    /// <summary>
    /// 回転。
    /// </summary>
    public Vector3 rotation;
}
