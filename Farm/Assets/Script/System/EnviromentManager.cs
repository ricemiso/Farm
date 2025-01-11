using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �����Ǘ�����N���X�B
/// </summary>
public class EnviromentManager : MonoBehaviour
{
    /// <summary>
    /// EnviromentManager�̃C���X�^���X�B
    /// </summary>
    public static EnviromentManager Instance { get; private set; }

    /// <summary>
    /// �S�ẴA�C�e���̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    public GameObject allItems;

    /// <summary>
    /// �S�Ă̖؂̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    public GameObject allTrees;

    /// <summary>
    /// �S�Ă̔z�u�A�C�e���̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    public GameObject allPlaceItem;

    /// <summary>
    /// �S�Ă̐΂̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    public GameObject allStones;

    /// <summary>
    /// �N���X�^���̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    public GameObject Crystal;

    /// <summary>
    /// ���[�̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    public GameObject Storage;

    /// <summary>
    /// �V���O���g���p�^�[����K�p���A�C���X�^���X�����������܂��B
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
