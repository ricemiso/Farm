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
        return "�}�i�𑕔�����F�L�[�������ƁA�}�i������ĉ񕜂ł��܂��B";
    }

    public void OnTaskSetting()
	{
		PlayerState state = PlayerState.Instance;
        // �̗͂�100%�Ȃ�̗͂����
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
