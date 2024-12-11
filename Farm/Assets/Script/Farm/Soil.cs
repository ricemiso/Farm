using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �A���𐧌䂷��N���X�B��������n�̊Ǘ����s���B  
/// �y��̏�ԁi�󂩁A�A�����Ă��邩�j��A���̐������Ď����A���[�U�[���A���ɐ��������鏈�����Ǘ�
/// </summary>
public class Soil : MonoBehaviour
{
    /// <summary>
    /// �y�낪�󂩂ǂ����������t���O�B������Ԃł͋�B
    /// </summary>
    public bool isEmpty = true;

    /// <summary>
    /// �v���C���[���y��̋߂��ɂ��邩�ǂ����������t���O�B
    /// </summary>
    public bool playerInRange;

    /// <summary>
    /// ���ݐA�����Ă���A���̖��O�B
    /// </summary>
    public string plantName;

    /// <summary>
    /// ���ݐA�����Ă���A���̃C���X�^���X�B
    /// </summary>
    public Plant currentplant;

    /// <summary>
    /// �y��̃f�t�H���g�̃}�e���A���B
    /// </summary>
    public Material defaltMaterial;

    /// <summary>
    /// ����^����ꂽ�ۂ̓y��̃}�e���A���B
    /// </summary>
    public Material waterMaterial;

    /// <summary>
    /// �v���C���[�Ɠy��̊Ԃ̋����𑪂邽�߂̕ϐ��B
    /// </summary>
    float distance = 0;

    /// <summary>
    /// �v���C���[�̈ʒu�Ɋ�Â��ēy��Ƃ̋������v�Z���A�v���C���[���y��ɋ߂����ǂ����𔻒f����B
    /// </summary>
    private void Update()
    {
        if (PlayerState.Instance.currentHealth <= 0) return;

        // �v���C���[�Ƃ̋����𑪒�
        if (PlayerState.Instance.playerBody != null)
        {
            distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);
        }

        // �v���C���[���y��̋߂��ɂ��邩�𔻒f
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
    /// �v���C���[���I���������y��ɐA����B
    /// </summary>
    internal void PlantSeed()
    {
        // �I�����ꂽ��A�C�e�����擾
        InventoryItem selectedSeed = EquipSystem.Instance.selectedItem.GetComponent<InventoryItem>();
        isEmpty = false; // �y�낪���܂������Ƃ������t���O��ݒ�

        // ��̖��O����u�̎�v���폜���ĐA�������擾
        string onlyPlantName = selectedSeed.thisName.Split(new string[] { "�̎�" }, StringSplitOptions.None)[0];

        plantName = onlyPlantName;

        // �A���𐶐����ēy��ɔz�u
        GameObject instancePlant = Instantiate(Resources.Load($"{onlyPlantName}Plant") as GameObject);

        instancePlant.transform.parent = gameObject.transform;
        Vector3 plantPos = Vector3.zero;
        plantPos.y = 0f;
        instancePlant.transform.localPosition = plantPos;

        // �A���̃C���X�^���X���擾���A�A��������ݒ�
        currentplant = instancePlant.GetComponent<Plant>();
        currentplant.dayOfPlanting = TimeManager.Instance.dayInGame;
    }

    /// <summary>
    /// �y��ɐ���^������Ԃɂ���B
    /// </summary>
    internal void MakeSoilWatered()
    {
        GetComponent<Renderer>().material = waterMaterial; // ����^������Ԃ̃}�e���A���ɕύX
    }

    /// <summary>
    /// �y��ɐ���^���Ă��Ȃ���Ԃɖ߂��B
    /// </summary>
    internal void MakeSoilNotWatered()
    {
        GetComponent<Renderer>().material = defaltMaterial; // �f�t�H���g�̃}�e���A���ɖ߂�
    }
}

