using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static MainMenuSaveManager;

//担当者　越浦晃生

/// <summary>
/// 設定マネージャーを管理するクラス。
/// </summary>
public class SettingManager : MonoBehaviour
{
    /// <summary>
    /// 設定マネージャーのインスタンス。
    /// </summary>
    public static SettingManager Instance { get; set; }

    /// <summary>
    /// 戻るボタン。
    /// </summary>
    public Button backBTN;

    /// <summary>
    /// マスターボリュームのスライダー。
    /// </summary>
    public Slider masterSlider;

    /// <summary>
    /// マスターボリュームの値を表示するオブジェクト。
    /// </summary>
    public GameObject masterValue;

    /// <summary>
    /// 音楽ボリュームのスライダー。
    /// </summary>
    public Slider musicSlider;

    /// <summary>
    /// 音楽ボリュームの値を表示するオブジェクト。
    /// </summary>
    public GameObject musicValue;

    /// <summary>
    /// 効果音ボリュームのスライダー。
    /// </summary>
    public Slider effectSlider;

    /// <summary>
    /// 効果音ボリュームの値を表示するオブジェクト。
    /// </summary>
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

    /// <summary>
    /// ボタンがクリックされた時の処理を行います。
    /// </summary>
    private void Start()
    {
        backBTN.onClick.AddListener(() =>
        {
            MainMenuSaveManager.Instance.SaveVolumeSettings(musicSlider.value, effectSlider.value, masterSlider.value);
            // MainMenuSaveManager.Instance.SaveVolumeSettings(0.3f, 0.3f, 0.3f);
        });

        StartCoroutine(LoadAndApplySetting());
    }

    /// <summary>
    /// 設定を読み込み適用するコルーチン。
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator LoadAndApplySetting()
    {
        LoadAndSetVolume();
        yield return new WaitForSeconds(0.1f);
    }

    /// <summary>
    /// 音量を読み込み設定します。
    /// </summary>
    private void LoadAndSetVolume()
    {
        VolumeSettings volumeSettings = MainMenuSaveManager.Instance.LoadVolumeSettings();

        masterSlider.value = volumeSettings.masters;
        musicSlider.value = volumeSettings.musics;
        effectSlider.value = volumeSettings.effects;

        print("Volume Setting Load");
    }

    /// <summary>
    /// 音が変更されたらその変数を記憶する
    /// </summary>
    private void Update()
    {
        masterValue.GetComponent<TextMeshProUGUI>().text = $"{masterSlider.value}";
        musicValue.GetComponent<TextMeshProUGUI>().text = $"{musicSlider.value}";
        effectValue.GetComponent<TextMeshProUGUI>().text = $"{effectSlider.value}";

        // TODO:音を追加した時はここに追加する
        AudioListener.volume = masterSlider.value / 20;

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
