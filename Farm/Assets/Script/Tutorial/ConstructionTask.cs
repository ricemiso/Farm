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
        return "���n�����~�j�I���𑕔����āA\n���N���b�N�Ŕz�u�ł��܂��B\nQ�L�[�Œ�~,�Ǐ]�̕ύX���\�ł��B";
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
