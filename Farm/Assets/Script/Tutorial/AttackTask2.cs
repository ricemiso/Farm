using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTask2 : ITutorialTask
{
    public string GetTitle()
    {
        return "��{���� �U��(2/2)";
    }

    public string GetText()
    {
        return "�G��|���Ă݂悤";
    }

    public void OnTaskSetting()
    {
		//TutorialManager.Instantiate(resource);
	}

    public bool CheckTask()
    {
        if (GrobalState.Instance.isDeath)
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
