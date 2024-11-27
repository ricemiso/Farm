using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTask : ITutorialTask
{
    public string GetTitle()
    {
        return "��{���� ����(����)";
    }

    public string GetText()
    {
        return "�N�C�b�N�X���b�g�̉E��ɂ��鐔���̃L�[�A�}�E�X�̃z�C�[���ő����ł��܂�";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (EquipSystem.Instance.IsHoldingWeapon())
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
