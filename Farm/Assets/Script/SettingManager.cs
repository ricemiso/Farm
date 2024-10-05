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
    }
}
