using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppableTreeTask : ITutorialTask
{
    public string GetTitle()
    {
        return "��{���� �؂𔰍̂���";
    }

    public string GetText()
    {
        return "���Ŗ؂𔰍̂��Ă݂悤";
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
