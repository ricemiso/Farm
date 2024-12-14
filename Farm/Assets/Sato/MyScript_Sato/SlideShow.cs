using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class SlideShow : MonoBehaviour
{
	public Sprite[] spr;

	public Image img;

	// Start is called before the first frame update
	void Start()
	{
		LoadImage();
	}

	// Update is called once per frame
	void Update()
	{}

	public void LoadImage()
	{
		int rnd = Random.Range(0, spr.Length);
		img.sprite = spr[rnd];
	}
}
