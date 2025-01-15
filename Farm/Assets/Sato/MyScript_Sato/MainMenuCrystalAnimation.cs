using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class MainMenuCrystalAnimation : MonoBehaviour
{
	public Vector3 rotAngle;
	// Start is called before the first frame update
	void Start()
    {
		rotAngle = Vector3.zero;
	}

	// Update is called once per frame
	void Update()
    {
		rotAngle.y += 0.3f;
		transform.eulerAngles = rotAngle;
	}
}
