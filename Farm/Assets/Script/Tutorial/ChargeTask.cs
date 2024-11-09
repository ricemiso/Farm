using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeTask : ITutorialTask
{
    public string GetTitle()
    {
        return "基本操作 クリスタルの成長";
    }

    public string GetText()
    {
        return "マナをクリスタルにチャージして成長させよう。100%になるとゲームクリア!";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (SelectionManager.Instance.Chargeing)
        {
            //TODO:チュートリアル終了変数
            GrobalState.Instance.isTutorialEnd = true;
            return true;
        }

        return false;
    }

    public float GetTransitionTime()
    {
        return 2f;
    }

    
}
