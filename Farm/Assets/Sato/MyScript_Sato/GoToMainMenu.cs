using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMainMenu : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			SceneManager.LoadScene("MainMenu");
		}
	}

	public void Click()
    {
        SceneManager.LoadScene("MainMenu"); //メインメニューに移行
    }
}
