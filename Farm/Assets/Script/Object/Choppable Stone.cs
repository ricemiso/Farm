using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//担当者　越浦晃生

/// <summary>
/// 破壊可能な石オブジェクトを管理するクラス。
/// </summary>
/// 

//コライダーをセットする
[RequireComponent(typeof(BoxCollider))]
public class ChoppableStone : MonoBehaviour
{
    /// <summary>
    /// プレイヤーが範囲内にいるかどうかを示すフラグ。
    /// </summary>
    public bool playerRange;

    /// <summary>
    /// チョップ可能かどうかを示すフラグ。
    /// </summary>
    public bool canBeChopped;

    /// <summary>
    /// アニメーションのクールタイムを示すフラグ。
    /// </summary>
    public bool animcooltime;

    /// <summary>
    /// 石の最大ヘルス。
    /// </summary>
    public float stoneMaxHealth;

    /// <summary>
    /// 石の現在のヘルス。
    /// </summary>
    public float stoneHealth;

    // public Animator Animator;

    /// <summary>
    /// 石をチョップする際に消費するカロリー。
    /// </summary>
    public float caloriesSpendChoppingWood;

    /// <summary>
    /// 判定距離。
    /// </summary>
    [SerializeField] float dis = 10f;

    /// <summary>
    /// 初期設定を行います。
    /// </summary>
    private void Start()
    {
        if (stoneHealth == 0)
        {
            stoneHealth = stoneMaxHealth;
        }

        caloriesSpendChoppingWood = 20;
        // Animator = transform.parent.GetComponent<Animator>();
    }

    /// <summary>
    /// 毎フレームの更新処理を行います。
    /// </summary>
    private void Update()
    {
        if (canBeChopped)
        {
            GrobalState.Instance.resourceHelth = stoneHealth;
            GrobalState.Instance.resourceMaxHelth = stoneMaxHealth;
        }

        if (PlayerState.Instance == null) return;
        float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

        if (distance < dis)
        {
            playerRange = true;
        }
        else
        {
            playerRange = false;
        }
    }

    /// <summary>
    /// アイテムがヒットされた時の処理を行います。
    /// </summary>
    public void GetHit()
    {
        // Animator.SetTrigger("shake");

        stoneHealth -= 1;

        PlayerState.Instance.currentCalories -= caloriesSpendChoppingWood;

        if (stoneHealth <= 0)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.Stonebreak);
            StoneIsDead();
        }
    }

    /// <summary>
    /// 石が破壊された時の処理を行います。
    /// </summary>
    void StoneIsDead()
    {
        Vector3 treePosition = transform.position;
        GrobalState.Instance.isStoneChopped = true;
        Destroy(transform.parent.gameObject);
        canBeChopped = false;
        SelectionManager.Instance.selectedStone = null;
        SelectionManager.Instance.chopHolder.gameObject.SetActive(false);

        // オブジェクト1
        GameObject brokenTree1 = Instantiate(Resources.Load<GameObject>("Stone_model"), treePosition, Quaternion.Euler(0, 0, 0));

        // オブジェクト2 (x 座標を少しずらす)
        GameObject brokenTree2 = Instantiate(Resources.Load<GameObject>("Stone_model"), new Vector3(treePosition.x + 1.5f, treePosition.y, treePosition.z), Quaternion.Euler(0, 0, 0));

        // オブジェクト3 (z 座標を少しずらす)
        GameObject brokenTree3 = Instantiate(Resources.Load<GameObject>("Stone_model"), new Vector3(treePosition.x, treePosition.y, treePosition.z + 1.5f), Quaternion.Euler(0, 0, 0));

        Destroy(brokenTree1, 60f);
        Destroy(brokenTree2, 60f);
        Destroy(brokenTree3, 60f);
    }
}
