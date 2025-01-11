using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �}�i�o�[���Ǘ�����N���X�B
/// </summary>
public class HyderatinBar : MonoBehaviour
{
    /// <summary>
    /// �X���C�_�[�R���|�[�l���g�B
    /// </summary>
    private Slider Slider;

    /// <summary>
    /// �}�i�J�E���^�[�̃e�L�X�g�R���|�[�l���g�B
    /// </summary>
    public Text hydrationCounter;

    /// <summary>
    /// �v���C���[�̏�Ԃ������Q�[���I�u�W�F�N�g�B
    /// </summary>
    public GameObject playerState;

    /// <summary>
    /// ���݂̃}�i�B
    /// </summary>
    private float currenthydration;

    /// <summary>
    /// �ő�}�i�B
    /// </summary>
    private float maxhydration;

    /// <summary>
    /// �����ݒ���s���܂��B
    /// </summary>
    void Awake()
    {
        Slider = GetComponent<Slider>();
    }

    /// <summary>
    /// �}�i�ʂ̍X�V
    /// </summary>
    void Update()
    {
        currenthydration = playerState.GetComponent<PlayerState>().currentHydrationPercent;
        maxhydration = playerState.GetComponent<PlayerState>().maxHydrationPercent;

        float fillValue = currenthydration / maxhydration;
        Slider.value = fillValue;

        if (Slider.value <= 0)
        {
            currenthydration = 0;
        }

        hydrationCounter.text = $"{currenthydration}%";
    }
}
