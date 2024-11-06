using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMainMenu : MonoBehaviour
{
    
    public void Click()
    {
        SceneManager.LoadScene("MainMenu"); //メインメニューに移行
    }
}
