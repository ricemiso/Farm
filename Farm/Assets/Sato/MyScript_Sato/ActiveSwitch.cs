using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSwitch : MonoBehaviour
{
    public GameObject activeText; //アクティブ中に表示するテキスト
    public GameObject inactiveText; //非アクティブ中に表示するテキスト

	// Start is called before the first frame update
	void Start()
    {
		activeText.gameObject.SetActive(true);
		inactiveText.gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update()
    {
		//Zキーを押すとアクティブと非アクティブ時の文章を切り替える
		if (Input.GetKeyDown(KeyCode.Z))
        {
            if (activeText.active == true)
            {
                activeText.gameObject.SetActive(false);
                inactiveText.gameObject.SetActive(true);
            }
            else
            {
                activeText.gameObject.SetActive(true);
                inactiveText.gameObject.SetActive(false);
            }

        }
    }
}

