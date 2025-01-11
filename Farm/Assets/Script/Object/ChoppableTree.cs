using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//担当者　越浦晃生

/// <summary>
/// 破壊可能な木オブジェクトを管理するクラス。
/// </summary>
/// 

//コライダーをセットする
[RequireComponent(typeof(BoxCollider))]
public class ChoppableTree : MonoBehaviour
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
    /// 木の最大ヘルス。
    /// </summary>
    public float treeMaxHealth;

    /// <summary>
    /// 木の現在のヘルス。
    /// </summary>
    public float treeHealth;

    /// <summary>
    /// アニメーターコンポーネント。
    /// </summary>
    public Animator Animator;

    /// <summary>
    /// 木をチョップする際に消費するカロリー。
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
        if (treeHealth == 0)
        {
            treeHealth = treeMaxHealth;
        }

        caloriesSpendChoppingWood = 20;
        Animator = transform.parent.transform.parent.GetComponent<Animator>();
    }

    /// <summary>
    /// 毎フレームの更新処理を行います。
    /// </summary>
    private void Update()
    {
        if (canBeChopped)
        {
            GrobalState.Instance.resourceHelth = treeHealth;
            GrobalState.Instance.resourceMaxHelth = treeMaxHealth;
        }
        if (PlayerState.Instance.playerBody == null) return;
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
        Animator.SetTrigger("shake");

        treeHealth -= 1;

        PlayerState.Instance.currentCalories -= caloriesSpendChoppingWood;

        if (treeHealth <= 0)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.treeFallSound);
            TreeIsDead();
        }
    }

    /// <summary>
    /// 木が破壊された時の処理を行います。
    /// </summary>
    void TreeIsDead()
    {
        Vector3 treePosition = transform.position;
        GrobalState.Instance.isTreeChopped = true;
        Destroy(transform.parent.transform.parent.gameObject);
        canBeChopped = false;
        SelectionManager.Instance.selectedTree = null;
        SelectionManager.Instance.chopHolder.gameObject.SetActive(false);

        GameObject brokenTree = Instantiate(Resources.Load<GameObject>("ChoppedTree"), new Vector3(treePosition.x, treePosition.y + 1, treePosition.z), Quaternion.Euler(0, 0, 0));

        brokenTree.transform.SetParent(transform.parent.transform.parent.transform.parent);
        Destroy(brokenTree, 60f);
    }
}
