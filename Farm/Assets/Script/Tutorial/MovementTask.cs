using UnityEngine;

public class MovementTask : ITutorialTask
{
    public string GetTitle()
    {
        return "��{���� �ړ�";
    }

    public string GetText()
    {
        return "WSAD�L�[�ŏ㉺���E�Ɉړ��ł��܂��B";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        // �v���C���[�̓��́i�ړ������j���擾
        float axis_v = Input.GetAxis("Vertical");
        float axis_h = Input.GetAxis("Horizontal");

        // �c�܂��͉��̈ړ����͂�����ꍇ
        if (0 < axis_v || 0 < axis_h || axis_v < 0 || axis_h < 0)
        {
            return true; // �ړ����Ă���
        }

        return false; // �ړ����Ă��Ȃ�
    }

    public float GetTransitionTime()
    {
        return 2f;
    }
}