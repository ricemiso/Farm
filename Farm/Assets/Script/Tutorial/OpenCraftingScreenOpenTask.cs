using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCraftingScreenOpenTask : ITutorialTask
{
    public string GetTitle()
    {
        return "基本操作 クラフト(1/2)";
    }

    public string GetText()
    {
        return "Cキーを押してクラフトスクリーンを開こう";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (CraftingSystem.Instance.isOpen)
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
