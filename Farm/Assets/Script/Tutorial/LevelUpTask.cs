using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpTask : ITutorialTask
{
    public string GetTitle()
    {
        return "��{���� �~�j�I���̋���";
    }

    public string GetText()
    {
        return "�}�i��^���ă~�j�I���������ł��܂��B�z�u�����~�j�I���ɋ߂Â��āA���N���b�N�ŋ����ł��܂��B";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (SelectionManager.Instance.leveling)
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
