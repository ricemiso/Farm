using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MiniCrystal : MonoBehaviour
{
	// リスト
	[SerializeField] List<GameObject> soils = new List<GameObject>();

	public float maxEnergy;
	public float currentEnergy;

	public Vector3 rotAngle;

	public bool playerRange;
	public float CrystalMaxHealth;
	public bool canBeWatchs;
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

		canBeWatchs = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (PlayerState.Instance.playerBody == null) return;

        //TODO:止める処理を入れる

        if (MenuManager.Instance.isCrystalMove)
        {
			rotAngle.y += 0.3f;
			transform.eulerAngles = rotAngle;
		}
		
        if (canBeWatchs)
        {
			GrobalState.Instance.resourceHelth = CrystalHealth;
			GrobalState.Instance.resourceMaxHelth = CrystalMaxHealth;
		}

		canBeWatchs = false;

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

	//
	public void GetEnergy(float getEnergy)
	{
		PlayerState.Instance.currentHydrationPercent += getEnergy;
	}

	public void GetHit(float damage)
	{



		Color color = Log.Instance.ColorChange(gameObject);
		Log.Instance.OnFarmAttack(gameObject.name, color);
		

		CrystalHealth -= damage;
		GrobalState.Instance.resourceHelth = CrystalHealth;
		GrobalState.Instance.resourceMaxHelth = CrystalMaxHealth;

		PlayerState.Instance.currentCalories -= caloriesSpendCarge;

		if (CrystalHealth <= 0 || PlayerState.Instance.currentHealth <= 0)
		{

			Color deathcolor = Log.Instance.ColorChange(gameObject);
			Log.Instance.OnFarmDeath(gameObject.name, deathcolor);

            foreach(GameObject soil in soils)
            {
				// 破壊処理
				Destroy(soil.gameObject);
				Destroy(gameObject);

            }
		}

	}

	

	// ミニクリスタルが生きているか
	public bool IsAlive()
	{
		if (CrystalHealth <= 0) return false;
		else return true;
	}

}
