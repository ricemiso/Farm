using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �험�i���Ǘ�����N���X�B
/// </summary>
public class Lootable : MonoBehaviour
{
    /// <summary>
    /// �험�i�̉\�����X�g�B
    /// </summary>
    public List<LootPossibility> possibilities;

    /// <summary>
    /// �l�������험�i�̃��X�g�B
    /// </summary>
    public List<LootRecieved> finalLoot;

    /// <summary>
    /// �험�i���v�Z���ꂽ���ǂ����������t���O�B
    /// </summary>
    public bool wasLootCalulated;
}

/// <summary>
/// �험�i�̉\����\���N���X�B
/// </summary>
[System.Serializable]
public class LootPossibility
{
    /// <summary>
    /// �험�i�̃A�C�e���B
    /// </summary>
    public GameObject item;

    /// <summary>
    /// �험�i�̍ŏ����ʁB
    /// </summary>
    public int amountMin;

    /// <summary>
    /// �험�i�̍ő吔�ʁB
    /// </summary>
    public int amountMax;
}

/// <summary>
/// �l�������험�i��\���N���X�B
/// </summary>
[System.Serializable]
public class LootRecieved
{
    /// <summary>
    /// �l�������험�i�̃A�C�e���B
    /// </summary>
    public GameObject item;

    /// <summary>
    /// �l�������험�i�̐��ʁB
    /// </summary>
    public int amount;
}
