using UnityEngine;

public class MovementTask : ITutorialTask
{
    public string GetTitle()
    {
        return "基本操作 移動";
    }

    public string GetText()
    {
        return "WSADキーで上下左右に移動できます。";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        // プレイヤーの入力（移動方向）を取得
        float axis_v = Input.GetAxis("Vertical");
        float axis_h = Input.GetAxis("Horizontal");

        // 縦または横の移動入力がある場合
        if (0 < axis_v || 0 < axis_h || axis_v < 0 || axis_h < 0)
        {
            return true; // 移動している
        }

        return false; // 移動していない
    }

    public float GetTransitionTime()
    {
        return 2f;
    }
}