using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightSystem : MonoBehaviour
{
    public static DayNightSystem Instance { get; set; }

    public Light directionalLight;

    public float dayDurationInSecounds = 24.0f;
    public int currentHour;
    public float currentTimeOfDay = 0.35f;

    public Text timeUI;
    

    public List<SkyBoxTimeMapping> timeMappings;

    float blendevalue = 0.0f;

    bool lockNextDayTrigger = false;

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


    // Update is called once per frame
    void Update()
    {
        currentTimeOfDay += Time.deltaTime / dayDurationInSecounds;
        currentTimeOfDay %= 1;

        currentHour = Mathf.FloorToInt(currentTimeOfDay * 24);

        timeUI.text = $"{currentHour}:00";


        directionalLight.transform.rotation = Quaternion.Euler(new Vector3((currentTimeOfDay * 360) - 90, 170, 0));
        UpdateSkyBox();
    }

    private void UpdateSkyBox()
    {
        Material currentSkybox = null;

        foreach(SkyBoxTimeMapping mapping in timeMappings)
        {
            if(currentHour == mapping.hour)
            {
                currentSkybox = mapping.skyboxMaterial;

                if(currentSkybox.shader != null)
                {
                    if(currentSkybox.shader.name == "Custom/SkyboxTransition")
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


        if(currentHour == 0 && !lockNextDayTrigger)
        {
            TimeManager.Instance.TriggerNextDay();
            lockNextDayTrigger = true;
        }

        if (currentHour != 0) 
        {
            lockNextDayTrigger = false;
        }


        if(currentSkybox != null)
        {
            RenderSettings.skybox = currentSkybox;
        }
    }
}

[System.Serializable]
public class SkyBoxTimeMapping
{
    public string phaseName;
    public int hour;
    public Material skyboxMaterial;
}

