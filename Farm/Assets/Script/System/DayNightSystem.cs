using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//担当者　越浦晃生

/// <summary>
/// 昼夜のサイクルを管理するシステムのクラス。
/// </summary>
public class DayNightSystem : MonoBehaviour
{
    /// <summary>
    /// DayNightSystemのインスタンス。
    /// </summary>
    public static DayNightSystem Instance { get; set; }

    /// <summary>
    /// ディレクショナルライト。
    /// </summary>
    public Light directionalLight;

    /// <summary>
    /// 1日の長さ（秒）。
    /// </summary>
    public float dayDurationInSecounds = 24.0f;

    /// <summary>
    /// 現在の時間（時）。
    /// </summary>
    public int currentHour;

    /// <summary>
    /// 現在の昼夜の進行度。
    /// </summary>
    public float currentTimeOfDay = 0.35f;

    /// <summary>
    /// 時刻表示用のテキストUI。
    /// </summary>
    public Text timeUI;

    /// <summary>
    /// 時刻に応じたスカイボックスのマッピングリスト。
    /// </summary>
    public List<SkyBoxTimeMapping> timeMappings;

    /// <summary>
    /// スカイボックスのブレンド値。
    /// </summary>
    float blendevalue = 0.0f;

    /// <summary>
    /// 次の日への移行をロックするフラグ。
    /// </summary>
    bool lockNextDayTrigger = false;

    /// <summary>
    /// シングルトンパターンを適用し、インスタンスを初期化します。
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
    /// 時刻の計算と空の変更
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
                Log.Instance.OnCreateEnemy("ミニクリスタル(黄)");
            }
            else if (currentHour >= 10 && currentHour < 11 && TimeManager.Instance.dayInGame == 2)
            {
                Log.Instance.OnCreateEnemy("ミニクリスタル(赤)");
            }
            else if (currentHour >= 10 && currentHour < 11 && TimeManager.Instance.dayInGame == 3)
            {
                Log.Instance.OnCreateEnemy("ミニクリスタル(緑)");
            }
        }

       
    }

    /// <summary>
    /// スカイボックスを更新します。
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
/// 時刻に応じたスカイボックスのマッピングを表すクラス。
/// </summary>
[System.Serializable]
public class SkyBoxTimeMapping
{
    /// <summary>
    /// フェーズの名前。
    /// </summary>
    public string phaseName;

    /// <summary>
    /// 時刻（時）。
    /// </summary>
    public int hour;

    /// <summary>
    /// スカイボックスのマテリアル。
    /// </summary>
    public Material skyboxMaterial;
}
