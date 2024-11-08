using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftAllMinion : ITutorialTask
{
    public string GetTitle()
    {
        return "��{���� �N���t�g(2/2)";
    }

    public string GetText()
    {
        return "�f�ނ�����Ă���Ώo�Ă���N���t�g�{�^���������Ď����ɓ���悤";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (CraftingSystem.Instance.isMinionCraft||CraftingSystem.Instance.isTankMinionCraft
            ||CraftingSystem.Instance.isMagicMinionCraft)
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
