using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStartLoadButton : MonoBehaviour
{
    private void Start()
    {
        SoundManager.Instance.StopSound(SoundManager.Instance.gameClearBGM);
        SoundManager.Instance.StopSound(SoundManager.Instance.gameOverBGM);
        SoundManager.Instance.PlaySound(SoundManager.Instance.startingZoneBGMMusic);
    }

    public void MunuBottun(string sceneName)
    {
        MainMenuSaveManager.Instance.StartLoadedGame(sceneName);
    }
}
