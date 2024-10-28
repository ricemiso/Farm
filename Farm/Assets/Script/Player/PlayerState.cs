using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{

	public static PlayerState Instance { get; set; }

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

	public Vector3 isPosition;

	public float playerSpeedRate;

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

	private void Start()
	{
		currentHealth = maxHealth;
		currentCalories = maxCalories;
		currentHydrationPercent = 0;

		//StartCoroutine(decreaseHydration());
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

		if (distanceTravelled >= 50)
		{
			distanceTravelled = 0;
			currentCalories -= 1;
		}

		if (Input.GetKeyDown(KeyCode.N))
		{
			currentHealth -= 10;
		}


		if (currentHydrationPercent >= 100)
		{
			Debug.Log("GameClear");
		}

	}

	// ‘Ì—Í‚ð‘Œ¸‚³‚¹‚é
	public void AddHealth(float num)
	{
		currentHealth += num;

		if (currentHealth > maxHealth)
		{
			currentHealth = maxHealth;
		}
		else if (currentHealth < 0)
		{
			currentHealth = 0;
			Debug.Log("GameOver");
		}
	}

	public void setHealth(float newHealth)
	{
		currentHealth = newHealth;
	}

	public void setCalories(float newCalories)
	{
		currentCalories = newCalories;
	}

	public void setHydration(float newHydration)
	{
		currentHydrationPercent = newHydration;
	}

	public void setPlayerPosition(Vector3 newPosition)
	{
		isPosition = newPosition;
	}

	public Vector3 getPlayerPosition()
	{
		return isPosition;
	}

	public void setPlayerSpeedRate(float newPlayerSpeed)
	{
		playerSpeedRate = newPlayerSpeed;
	}

	public float getPlayerSpeedRate()
	{
		return playerSpeedRate;
	}
}
