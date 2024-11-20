using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallVisible : MonoBehaviour
{
	public GameObject player;
	public Material wallMat;
	Color meshColor;
	float colorRate = 256;

	public float maxDst = 200.0f;
	public float minDst = 100.0f;
	public float maxAlpha = 100.0f;
	public float minAlpha = 0.0f;


	public float distanceToPlayer;

	// Start is called before the first frame update
	void Start()
	{
		meshColor = this.GetComponent<MeshRenderer>().material.color;
	}

	// Update is called once per frame
	void Update()
	{
		distanceToPlayer = Vector3.Distance(this.transform.position, player.transform.position);

		//adjustColorAlpha();

		Debug.Log(this.gameObject.name + " " + distanceToPlayer);
	}


	void adjustColorAlpha()
	{
		if (distanceToPlayer > maxDst)
		{
			this.GetComponent<MeshRenderer>().material.color = new Color(wallMat.color.r, wallMat.color.g, wallMat.color.b, minAlpha) / colorRate;
		}
		else if(distanceToPlayer > minDst)
		{

			this.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, (-0.67f * distanceToPlayer + 134.0f)* 2.0f) / colorRate;
		}
		else
		{
			this.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, maxAlpha * 2.0f) / colorRate;
		}
	}
}
