using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatEnemy : ITutorialTask
{
	public string GetTitle()
	{
		return "�G��|��";
	}

	public string GetText()
	{
		return "�~�j�I���œG���U�����悤";
	}

	public void OnTaskSetting()
	{
	}

	public bool CheckTask()
	{
		if (GrobalState.Instance.isDamage)
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
