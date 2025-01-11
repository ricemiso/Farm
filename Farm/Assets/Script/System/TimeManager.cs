using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �Q�[�����̓������Ǘ�����N���X�B
/// </summary>
public class TimeManager : MonoBehaviour
{
    /// <summary>
    /// TimeManager�̃C���X�^���X�B
    /// </summary>
    public static TimeManager Instance { get; set; }

    /// <summary>
    /// 1�����o�߂����Ƃ��̃C�x���g�B
    /// </summary>
    public UnityEvent oneDayPass = new UnityEvent();

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
    }

    /// <summary>
    /// �Q�[�����̓����B
    /// </summary>
    public int dayInGame = 1;

    /// <summary>
    /// �����\���p�̃e�L�X�gUI�B
    /// </summary>
    public Text dayUI;

    /// <summary>
    /// �����ݒ���s���܂��B
    /// </summary>
    private void Start()
    {
        dayUI.text = $"{dayInGame}����";
    }

    /// <summary>
    /// ���̓����g���K�[���܂��B
    /// </summary>
    public void TriggerNextDay()
    {
        dayInGame += 1;
        dayUI.text = $"{dayInGame}����";

        oneDayPass.Invoke();
    }
}
