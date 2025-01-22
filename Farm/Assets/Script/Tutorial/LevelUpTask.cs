using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpTask : ITutorialTask
{
    public string GetTitle()
    {
        return "基本操作 ミニオンの強化";
    }

    public string GetText()
    {
        return "マナを与えてミニオンを強化できます。\n配置したミニオンに近づいて、\n左クリックで強化できます。";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (SelectionManager.Instance.leveling)
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
