using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInventroyTask : ITutorialTask
{
    public string GetTitle()
    {
        return "基本操作 クラフト画面";
    }

    public string GetText()
    {
        return "Eキーを押してクラフト画面を開けます。";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (InventorySystem.Instance.isOpen)
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
