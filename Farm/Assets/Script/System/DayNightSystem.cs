using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�S���ҁ@�z�Y�W��

/// <summary>
/// ����̃T�C�N�����Ǘ�����V�X�e���̃N���X�B
/// </summary>
public class DayNightSystem : MonoBehaviour
{
    /// <summary>
    /// DayNightSystem�̃C���X�^���X�B
    /// </summary>
    public static DayNightSystem Instance { get; set; }

    /// <summary>
    /// �f�B���N�V���i�����C�g�B
    /// </summary>
    public Light directionalLight;

    /// <summary>
    /// 1���̒����i�b�j�B
    /// </summary>
    public float dayDurationInSecounds = 24.0f;

    /// <summary>
    /// ���݂̎��ԁi���j�B
    /// </summary>
    public int currentHour;

    /// <summary>
    /// ���݂̒���̐i�s�x�B
    /// </summary>
    public float currentTimeOfDay = 0.35f;

    /// <summary>
    /// �����\���p�̃e�L�X�gUI�B
    /// </summary>
    public Text timeUI;

    /// <summary>
    /// �����ɉ������X�J�C�{�b�N�X�̃}�b�s���O���X�g�B
    /// </summary>
    public List<SkyBoxTimeMapping> timeMappings;

    /// <summary>
    /// �X�J�C�{�b�N�X�̃u�����h�l�B
    /// </summary>
    float blendevalue = 0.0f;

    /// <summary>
    /// ���̓��ւ̈ڍs�����b�N����t���O�B
    /// </summary>
    bool lockNextDayTrigger = false;

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
    /// �����̌v�Z�Ƌ�̕ύX
    /// </summary>
    void Update()
    {
        currentTimeOfDay += Time.deltaTime / dayDurationInSecounds;
        currentTimeOfDay %= 1;

        currentHour = Mathf.FloorToInt(currentTimeOfDay * 24);
        timeUI.text = $"{currentHour}:00";

        directionalLight.transform.rotation = Quaternion.Euler(new Vector3((currentTimeOfDay * 360) - 90, 170, 0));
        UpdateSkyBox();


        if (GrobalState.Instance.isTutorialEnd)
        {
            if (currentHour >= 10 && currentHour < 11 && TimeManager.Instance.dayInGame == 1)
            {
                Log.Instance.OnCreateEnemy("�~�j�N���X�^��(��)");
            }
            else if (currentHour >= 10 && currentHour < 11 && TimeManager.Instance.dayInGame == 2)
            {
                Log.Instance.OnCreateEnemy("�~�j�N���X�^��(��)");
            }
            else if (currentHour >= 10 && currentHour < 11 && TimeManager.Instance.dayInGame == 3)
            {
                Log.Instance.OnCreateEnemy("�~�j�N���X�^��(��)");
            }
        }

       
    }

    /// <summary>
    /// �X�J�C�{�b�N�X���X�V���܂��B
    /// </summary>
    private void UpdateSkyBox()
    {
        Material currentSkybox = null;

        foreach (SkyBoxTimeMapping mapping in timeMappings)
        {
            if (currentHour == mapping.hour)
            {
                currentSkybox = mapping.skyboxMaterial;

                if (currentSkybox.shader != null)
                {
                    if (currentSkybox.shader.name == "Custom/SkyboxTransition")
                    {
                        blendevalue += Time.deltaTime;
                        blendevalue = Mathf.Clamp01(blendevalue);
                        currentSkybox.SetFloat("_TransitionFactor", blendevalue);
                    }
                    else
                    {
                        blendevalue = 0;
                    }
                }
                break;
            }
        }

        if (currentHour == 0 && !lockNextDayTrigger)
        {
            TimeManager.Instance.TriggerNextDay();
            lockNextDayTrigger = true;
        }

        if (currentHour != 0)
        {
            lockNextDayTrigger = false;
        }

        if (currentSkybox != null)
        {
            RenderSettings.skybox = currentSkybox;
        }
    }
}

/// <summary>
/// �����ɉ������X�J�C�{�b�N�X�̃}�b�s���O��\���N���X�B
/// </summary>
[System.Serializable]
public class SkyBoxTimeMapping
{
    /// <summary>
    /// �t�F�[�Y�̖��O�B
    /// </summary>
    public string phaseName;

    /// <summary>
    /// �����i���j�B
    /// </summary>
    public int hour;

    /// <summary>
    /// �X�J�C�{�b�N�X�̃}�e���A���B
    /// </summary>
    public Material skyboxMaterial;
}
