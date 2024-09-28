using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    private Slider Slider;
    private float currentHealth, maxHealth;

    public GameObject globalState;

    private void Awake()
    {
        Slider = GetComponent<Slider>();
    }

    private void Update()
    {
        currentHealth = globalState.GetComponent<GrobalState>().resourceHelth;
        maxHealth = globalState.GetComponent<GrobalState>().resourceMaxHelth;

        float fillValue = currentHealth / maxHealth;

        Slider.value = fillValue;

        if (Slider.value <= 0)
        {
            currentHealth = 0;
        }
    }

}
