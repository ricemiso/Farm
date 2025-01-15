using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static MainMenuSaveManager;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �ݒ�}�l�[�W���[���Ǘ�����N���X�B
/// </summary>
public class SettingManager : MonoBehaviour
{
    /// <summary>
    /// �ݒ�}�l�[�W���[�̃C���X�^���X�B
    /// </summary>
    public static SettingManager Instance { get; set; }

    /// <summary>
    /// �߂�{�^���B
    /// </summary>
    public Button backBTN;

    /// <summary>
    /// �}�X�^�[�{�����[���̃X���C�_�[�B
    /// </summary>
    public Slider masterSlider;

    /// <summary>
    /// �}�X�^�[�{�����[���̒l��\������I�u�W�F�N�g�B
    /// </summary>
    public GameObject masterValue;

    /// <summary>
    /// ���y�{�����[���̃X���C�_�[�B
    /// </summary>
    public Slider musicSlider;

    /// <summary>
    /// ���y�{�����[���̒l��\������I�u�W�F�N�g�B
    /// </summary>
    public GameObject musicValue;

    /// <summary>
    /// ���ʉ��{�����[���̃X���C�_�[�B
    /// </summary>
    public Slider effectSlider;

    /// <summary>
    /// ���ʉ��{�����[���̒l��\������I�u�W�F�N�g�B
    /// </summary>
    public GameObject effectValue;

    /// <summary>
    /// �}�E�X���x�̒l��\������I�u�W�F�N�g
    /// </summary>
    public GameObject sensivirityValue;

    /// <summary>
    /// ���ʉ��{�����[���̃X���C�_�[�B
    /// </summary>
    public Slider sensiviritySlider;

   

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
    /// �{�^�����N���b�N���ꂽ���̏������s���܂��B
    /// </summary>
    private void Start()
    {
        backBTN.onClick.AddListener(() =>
        {
            MainMenuSaveManager.Instance.SaveVolumeSettings(musicSlider.value, effectSlider.value, masterSlider.value);
            MainMenuSaveManager.Instance.SaveVolumeMouseSettings(sensiviritySlider.value);

            // MainMenuSaveManager.Instance.SaveVolumeSettings(0.3f, 0.3f, 0.3f);
        });

        StartCoroutine(LoadAndApplySetting());
    }

    /// <summary>
    /// �ݒ��ǂݍ��ݓK�p����R���[�`���B
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator LoadAndApplySetting()
    {
        LoadAndSetVolume();
        LoadAndSetMouse();
        yield return new WaitForSeconds(0.1f);
    }

    /// <summary>
    /// ���ʂ�ǂݍ��ݐݒ肵�܂��B
    /// </summary>
    private void LoadAndSetVolume()
    {
        VolumeSettings volumeSettings = MainMenuSaveManager.Instance.LoadVolumeSettings();

        masterSlider.value = volumeSettings.masters;
        musicSlider.value = volumeSettings.musics;
        effectSlider.value = volumeSettings.effects;

        
    }

    /// <summary>
    /// �}�E�X�̊��x�����[�h����
    /// </summary>
    private  void LoadAndSetMouse()
    {
        MouseSettings mouseSetting = MainMenuSaveManager.Instance.LoadMouseSettings();
        sensiviritySlider.value = mouseSetting.mouse;
    }

    /// <summary>
    /// �����ύX���ꂽ�炻�̕ϐ����L������
    /// </summary>
    private void Update()
    {
        masterValue.GetComponent<TextMeshProUGUI>().text = $"{masterSlider.value}";
        musicValue.GetComponent<TextMeshProUGUI>().text = $"{musicSlider.value}";
        effectValue.GetComponent<TextMeshProUGUI>().text = $"{effectSlider.value}";
        sensivirityValue.GetComponent<TextMeshProUGUI>().text = $"{sensiviritySlider.value}";


        // TODO:����ǉ��������͂����ɒǉ�����
        AudioListener.volume = masterSlider.value / 20;

        SoundManager.Instance.startingZoneBGMMusic.volume = musicSlider.value / 20;
        SoundManager.Instance.gameClearBGM.volume = musicSlider.value / 20;
        SoundManager.Instance.gameOverBGM.volume = musicSlider.value / 20;
        SoundManager.Instance.EnemyCreateBGM.volume = musicSlider.value / 20;
        SoundManager.Instance.TutorialBGM.volume = musicSlider.value / 20;

       SoundManager.Instance.dropItemSound.volume = effectSlider.value / 20;
        SoundManager.Instance.craftingSound.volume = effectSlider.value / 20;
        SoundManager.Instance.toolSwingSound.volume = effectSlider.value / 20;
        SoundManager.Instance.chopSound.volume = effectSlider.value / 20;
        SoundManager.Instance.PickUpItemSound.volume = effectSlider.value / 20;
        SoundManager.Instance.grassWalkSound.volume = effectSlider.value / 20;
        SoundManager.Instance.treeFallSound.volume = effectSlider.value / 20;
        SoundManager.Instance.PutSeSound.volume = effectSlider.value / 20;
        SoundManager.Instance.gravelWalkSound.volume = effectSlider.value / 20;
        SoundManager.Instance.foundationWalkSound.volume = effectSlider.value / 20;
        SoundManager.Instance.FarmWalkSound.volume = effectSlider.value / 20;
        SoundManager.Instance.EatSound.volume = effectSlider.value / 20;
        SoundManager.Instance.DamageSound.volume = effectSlider.value / 20;
        SoundManager.Instance.CrystalAttack.volume = effectSlider.value / 20;
        SoundManager.Instance.Crystalbreak.volume = effectSlider.value / 20;
        SoundManager.Instance.Stonebreak.volume = effectSlider.value / 20;
       

        if (PlayerState.Instance != null)
        {
            PlayerState.Instance.mousesensitivity = sensiviritySlider.value/10;
        }
       
    }
}
