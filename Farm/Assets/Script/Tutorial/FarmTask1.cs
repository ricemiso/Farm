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
        return "����N���t�g���āA�y��ɋ߂Â���\n���N���b�N�ŐA�����܂��B";
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
