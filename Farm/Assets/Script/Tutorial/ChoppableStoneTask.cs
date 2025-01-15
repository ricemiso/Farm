using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppableStoneTask : ITutorialTask
{
    public string GetTitle()
    {
        return "Šî–{‘€ì Î‚ğÌŒ@‚·‚é";
    }

    public string GetText()
    {
        return "‚Â‚é‚Í‚µ‚ÅÎ‚ğÌŒ@‚Å‚«‚Ü‚·B";
    }

    public void OnTaskSetting()
    {
        //TutorialManager.Instance.stone.SetActive(true);
    }

    public bool CheckTask()
    {
        if (GrobalState.Instance.isStoneChopped)
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
