using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelthBar : MonoBehaviour
{

    private Slider Slider;
    public Text healthCounter;

    public GameObject playerState;

    private float currentHealth, maxHealth;

    void Awake()
    {
        Slider = GetComponent<Slider>();
    }

    
    void Update()
    {
        currentHealth = playerState.GetComponent<PlayerState>().currentHealth;
        maxHealth = playerState.GetComponent<PlayerState>().maxHealth;

        float fillValue = currentHealth / maxHealth;

        Slider.value = fillValue;

        if (Slider.value <= 0)
        {
            currentHealth = 0;
        }

        healthCounter.text = currentHealth + "/" + maxHealth;
    }
}
