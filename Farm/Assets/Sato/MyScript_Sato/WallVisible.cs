using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WallVisible : MonoBehaviour
{
	public GameObject player;
	public Material wallMat;
	Color meshColor;
	float colorRate = 256;

	public float maxDst = 130.0f;
	public float minDst = 30.0f;
	public float maxAlpha = 100.0f;
	public float minAlpha = 0.0f;


	//public float distanceToPlayerX;
	//public float distanceToPlayerZ;
	public bool isCheckXDst;

	// Start is called before the first frame update
	void Start()
	{
		meshColor = this.GetComponent<MeshRenderer>().material.color;
	}

	// Update is called once per frame
	void Update()
	{
		//distanceToPlayer = Vector3.Distance(this.transform.position, player.transform.position);
		//distanceToPlayerX = Math.Abs(player.transform.position.x - this.transform.position.x);
		//distanceToPlayerZ = Math.Abs(player.transform.position.z - this.transform.position.z);

		if(isCheckXDst)
		{
			adjustColorAlpha(Math.Abs(player.transform.position.x - this.transform.position.x));
		}
		else
		{
			adjustColorAlpha(Math.Abs(player.transform.position.z - this.transform.position.z));
		}

		

		//Debug.Log(this.gameObject.name + " " + distanceToPlayer);
	}


	void adjustColorAlpha(float distanceToPlayer)
	{
		Debug.Log(this.gameObject.name + " " + distanceToPlayer);
		if (distanceToPlayer > maxDst)
		{
			this.GetComponent<MeshRenderer>().material.color = new Color(wallMat.color.r, wallMat.color.g, wallMat.color.b, minAlpha) / colorRate;
			Debug.Log(this.gameObject.name + " " + this.GetComponent<MeshRenderer>().material.color.a);
		}
		else if(distanceToPlayer > minDst)
		{

			this.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, (-1.0f * distanceToPlayer + 130.0f)) / colorRate;
		}
		else
		{
			this.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, maxAlpha) / colorRate;
		}
	}
}
