using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldTreeGrowth : MonoBehaviour
{
	public float timeOut; //成長間隔(1.0秒 = 1.0f)
	public float timeTrigger; //タイマー
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
		//todo どっちのifを上位にもっていくか
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
