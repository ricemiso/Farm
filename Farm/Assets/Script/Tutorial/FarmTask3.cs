using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmTask3 : ITutorialTask
{
    public string GetTitle()
    {
        return "��{���� �_��(3/3)";
    }

    public string GetText()
    {
        return "��ďグ���앨���|�C���g���āA\n���N���b�N�Ŏ��n�ł��܂��B";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (InventorySystem.Instance.isMinonget)
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
