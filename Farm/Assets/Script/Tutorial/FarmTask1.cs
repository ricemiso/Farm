using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmTask1 : ITutorialTask
{
    public string GetTitle()
    {
        return "��{���� �_��(1/3)";
    }

    public string GetText()
    {
        return "����N���t�g���đ������A�y��ɐA���Ă݂悤�B�y��ɋ߂Â��č��N���b�N�ŐA������";
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
