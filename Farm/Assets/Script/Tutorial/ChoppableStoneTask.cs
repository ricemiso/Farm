using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppableStoneTask : ITutorialTask
{
    public string GetTitle()
    {
        return "��{���� �΂��̌@����";
    }

    public string GetText()
    {
        return "��͂��Ő΂��̌@�ł��܂��B";
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
