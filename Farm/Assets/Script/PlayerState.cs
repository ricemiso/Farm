using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    
    public static PlayerState instance { get; set; }

    //Health
    public float currentHealth;
    public float maxHealth;

    //Calories
    public float currentCalories;
    public float maxCalories;

    float distanceTravelled;
    Vector3 lastPosition;

    public GameObject playerBody;

    //Hydration
    public float currentHydrationPercent;
    public float maxHydrationPercent;

    public bool isHydrationActive;


    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentCalories = maxCalories;
        currentHydrationPercent = maxHydrationPercent;

        StartCoroutine(decreaseHydration());
    }

    IEnumerator decreaseHydration()
    {
        while (/*isHydrationActive*/true)
        {
            currentHydrationPercent -= 1;
            yield return new WaitForSeconds(10);
        }
    }


    void Update()
    {

        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        if(distanceTravelled >= 50)
        {
            distanceTravelled = 0;
            currentCalories -= 1;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10;
        }
    }
}
