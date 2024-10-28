using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Todo:ÉZÅ[ÉuÇ∑ÇÈéÌóﬁÇí«â¡Ç∑ÇÈ

[System.Serializable]
public class EnviromentData
{
    public List<string> pickedupItems;

    public List<TreeData> TreeData;

    public List<StoneData> StoneData;

    public List<ConstructionData> PlaceItems;

    public List<StorageData> storages;

    public EnviromentData(List<string> _PickedupItems, List<TreeData> _TreeData, List<ConstructionData> _PlaceItems, List<StoneData> _StoneData
        , List<StorageData> _storages)
    {
        pickedupItems = _PickedupItems;
        TreeData = _TreeData;
        PlaceItems = _PlaceItems;
        StoneData = _StoneData;
        storages = _storages;
    }


}

[System.Serializable]
public class TreeData
{
    public string name;
    public Vector3 position;
    public Vector3 rotation;
    public float currentHP;
}

[System.Serializable]
public class StoneData
{
    public string name;
    public Vector3 position;
    public Vector3 rotation;
    public float currentHP;
}

[System.Serializable]
public class ConstructionData
{
    public string name;
    public Vector3 position;
    public Vector3 rotation;
    public float currentHP;
}

[System.Serializable]
public class StorageData
{
    public List<string> itemsname;
    public Vector3 position;
    public Vector3 rotation;
}