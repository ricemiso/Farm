using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �w���X�o�[���Ǘ�����N���X�B
/// </summary>
public class HelthBar : MonoBehaviour
{
    /// <summary>
    /// �X���C�_�[�R���|�[�l���g�B
    /// </summary>
    private Slider Slider;

    /// <summary>
    /// �w���X�J�E���^�[�̃e�L�X�g�R���|�[�l���g�B
    /// </summary>
    public Text healthCounter;

    /// <summary>
    /// �v���C���[�̏�Ԃ������Q�[���I�u�W�F�N�g�B
    /// </summary>
    public GameObject playerState;

    /// <summary>
    /// ���݂̗̑́B
    /// </summary>
    private float currentHealth;

    /// <summary>
    /// �ő�̗́B
    /// </summary>
    private float maxHealth;

    /// <summary>
    /// �����ݒ���s���܂��B
    /// </summary>
    void Awake()
    {
        Slider = GetComponent<Slider>();
    }

    /// <summary>
    /// �̗͂��X�V����
    /// </summary>
    void Update()
    {
        currentHealth = playerState.GetComponent<PlayerState>().currentHealth;
        maxHealth = playerState.GetComponent<PlayerState>().maxHealth;

        float fillValue = currentHealth / maxHealth;
        Slider.value = fillValue;

        healthCounter.text = $"{currentHealth}/{maxHealth}";
    }
}
