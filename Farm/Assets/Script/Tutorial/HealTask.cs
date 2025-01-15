using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTask : ITutorialTask
{
    public string GetTitle()
    {
        return "基本操作 回復";
    }

    public string GetText()
    {
        return "マナを装備してFキーを押すと、マナを消費して回復できます。";
    }

    public void OnTaskSetting()
	{
		PlayerState state = PlayerState.Instance;
        // 体力が100%なら体力を削る
        if(state.maxHealth == state.currentHealth)
        {
            state.AddHealth(-1);

		}
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
