using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSwitch : MonoBehaviour
{
    public GameObject activeText; //�A�N�e�B�u���ɕ\������e�L�X�g
    public GameObject inactiveText; //��A�N�e�B�u���ɕ\������e�L�X�g

	// Start is called before the first frame update
	void Start()
    {
		activeText.gameObject.SetActive(true);
		inactiveText.gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update()
    {
		//Z�L�[�������ƃA�N�e�B�u�Ɣ�A�N�e�B�u���̕��͂�؂�ւ���
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

