using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//担当者　越浦晃生　
//建築廃止に伴い現在使用していない

/// <summary>
/// 破壊可能なクラフトオブジェクトを管理するクラス。
/// </summary>
public class Choppablecraft : MonoBehaviour
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
    /// 最大ヘルス。
    /// </summary>
    public float MaxHealth;

    /// <summary>
    /// 現在のヘルス。
    /// </summary>
    public float Health;

    /// <summary>
    /// クラフトアイテムの名前。
    /// </summary>
    public string craftName;

    /// <summary>
    /// 判定距離。
    /// </summary>
    [SerializeField] float dis = 50f;

    /// <summary>
    /// 木をチョップする際に消費するカロリー。
    /// </summary>
    public float caloriesSpendChoppingWood;

    /// <summary>
    /// 初期設定を行います。
    /// </summary>
    private void Start()
    {
        Health = MaxHealth;
        caloriesSpendChoppingWood = 20;
    }

    /// <summary>
    /// クラフトアイテムの名前を返します。
    /// </summary>
    /// <returns>クラフトアイテムの名前</returns>
    public string CraftItemName()
    {
        return craftName;
    }

    /// <summary>
    /// 毎フレームの更新処理を行います。
    /// </summary>
    private void Update()
    {
        if (canBeChopped)
        {
            GrobalState.Instance.resourceHelth = Health;
            GrobalState.Instance.resourceMaxHelth = MaxHealth;
        }
    }

    /// <summary>
    /// 他のオブジェクトがコライダーに入った時の処理を行います。
    /// </summary>
    /// <param name="other">コライダーに入った他のオブジェクト</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRange = true;
        }
    }

    /// <summary>
    /// 他のオブジェクトがコライダーから出た時の処理を行います。
    /// </summary>
    /// <param name="other">コライダーから出た他のオブジェクト</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRange = false;
        }
    }

    /// <summary>
    /// アイテムがヒットされた時の処理を行います。
    /// </summary>
    public void GetHit()
    {
        Health -= 1;
        PlayerState.Instance.currentCalories -= caloriesSpendChoppingWood;

        if (Health <= 0)
        {
            Destroy();
        }
    }

    /// <summary>
    /// ダメージを受けた時の処理を行います。
    /// </summary>
    public void GetDamage()
    {
        //Todo EnemyAI_Movementに与えるダメージ量を入れる変数を作成
        //Health -= EnemyAI_Movement.Instantiate.damage;

        if (Health <= 0)
        {
            Destroy();
        }
    }

    /// <summary>
    /// オブジェクトを破壊します。
    /// </summary>
    public void Destroy()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.treeFallSound);

        Destroy(gameObject);
        canBeChopped = false;
        SelectionManager.Instance.selectedCraft = null;
        SelectionManager.Instance.chopHolder.gameObject.SetActive(false);
    }
}
