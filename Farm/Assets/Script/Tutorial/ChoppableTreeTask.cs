using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppableTreeTask : ITutorialTask
{
    public string GetTitle()
    {
        return "Šî–{‘€ì –Ø‚ğ”°Ì‚·‚é";
    }

    public string GetText()
    {
        return "•€‚Å–Ø‚ğ”°Ì‚µ‚Ä‚İ‚æ‚¤";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (GrobalState.Instance.isTreeChopped)
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
