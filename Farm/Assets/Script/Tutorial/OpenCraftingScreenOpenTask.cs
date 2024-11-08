using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCraftingScreenOpenTask : ITutorialTask
{
    public string GetTitle()
    {
        return "��{���� �N���t�g(1/2)";
    }

    public string GetText()
    {
        return "C�L�[�������ăN���t�g�X�N���[�����J����";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (CraftingSystem.Instance.isOpen)
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
