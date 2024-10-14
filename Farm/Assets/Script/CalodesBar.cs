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

	float normalSpeedRate = 1.0f;
	public float weakSpeedRate;

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

		if (Slider.value >=  0.3f)
        {
			PlayerState.Instance.setPlayerSpeedRate(normalSpeedRate);
		}
        else
        {
			PlayerState.Instance.setPlayerSpeedRate(weakSpeedRate);
		}

        //デバッグ用
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerState.Instance.currentCalories = 1;

		}

		if (Slider.value <= 0)
        {
            currentcalories = 0;
        }

        caloriesCounter.text = currentcalories + "/" + maxcalories;
    }
}
