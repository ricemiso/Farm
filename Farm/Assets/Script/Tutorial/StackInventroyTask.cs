using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackInventroyTask : ITutorialTask
{
    public string GetTitle()
    {
        return "��{���� �X�^�b�N����";
    }

    public string GetText()
    {
        return "�X�^�b�N���ꂽ�A�C�e������SHIFT�L�[�������Ȃ���h���b�O&�h���b�v���s���ĕ������悤";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (InventorySystem.Instance.isStacked)
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
