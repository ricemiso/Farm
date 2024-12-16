using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static MainMenuSaveManager;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance { get; set; }

    public Button backBTN;

    public Slider masterSlider;
    public GameObject masterValue;

    public Slider musicSlider;
    public GameObject musicValue;

    public Slider effectSlider;
    public GameObject effectValue;


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


    private void Start()
    {
        backBTN.onClick.AddListener(() =>
        {
			MainMenuSaveManager.Instance.SaveVolumeSettings(musicSlider.value, effectSlider.value, masterSlider.value);
			//MainMenuSaveManager.Instance.SaveVolumeSettings(0.3f, 0.3f, 0.3f);
		});

        StartCoroutine(LoadAndApplySetting());
    }

    private IEnumerator LoadAndApplySetting()
    {
        LoadAndSetVolume();
        yield return new WaitForSeconds(0.1f);
    }

    private void LoadAndSetVolume()
    {
        VolumeSettings volumeSettings = MainMenuSaveManager.Instance.LoadVolumeSettings();



        masterSlider.value = volumeSettings.masters;
        musicSlider.value = volumeSettings.musics;
        effectSlider.value = volumeSettings.effects;

        


        print("Volume Setting Load");
    }

    private void Update()
    {
        masterValue.GetComponent<TextMeshProUGUI>().text = "" + (masterSlider.value) + "";
        musicValue.GetComponent<TextMeshProUGUI>().text = "" + (musicSlider.value) + "";
        effectValue.GetComponent<TextMeshProUGUI>().text = "" + (effectSlider.value) + "";

        //TODO:‰¹‚ð’Ç‰Á‚µ‚½Žž‚Í‚±‚±‚É’Ç‰Á‚·‚é
        AudioListener.volume = masterSlider.value/20;

        SoundManager.Instance.startingZoneBGMMusic.volume = musicSlider.value / 20;
        SoundManager.Instance.gameClearBGM.volume = musicSlider.value / 20;
        SoundManager.Instance.gameOverBGM.volume = musicSlider.value / 20;
        SoundManager.Instance.EnemyCreateBGM.volume = musicSlider.value / 20;

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
    }
}
