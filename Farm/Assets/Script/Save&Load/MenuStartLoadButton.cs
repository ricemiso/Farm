using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStartLoadButton : MonoBehaviour
{
   public void MunuBottun(string sceneName)
    {
        MainMenuSaveManager.Instance.StartLoadedGame(sceneName);
    }
}
