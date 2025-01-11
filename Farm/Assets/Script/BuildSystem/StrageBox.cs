using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//担当者　越浦晃生

/// <summary>
/// プレイヤーが近づくと開閉する収納ボックスを表すクラス。
/// </summary>
public class StrageBox : MonoBehaviour
{
    /// <summary>
    /// プレイヤーが範囲内にいるかどうかを示すフラグ。
    /// </summary>
    public bool playerInRange;

    /// <summary>
    /// プレイヤーとの判定距離。
    /// </summary>
    [SerializeField] float dis = 10f;

    /// <summary>
    /// 収納ボックス内のアイテムリスト。
    /// </summary>
    [SerializeField] public List<string> items;

    /// <summary>
    /// アニメーションコンポーネント。
    /// </summary>
    public Animation animation;

    /// <summary>
    /// アニメーションのカウントを管理する変数。
    /// </summary>
    private int cnt = 0;

    /// <summary>
    /// アニメーションコンポーネントを取得します。
    /// </summary>
    private void Start()
    {
        animation = GetComponent<Animation>();
    }

    /// <summary>
    /// ボックスのタイプを表す列挙型。
    /// </summary>
    public enum BoxType
    {
        smallBox,
        bigBox
    }

    /// <summary>
    /// このボックスのタイプ。
    /// </summary>
    public BoxType thisboxType;

    /// <summary>
    /// プレイヤーとの距離を測定し、収納UIの開閉に応じてボックスを開閉します。
    /// </summary>
    private void Update()
    {
        float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

        if (distance < dis)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }

        if (StorageManager.Instance.storageUIOpen)
        {
            if (cnt == 0)
            {
                animation.Play("A_SeaChest_Open");
                cnt++;
            }
        }
        else if (!StorageManager.Instance.storageUIOpen)
        {
            if (cnt == 1)
            {
                animation.Play("A_SeaChest_Close");
                cnt = 0;
            }
        }
    }
}
