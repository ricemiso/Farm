using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionTask : ITutorialTask
{
    public string GetTitle()
    {
        return "基本操作 ミニオンの配置";
    }

    public string GetText()
    {
        return "収穫したミニオンを装備して左クリックで配置できます。";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (ConstructionManager.Instance.isConstruction)
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
