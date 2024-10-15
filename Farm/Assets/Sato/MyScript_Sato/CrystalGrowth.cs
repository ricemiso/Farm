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
		rotAngle.y += 0.3f;
		transform.eulerAngles = rotAngle;

		//�f�o�b�O�p
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
		//------

		if (currentEnergy >= maxEnergy)
		{
			Debug.Log("GameClear");

			//Todo ���ۂ̃N���A�V�[��������
			//SceneManager.LoadScene("ClearScene");
		}

	}

	//
	void GetEnergy(float getEnergy)
	{
		currentEnergy += getEnergy;
	}
}
