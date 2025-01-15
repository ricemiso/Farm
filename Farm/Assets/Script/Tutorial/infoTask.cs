using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infoTask : ITutorialTask
{
    public string GetTitle()
    {
        return "基本操作 アイテム説明の表示";
    }

    public string GetText()
    {
        return "Hキーを押すと装備したアイテム説明を見れるよ";
    }

    public void OnTaskSetting()
    {
       
      
    }

    public bool CheckTask()
    {
        if (GrobalState.Instance.isInfoTask)
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
