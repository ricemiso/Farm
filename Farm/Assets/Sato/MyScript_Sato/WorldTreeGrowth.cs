using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldTreeGrowth : MonoBehaviour
{
	public float timeOut; //¬’·ŠÔŠu(1.0•b = 1.0f)
	public float timeTrigger; //ƒ^ƒCƒ}[
	public float scaleNum;

	// Start is called before the first frame update
	void Start()
	{
		timeTrigger = 0.0f;
		timeOut = 0.01f;
		scaleNum = 1.0f;
	}

	// Update is called once per frame
	void Update()
	{
		//todo ‚Ç‚Á‚¿‚Ìif‚ðãˆÊ‚É‚à‚Á‚Ä‚¢‚­‚©
		if (Time.time > timeTrigger)
		{
			if (scaleNum <= 50)
			{
				scaleNum += 0.0005f;
				transform.localScale = new Vector3(scaleNum * 2, scaleNum, scaleNum * 2);
			}

			timeTrigger = Time.time + timeOut;
		}

	}
}
