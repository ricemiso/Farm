using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmTask3 : ITutorialTask
{
    public string GetTitle()
    {
        return "Šî–{‘€ì ”_‹Æ(3/3)";
    }

    public string GetText()
    {
        return "ˆç‚Äã‚°‚½ì•¨‚ğûŠn‚µ‚æ‚¤B";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (SelectionManager.Instance.Watering)
        {
            return true;
        }

        return false;
    }

    public float GetTransitionTime()
    {
        return 2f;
    }
}
