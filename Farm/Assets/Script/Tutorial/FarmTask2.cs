using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmTask2 : ITutorialTask
{
    public string GetTitle()
    {
        return "��{���� �_��(2/3)";
    }

    public string GetText()
    {
        return "���A�����y��Ƀ}�i�������Đ����ł��܂��B\n�}�i�𑕔����č��N���b�N�ł������܂��B";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (GrobalState.Instance.isWater)
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

