using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTask : ITutorialTask
{
    public string GetTitle()
    {
        return "基本操作 装備(武器)";
    }

    public string GetText()
    {
        return "クイックスロットの右上にある数字のキー、マウスのホイールで装備できます";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (EquipSystem.Instance.IsHoldingWeapon())
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
