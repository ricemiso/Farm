using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackInventroyTask : ITutorialTask
{
    public string GetTitle()
    {
        return "基本操作 スタック分割";
    }

    public string GetText()
    {
        return "スタックされたアイテムを左SHIFTキーを押しながらドラッグ&ドロップを行って分割しよう";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (InventorySystem.Instance.isStacked)
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
