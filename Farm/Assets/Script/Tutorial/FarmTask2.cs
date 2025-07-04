using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmTask2 : ITutorialTask
{
    public string GetTitle()
    {
        return "基本操作 農業(2/3)";
    }

    public string GetText()
    {
        return "種を植えた土壌にマナをあげて成長できます。\nマナを装備して左クリックであげられます。";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (GrobalState.Instance.isWater)
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

