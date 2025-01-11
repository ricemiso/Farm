using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�S���ҁ@�z�Y�W��

/// <summary>
/// ���f�[�^���Ǘ�����N���X�B
/// </summary>
[System.Serializable]
public class EnviromentData
{
    /// <summary>
    /// �E�����A�C�e���̃��X�g�B
    /// </summary>
    public List<string> pickedupItems;

    /// <summary>
    /// �؂̃f�[�^�̃��X�g�B
    /// </summary>
    public List<TreeData> TreeData;

    /// <summary>
    /// �΂̃f�[�^�̃��X�g�B
    /// </summary>
    public List<StoneData> StoneData;

    /// <summary>
    /// �z�u���ꂽ�A�C�e���̃f�[�^���X�g�B
    /// </summary>
    public List<ConstructionData> PlaceItems;

    /// <summary>
    /// ���[�f�[�^�̃��X�g�B
    /// </summary>
    public List<StorageData> storages;

    /// <summary>
    /// �N���X�^���̃f�[�^�̃��X�g�B
    /// </summary>
    public List<CrystalData> crystalData;

    /// <summary>
    /// EnviromentData�N���X�̃R���X�g���N�^�B
    /// </summary>
    /// <param name="_PickedupItems">�E�����A�C�e���̃��X�g</param>
    /// <param name="_TreeData">�؂̃f�[�^�̃��X�g</param>
    /// <param name="_PlaceItems">�z�u���ꂽ�A�C�e���̃f�[�^���X�g</param>
    /// <param name="_StoneData">�΂̃f�[�^�̃��X�g</param>
    /// <param name="_storages">���[�f�[�^�̃��X�g</param>
    /// <param name="_crystalData">�N���X�^���̃f�[�^�̃��X�g</param>
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
/// �؂̃f�[�^���Ǘ�����N���X�B
/// </summary>
[System.Serializable]
public class TreeData
{
    /// <summary>
    /// ���O�B
    /// </summary>
    public string name;

    /// <summary>
    /// �ʒu�B
    /// </summary>
    public Vector3 position;

    /// <summary>
    /// ��]�B
    /// </summary>
    public Vector3 rotation;

    /// <summary>
    /// ���݂�HP�B
    /// </summary>
    public float currentHP;
}

/// <summary>
/// �N���X�^���̃f�[�^���Ǘ�����N���X�B
/// </summary>
[System.Serializable]
public class CrystalData
{
    /// <summary>
    /// ���O�B
    /// </summary>
    public string name;

    /// <summary>
    /// �ʒu�B
    /// </summary>
    public Vector3 position;

    /// <summary>
    /// ��]�B
    /// </summary>
    public Vector3 rotation;

    /// <summary>
    /// ���݂�HP�B
    /// </summary>
    public float currentHP;
}

/// <summary>
/// �΂̃f�[�^���Ǘ�����N���X�B
/// </summary>
[System.Serializable]
public class StoneData
{
    /// <summary>
    /// ���O�B
    /// </summary>
    public string name;

    /// <summary>
    /// �ʒu�B
    /// </summary>
    public Vector3 position;

    /// <summary>
    /// ��]�B
    /// </summary>
    public Vector3 rotation;

    /// <summary>
    /// ���݂�HP�B
    /// </summary>
    public float currentHP;
}

/// <summary>
/// �������̃f�[�^���Ǘ�����N���X�B
/// </summary>
[System.Serializable]
public class ConstructionData
{
    /// <summary>
    /// ���O�B
    /// </summary>
    public string name;

    /// <summary>
    /// �ʒu�B
    /// </summary>
    public Vector3 position;

    /// <summary>
    /// ��]�B
    /// </summary>
    public Vector3 rotation;

    /// <summary>
    /// ���݂�HP�B
    /// </summary>
    public float currentHP;
}

/// <summary>
/// ���[�f�[�^���Ǘ�����N���X�B
/// </summary>
[System.Serializable]
public class StorageData
{
    /// <summary>
    /// �A�C�e�����̃��X�g�B
    /// </summary>
    public List<string> itemsname;

    /// <summary>
    /// �ʒu�B
    /// </summary>
    public Vector3 position;

    /// <summary>
    /// ��]�B
    /// </summary>
    public Vector3 rotation;
}
