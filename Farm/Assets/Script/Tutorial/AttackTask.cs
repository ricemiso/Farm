using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTask : ITutorialTask
{
    public string GetTitle()
    {
        return "��{���� �U��(1/2)";
    }

    public string GetText()
    {
        return "����𑕔������܂܁A���N���b�N�ōU���ł��܂�";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (EquipSystem.Instance.IsHoldingWeapon()&& Input.GetMouseButtonDown(0))
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
