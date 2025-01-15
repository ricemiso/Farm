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
		return "ƒ~ƒjƒIƒ“‚ÍüˆÍ‚Ì“G‚ğ©“®“I‚ÉUŒ‚‚Å‚«‚Ü‚·B";
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
