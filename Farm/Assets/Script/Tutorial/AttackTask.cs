using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTask : ITutorialTask
{
    public string GetTitle()
    {
        return "基本操作 攻撃(1/2)";
    }

    public string GetText()
    {
        return "武器を装備したまま、左クリックで攻撃できます";
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
