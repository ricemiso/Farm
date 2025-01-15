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
        return "•€‚Å–Ø‚ğ”°Ì‚Å‚«‚Ü‚·B";
    }

    public void OnTaskSetting()
    {
        //TutorialManager.Instance.tree.SetActive(true);
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
