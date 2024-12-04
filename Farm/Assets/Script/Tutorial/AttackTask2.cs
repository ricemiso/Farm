using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTask2 : ITutorialTask
{
    public string GetTitle()
    {
        return "Šî–{‘€ì UŒ‚(2/2)";
    }

    public string GetText()
    {
        return "“G‚ğ“|‚µ‚Ä‚İ‚æ‚¤";
    }

    public void OnTaskSetting()
    {
		//TutorialManager.Instantiate(resource);
	}

    public bool CheckTask()
    {
        if (EquipSystem.Instance.IsHoldingWeapon() && Input.GetMouseButtonDown(0)&&SelectionManager.Instance.isDamage)
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
