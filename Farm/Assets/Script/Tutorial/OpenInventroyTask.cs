using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInventroyTask : ITutorialTask
{
    public string GetTitle()
    {
        return "��{���� �C���x���g��";
    }

    public string GetText()
    {
        return "E�L�[�������ăC���x���g�����J����";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (InventorySystem.Instance.isOpen)
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
