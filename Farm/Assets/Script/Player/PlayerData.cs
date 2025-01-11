using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �v���C���[�f�[�^���Ǘ�����N���X�B
/// </summary>
[System.Serializable]
public class PlayerData
{
    /// <summary>
    /// �v���C���[�̃X�e�[�^�X���i�[����z��B
    /// </summary>
    public float[] playerStats;

    /// <summary>
    /// �v���C���[�̈ʒu�Ɖ�]���i�[����z��B
    /// </summary>
    public float[] playerPositionAndRotation;

    /// <summary>
    /// �C���x���g���̓��e���i�[����z��B
    /// </summary>
    public string[] inventortContent;

    /// <summary>
    /// �N�C�b�N�X���b�g�̓��e���i�[����z��B
    /// </summary>
    public string[] quickSlotContent;

    /// <summary>
    /// PlayerData�N���X�̃R���X�g���N�^�B
    /// </summary>
    /// <param name="PlayerState">�v���C���[�̃X�e�[�^�X�z��B</param>
    /// <param name="PlayerPosAndRot">�v���C���[�̈ʒu�Ɖ�]�z��B</param>
    /// <param name="InventortContent">�C���x���g���̓��e�z��B</param>
    /// <param name="QuickSlotContent">�N�C�b�N�X���b�g�̓��e�z��B</param>
    public PlayerData(float[] PlayerState, float[] PlayerPosAndRot, string[] InventortContent, string[] QuickSlotContent)
    {
        playerStats = PlayerState;
        playerPositionAndRotation = PlayerPosAndRot;
        inventortContent = InventortContent;
        quickSlotContent = QuickSlotContent;
    }
}
