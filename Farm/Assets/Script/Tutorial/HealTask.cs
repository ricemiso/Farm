using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTask : ITutorialTask
{
    public string GetTitle()
    {
        return "基本操作 回復";
    }

    public string GetText()
    {
        return "マナで体力を回復しよう。インベントリのマナを右クリックすると回復できるよ";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (InventorySystem.Instance.isHeal)
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
