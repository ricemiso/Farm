using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaCraftTask : ITutorialTask
{
    public string GetTitle()
    {
        return "��{���� �}�i�֕ϊ�";
    }

    public string GetText()
    {
        return "�N���t�g��ʂ���ۑ���΂����\n�}�i�ɕϊ��ł��܂��B";
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
