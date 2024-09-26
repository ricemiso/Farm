using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalodesBar : MonoBehaviour
{
    private Slider Slider;
    public Text caloriesCounter;

    public GameObject playerState;

    private float currentcalories, maxcalories;

    void Awake()
    {
        Slider = GetComponent<Slider>();
    }


    void Update()
    {
        currentcalories = playerState.GetComponent<PlayerState>().currentCalories;
        maxcalories = playerState.GetComponent<PlayerState>().maxCalories;

        float fillValue = currentcalories / maxcalories;

        Slider.value = fillValue;

        caloriesCounter.text = currentcalories + "/" + maxcalories;
    }
}
