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
        return "�}�i���N���X�^���Ƀ`���[�W���Đ��������悤�B100%�ɂȂ�ƃQ�[���N���A!";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (SelectionManager.Instance.Chargeing)
        {
            //TODO:�`���[�g���A���I���ϐ�
            GrobalState.Instance.isTutorialEnd = true;
            return true;
        }

        return false;
    }

    public float GetTransitionTime()
    {
        return 2f;
    }

    
}
