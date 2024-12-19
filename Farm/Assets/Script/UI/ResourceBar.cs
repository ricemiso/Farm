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

    private void Start()
    {
        if (globalState == null)
        {
            globalState = GameObject.Find("GrobalState");
        }
    }

    private void Update()
    {
        if (gameObject != null && globalState.TryGetComponent<GrobalState>(out var grobalState))
        {
            currentHealth = grobalState.resourceHelth;
            maxHealth = grobalState.resourceMaxHelth;
        }


        float fillValue = currentHealth / maxHealth;

        Slider.value = fillValue;

        if (Slider.value <= 0)
        {
            currentHealth = 0;
        }
    }

}
