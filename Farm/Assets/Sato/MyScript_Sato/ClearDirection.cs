using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearDirection : MonoBehaviour
{
	public static ClearDirection Instance { get; set; }

	public GameObject crystal;
	public GameObject player;

	Vector3 rotAngle;
	public float turnRate;
	public bool isClear = false;


	// Start is called before the first frame update
	void Start()
	{
		rotAngle = Vector3.zero;
		turnRate = 1.0f;
	}

	// Update is called once per frame
	void Update()
	{
		if (isClear)
		{
			player.SetActive(false);
		}
	}

	public void setClear()
	{
		isClear = true;
	}



	public void loadMenu()
	{
		Destroy(gameObject.transform.parent.parent.gameObject);
		Debug.Log(gameObject.transform.parent.parent.gameObject.name);
		//SceneManager.LoadScene("GameClear");
		SceneManager.LoadScene("MainMenu");

	}
}
