using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftAllMinion : ITutorialTask
{
    public string GetTitle()
    {
        return "基本操作 クラフト";
    }

    public string GetText()
    {
        return "クラフトから種を手に入れよう";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (CraftingSystem.Instance.isMinionCraft || CraftingSystem.Instance.isTankMinionCraft
            || CraftingSystem.Instance.isMagicMinionCraft)
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
