using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infoTask : ITutorialTask
{
    public string GetTitle()
    {
        return "��{���� �A�C�e�������̕\��";
    }

    public string GetText()
    {
        return "H�L�[�������Ƒ��������A�C�e��������������";
    }

    public void OnTaskSetting()
    {
       
      
    }

    public bool CheckTask()
    {
        if (GrobalState.Instance.isInfoTask)
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
