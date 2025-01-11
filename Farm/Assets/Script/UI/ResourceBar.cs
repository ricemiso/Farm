using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�S���ҁ@�z�Y�W��
    
/// <summary>
/// ���\�[�X�o�[���Ǘ�����N���X�B
/// </summary>
public class ResourceBar : MonoBehaviour
{
    /// <summary>
    /// �X���C�_�[�R���|�[�l���g�B
    /// </summary>
    private Slider Slider;

    /// <summary>
    /// ���݂̃w���X�B
    /// </summary>
    private float currentHealth;

    /// <summary>
    /// �ő�w���X�B
    /// </summary>
    private float maxHealth;

    /// <summary>
    /// �O���[�o���X�e�[�g�������Q�[���I�u�W�F�N�g�B
    /// </summary>
    public GameObject globalState;

    /// <summary>
    /// �����ݒ���s���܂��B
    /// </summary>
    private void Awake()
    {
        Slider = GetComponent<Slider>();
    }

    /// <summary>
    /// �����ݒ���s���܂��B
    /// </summary>
    private void Start()
    {
        if (globalState == null)
        {
            globalState = GameObject.Find("GrobalState");
        }
    }

    /// <summary>
    ///�̗͂��X�V����
    /// </summary>
    private void Update()
    {
        if (gameObject != null && globalState.TryGetComponent<GrobalState>(out var grobalState))
        {
            currentHealth = grobalState.resourceHelth;
            maxHealth = grobalState.resourceMaxHelth;
        }

        float fillValue = currentHealth / maxHealth;
        Slider.value = fillValue;

        if (Slider.value <= 0)
        {
            currentHealth = 0;
        }
    }
}
