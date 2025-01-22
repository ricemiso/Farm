using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaCraftTask : ITutorialTask
{
    public string GetTitle()
    {
        return "基本操作 マナへ変換";
    }

    public string GetText()
    {
        return "クラフト画面から丸太や石ころを\nマナに変換できます。";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (GrobalState.Instance.isManaCraft)
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
