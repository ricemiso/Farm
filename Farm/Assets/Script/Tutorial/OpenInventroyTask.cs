using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInventroyTask : ITutorialTask
{
    public string GetTitle()
    {
        return "基本操作 インベントリ";
    }

    public string GetText()
    {
        return "Eキーを押してインベントリを開こう";
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
