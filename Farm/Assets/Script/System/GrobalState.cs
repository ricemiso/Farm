using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�S���ҁ@�z�Y�W��

/// <summary>
/// ��Ԃ��Ǘ�����N���X�B
/// </summary>
public class GrobalState : MonoBehaviour
{
    /// <summary>
    /// GrobalState�̃C���X�^���X�B
    /// </summary>
    public static GrobalState Instance { get; set; }

    /// <summary>
    /// ���\�[�X�̃w���X�B
    /// </summary>
    public float resourceHelth;

    /// <summary>
    /// ���\�[�X�̍ő�w���X�B
    /// </summary>
    public float resourceMaxHelth;

    /// <summary>
    /// ���\�[�X�̃}�i�B
    /// </summary>
    public float resourceMana;

    /// <summary>
    /// ���x���B
    /// </summary>
    public int level;

    /// <summary>
    /// �_���[�W�B
    /// </summary>
    public int damage;

    // �`���[�g���A���p�̕ϐ�
    public bool isTreeChopped = false;
    public bool isStoneChopped = false;
    public bool isTutorialEnd = false;
    public bool isSkip = false;
    public bool isWater = false;
    public bool isDamage = false;
    public bool isLoot = false;
    public bool isFarm1 = false;
    public bool isDeath = false;
    public bool isManaCraft = false;
    public bool isInfoTask = false;

    /// <summary>
    /// �V���O���g���p�^�[����K�p���A�C���X�^���X�����������܂��B
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// �������������s���܂��B
    /// </summary>
    public void Initialize()
    {
        resourceHelth = 0;
        resourceMaxHelth = 0;
        resourceMana = 0;
        level = 0;
    }
}
