using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeTask : ITutorialTask
{
    public string GetTitle()
    {
        return "��{���� �N���X�^���̐���";
    }

    public string GetText()
    {
        return "�}�i���N���X�^���Ƀ`���[�W���Đ����ł��܂��B\n100%�ɂȂ�ƃQ�[���N���A!\n(�`���[�g���A����5%)";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (SelectionManager.Instance.Chargeing)
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
