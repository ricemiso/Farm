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
        return "収穫したミニオンをインベントリから右クリックして配置してみよう。Xキーを押すとキャンセルできるよ";
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
