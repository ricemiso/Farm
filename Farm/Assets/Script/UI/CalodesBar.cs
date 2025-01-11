using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �J�����[�o�[���Ǘ�����N���X�B
/// </summary>
public class CalodesBar : MonoBehaviour
{
    /// <summary>
    /// �X���C�_�[�R���|�[�l���g�B
    /// </summary>
    private Slider Slider;

    /// <summary>
    /// �J�����[�J�E���^�[�̃e�L�X�g�R���|�[�l���g�B
    /// </summary>
    public Text caloriesCounter;

    /// <summary>
    /// �v���C���[�̏�Ԃ������Q�[���I�u�W�F�N�g�B
    /// </summary>
    public GameObject playerState;

    /// <summary>
    /// ���݂̃J�����[�B
    /// </summary>
    private float currentcalories;

    /// <summary>
    /// �ő�J�����[�B
    /// </summary>
    private float maxcalories;

    /// <summary>
    /// �ʏ�̑��x�B
    /// </summary>
    float normalSpeedRate = 1.0f;

    /// <summary>
    /// ��̉��������̑��x�B
    /// </summary>
    public float weakSpeedRate;

    /// <summary>
    /// �����ݒ���s���܂��B
    /// </summary>
    void Awake()
    {
        Slider = GetComponent<Slider>();
    }

    /// <summary>
    /// �J�����[���X�V��������
    /// </summary>
    void Update()
    {
        currentcalories = playerState.GetComponent<PlayerState>().currentCalories;
        maxcalories = playerState.GetComponent<PlayerState>().maxCalories;

        float fillValue = currentcalories / maxcalories;
        Slider.value = fillValue;

        if (Slider.value >= 0.3f)
        {
            PlayerState.Instance.setPlayerSpeedRate(normalSpeedRate);
        }
        else
        {
            PlayerState.Instance.setPlayerSpeedRate(weakSpeedRate);
        }

        // �f�o�b�O�p
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerState.Instance.currentCalories = 1;
        }

        if (Slider.value <= 0)
        {
            currentcalories = 0;
        }

        caloriesCounter.text = $"{currentcalories}/{maxcalories}";
    }
}
