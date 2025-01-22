using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmTask1 : ITutorialTask
{
    public string GetTitle()
    {
        return "基本操作 農業(1/3)";
    }

    public string GetText()
    {
        return "種をクラフトして、土壌に近づいて\n左クリックで植えられます。";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (GrobalState.Instance.isFarm1)
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
