using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Button LoadGameBTN;
    

    public void NewGame()
    {
        if (GrobalState.Instance.isTutorialEnd)
        {
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            SceneManager.LoadScene("TutorialScene");
        }
       
    }

    public void ReturnMenu()
    {

        
        SceneManager.LoadScene("MainMenu");
    }



    public void ExitGame()
    {
        Application.Quit();
    }
}
