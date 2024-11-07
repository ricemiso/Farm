using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class CrystalGrowth : MonoBehaviour
{

	public float maxEnergy;
	public float currentEnergy;

	public Vector3 rotAngle;

	public bool playerRange;
	public float CrystalMaxHealth;
	public bool canBeChopped;
	public float CrystalHealth;
	public bool canBeCharge;

	public float caloriesSpendCarge;

	[SerializeField] float dis = 10f;

	// Start is called before the first frame update
	void Start()
	{
		currentEnergy = 0;

		CrystalHealth = CrystalMaxHealth;

		rotAngle = Vector3.zero;
	}

	// Update is called once per frame
	void Update()
	{
		if (PlayerState.Instance.currentHealth <= 0) return;

		//TODO:止める処理を入れる

		rotAngle.y += 0.3f;
		transform.eulerAngles = rotAngle;


		GrobalState.Instance.resourceHelth = CrystalHealth;
		GrobalState.Instance.resourceMaxHelth = CrystalMaxHealth;


		if (PlayerState.Instance.playerBody != null)
		{
			float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);


			if (distance < dis)
			{
				playerRange = true;
			}
			else
			{
				playerRange = false;
			}

		}
		
		// 中央クリスタルにマナが溜まり切ったらゲームクリア(仮で10)
		if (PlayerState.Instance.currentHydrationPercent >= 30)
		{
			Debug.Log("GameClear");

			UnityEngine.Cursor.lockState = CursorLockMode.None;
			//Destroy(SoundManager.Instance.gameObject);
			// クリアシーン

			SceneManager.LoadScene("GameClear");
		}

	}

	//
	public void GetEnergy(float getEnergy)
	{

		PlayerState.Instance.currentHydrationPercent += getEnergy;
	}

	public void GetHit()
	{
		Log.Instance.OnCrystalAttack();

		CrystalHealth -= 1;
		GrobalState.Instance.resourceHelth = CrystalHealth;
		GrobalState.Instance.resourceMaxHelth = CrystalMaxHealth;

		PlayerState.Instance.currentCalories -= caloriesSpendCarge;

		if (CrystalHealth <= 0)
		{
			SoundManager.Instance.PlaySound(SoundManager.Instance.treeFallSound);
			//TODO:ゲームオーバー処理を作る
			CrystalIsDead();

		}

	}

	public void CrystalIsDead()
    {
		//Destroy(SoundManager.Instance.gameObject);
		UnityEngine.Cursor.lockState = CursorLockMode.None;

		// ゲームオーバーシーンに移動
		SceneManager.LoadScene("GameOver");
	}
}
