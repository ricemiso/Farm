using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatEnemy : ITutorialTask
{
	public string GetTitle()
	{
		return "�|��";
	}

	public string GetText()
	{
		return "�G��|����";
	}

	public void OnTaskSetting()
	{
	}

	public bool CheckTask()
	{
		if (SelectionManager.Instance.isDamage)
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
