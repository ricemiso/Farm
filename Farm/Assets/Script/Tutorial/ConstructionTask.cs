using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionTask : ITutorialTask
{
    public string GetTitle()
    {
        return "��{���� �~�j�I���̔z�u";
    }

    public string GetText()
    {
        return "���n�����~�j�I�����C���x���g������E�N���b�N���Ĕz�u���Ă݂悤�BX�L�[�������ƃL�����Z���ł����";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (ConstructionManager.Instance.isConstruction)
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
