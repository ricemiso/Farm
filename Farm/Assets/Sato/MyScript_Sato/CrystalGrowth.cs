using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CrystalGrowth : MonoBehaviour
{
	public float maxEnergy;
	public float currentEnergy;
	public float getEnergy;

	public Vector3 rotAngle;

	// Start is called before the first frame update
	void Start()
	{
		currentEnergy = 0;

		rotAngle = Vector3.zero;
	}

	// Update is called once per frame
	void Update()
	{
		rotAngle.y += 0.1f;
		transform.eulerAngles = rotAngle;

		if (Input.GetKeyDown(KeyCode.G))
		{
			if (currentEnergy < maxEnergy)
			{
				GetEnergy(getEnergy);
			}
			Debug.Log("currentEnergy:" + currentEnergy);
		}

		if (Input.GetKeyDown(KeyCode.H))
		{
			Sato_ChoppableTree.instance.GetHit();
		}

		if (currentEnergy >= maxEnergy)
		{
			Debug.Log("GameClear");

			//Todo 実際のクリアシーンを入れる
			//SceneManager.LoadScene("ClearScene");
		}

	}

	//
	void GetEnergy(float getEnergy)
	{
		currentEnergy += getEnergy;
	}
}
