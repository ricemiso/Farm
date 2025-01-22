using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmTask3 : ITutorialTask
{
    public string GetTitle()
    {
        return "基本操作 農業(3/3)";
    }

    public string GetText()
    {
        return "育て上げた作物をポイントして、\n左クリックで収穫できます。";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (InventorySystem.Instance.isMinonget)
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
