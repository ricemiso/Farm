using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTask : ITutorialTask
{
    public string GetTitle()
    {
        return "��{���� ���D";
    }

    public string GetText()
    {
        return "�|�����G����E�N���b�N�ŃA�C�e����D����";
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
