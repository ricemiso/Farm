using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTask : ITutorialTask
{
    public string GetTitle()
    {
        return "��{���� ��";
    }

    public string GetText()
    {
        return "�}�i�ő̗͂��񕜂��悤�B�C���x���g���̃}�i���E�N���b�N����Ɖ񕜂ł����";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (InventorySystem.Instance.isHeal)
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
