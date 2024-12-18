using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{

	public static PlayerState Instance { get; set; }

	[SerializeField] Canvas falseCanvas;
	[SerializeField] Canvas gameOverCanvas;

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

	public GameObject bloodPannl;

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

		gameOverCanvas.gameObject.SetActive(false);

		gameOverCanvas.sortingOrder = -2;
	

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
			falseCanvas.gameObject.SetActive(false);
			gameOverCanvas.gameObject.SetActive(true);
			gameOverCanvas.sortingOrder = 1;
			UnityEngine.Cursor.lockState = CursorLockMode.None;
			UnityEngine.Cursor.visible = true;
			playerBody.GetComponent<PlayerMovement>().enabled = false;
			playerBody.GetComponent<MouseMovement>().enabled = false;

			SoundManager.Instance.StopSound(SoundManager.Instance.startingZoneBGMMusic);
			SoundManager.Instance.StopWalkSound();
			SoundManager.Instance.PlaySound(SoundManager.Instance.gameOverBGM);

		}

		StartCoroutine(delayPanel());
		
	}

	IEnumerator delayPanel()
    {
		yield return new WaitForSeconds(0.5f);
		bloodPannl.SetActive(false);


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
