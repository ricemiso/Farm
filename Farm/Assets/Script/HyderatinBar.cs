using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HyderatinBar : MonoBehaviour
{
    private Slider Slider;
    public Text hydrationCounter;

    public GameObject playerState;

    private float currenthydration, maxhydration;

    void Awake()
    {
        Slider = GetComponent<Slider>();
    }


    void Update()
    {
        currenthydration = playerState.GetComponent<PlayerState>().currentHydrationPercent;
        maxhydration = playerState.GetComponent<PlayerState>().maxHydrationPercent;

        float fillValue = currenthydration / maxhydration;

        Slider.value = fillValue;

        hydrationCounter.text = currenthydration + "%" ;
    }
}
