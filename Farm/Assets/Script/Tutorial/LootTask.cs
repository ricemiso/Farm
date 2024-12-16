using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTask : ITutorialTask
{
    public string GetTitle()
    {
        return "基本操作 収奪";
    }

    public string GetText()
    {
        return "倒した敵から右クリックでアイテムを奪おう";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (Input.GetMouseButtonDown(0) && GrobalState.Instance.isloot)
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
