using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatEnemy : ITutorialTask
{
	public string GetTitle()
	{
		return "“G‚ğ“|‚¹";
	}

	public string GetText()
	{
		return "ƒ~ƒjƒIƒ“‚Å“G‚ğUŒ‚‚µ‚æ‚¤";
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
