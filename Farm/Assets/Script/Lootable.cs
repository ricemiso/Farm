using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootable : MonoBehaviour
{
    public List<LootPossibility> possibilities;
    public List<LootRecieved> finalLoot;

    public bool wasLootCalulated;
}

[System.Serializable]
public class LootPossibility
{
    public GameObject item;
    public int amountMin;
    public int amountMax;
}

[System.Serializable]
public class LootRecieved
{
    public GameObject item;
    public int amount;
}
