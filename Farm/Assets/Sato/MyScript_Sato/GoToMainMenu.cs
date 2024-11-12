using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMainMenu : MonoBehaviour
{


	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			ShowCursor();
			SceneManager.LoadScene("MainMenu");
		}
	}

	public void Click()
    {
		ShowCursor();

		SceneManager.LoadScene("MainMenu"); //メインメニューに移行
    }

	private void ShowCursor()
	{
		UnityEngine.Cursor.visible = true;
		UnityEngine.Cursor.lockState = CursorLockMode.None; // カーソルをロック解除
	}
}
