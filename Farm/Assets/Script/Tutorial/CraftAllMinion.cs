using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftAllMinion : ITutorialTask
{
    public string GetTitle()
    {
        return "��{���� �N���t�g";
    }

    public string GetText()
    {
        return "�N���t�g��������ɓ���悤";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (CraftingSystem.Instance.isMinionCraft || CraftingSystem.Instance.isTankMinionCraft
            || CraftingSystem.Instance.isMagicMinionCraft)
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
