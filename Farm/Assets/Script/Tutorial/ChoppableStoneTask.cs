using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppableStoneTask : ITutorialTask
{
    public string GetTitle()
    {
        return "基本操作 石を採掘する";
    }

    public string GetText()
    {
        return "つるはしで石を採掘できます。";
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
