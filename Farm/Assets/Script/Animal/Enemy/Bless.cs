using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//担当者　越浦晃生

/// <summary>
/// ドラゴンのブレスを管理するプログラム
/// </summary>
public class Bless : MonoBehaviour
{

    LongRange Long;
    private bool isdamege;

    private void Start()
    {
        Long = GetComponentInParent<LongRange>();
        isdamege = false;
    }


    /// <summary>
    /// ブレスが接触したものが、ミニオン、クリスタルだった場合ダメージを与える
    /// </summary>
    /// <param name="other">接触した敵</param>
    private void OnTriggerStay(Collider other)
    {
        float damage = GetComponentInParent<Animal>().damage;
        if ((other.CompareTag("SupportUnit") || other.CompareTag("Player") || other.CompareTag("Crystal") || other.CompareTag("MiniCrystal"))&&!isdamege)
        {
            if (other.gameObject.GetComponent<Animal>())
            {
                if (other.gameObject.GetComponent<Animal>().isDead == false)
                {
                   
                    Long.Attack(damage,other.gameObject);

                }
            }
            else
            {
                Long.Attack(damage, other.gameObject);

            }

            StartCoroutine(attackDelay());
        }


    }

    /// <summary>
    /// 連続ヒットさせないために遅延を入れてフラグを立てる
    /// </summary>
    /// <returns></returns>
    IEnumerator attackDelay()
    {
        isdamege = true;
        yield return new WaitForSeconds(0.5f);
        isdamege = false;
    }
}
