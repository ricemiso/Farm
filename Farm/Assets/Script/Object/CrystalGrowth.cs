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
	public bool canBeWatch;
	public float CrystalHealth;
	public bool canBeCharge;

	public float caloriesSpendCarge;

	public Canvas loadScreen;

	[SerializeField] float dis = 10f;

	// Start is called before the first frame update
	void Start()
	{
		currentEnergy = 0;

		CrystalHealth = CrystalMaxHealth;

		rotAngle = Vector3.zero;
		canBeWatch = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (PlayerState.Instance.currentHealth <= 0) return;

        //TODO:止める処理を入れる

        if (canBeWatch)
        {
			GrobalState.Instance.resourceHelth = CrystalHealth;
			GrobalState.Instance.resourceMaxHelth = CrystalMaxHealth;
		}

		canBeWatch = false;

        if (MenuManager.Instance.isCrystalMove)
        {
			rotAngle.y += 0.3f;
			transform.eulerAngles = rotAngle;
		}
		


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

      
			


        if (GrobalState.Instance.isTutorialEnd || GrobalState.Instance.isSkip)
        {
			// TODO:中央クリスタルにマナが溜まり切ったらゲームクリア
			if (PlayerState.Instance.currentHydrationPercent >= 100)
			{


				UnityEngine.Cursor.lockState = CursorLockMode.None;
				//Destroy(SoundManager.Instance.gameObject);
				// クリアシーン

				Destroy(gameObject.transform.parent.parent.gameObject);
				Debug.Log(gameObject.transform.parent.parent.gameObject.name);
				SceneManager.LoadScene("GameClear");
			}
        }
        else
        {
			// TODO:中央クリスタルにマナが溜まり切ったらゲームクリア
			if (PlayerState.Instance.currentHydrationPercent >= 5)
			{
				UnityEngine.Cursor.lockState = CursorLockMode.None;
				//Destroy(SoundManager.Instance.gameObject);
				// クリアシーン
				//TODO:チュートリアル終了変数
				GrobalState.Instance.isTutorialEnd = true;
				Destroy(gameObject.transform.parent.parent.gameObject);
				//SceneManager.LoadScene("MainScene");
				MainMenuSaveManager.Instance.StartLoadedGame("MainScene");

			}
        }
	}

	//
	public void GetEnergy(float getEnergy)
	{

		PlayerState.Instance.currentHydrationPercent += getEnergy;
	}

	public void GetHit(float damage)
	{
		Log.Instance.OnFarmAttack(gameObject.name);

		CrystalHealth -= damage;
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
