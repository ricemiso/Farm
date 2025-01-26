using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//担当者　越浦晃生

/// <summary>
/// ドラゴンのブレスを管理するプログラム
/// </summary>
public class Bless : MonoBehaviour
{

    /// <summary>
    /// 範囲内のターゲットを追跡
    /// </summary>
    private HashSet<Collider> targetsInRange = new HashSet<Collider>();

    /// <summary>
    /// ターゲットごとのクールダウン
    /// </summary>
    private Dictionary<Collider, bool> targetCooldowns = new Dictionary<Collider, bool>();

    /// <summary>
    /// ダメージ間隔
    /// </summary>
    public float damageInterval = 0.5f;

    private void Update()
    {
        // 範囲内のすべてのターゲットにダメージを与える
        foreach (var target in targetsInRange)
        {
            if (targetCooldowns.TryGetValue(target, out bool isOnCooldown) && !isOnCooldown)
            {
                ApplyDamage(target);
                StartCoroutine(SetCooldown(target));
            }
        }
    }

    /// <summary>
    /// ブレスが接触した対象を追跡リストに追加
    /// </summary>
    /// <param name="other">接触した敵</param>
    private void OnTriggerEnter(Collider other)
    {
        if (IsValidTarget(other))
        {
            targetsInRange.Add(other);
            if (!targetCooldowns.ContainsKey(other))
            {
                targetCooldowns[other] = false; // 初期状態でクールダウンはオフ
            }
        }
    }

    /// <summary>
    /// 範囲から出た対象を追跡リストから削除
    /// </summary>
    /// <param name="other">離脱した敵</param>
    private void OnTriggerExit(Collider other)
    {
        if (targetsInRange.Contains(other))
        {
            targetsInRange.Remove(other);
            targetCooldowns.Remove(other);
        }
    }

    /// <summary>
    /// 有効なターゲットかどうかを判定
    /// </summary>
    /// <param name="other">ターゲット</param>
    /// <returns>有効かどうか</returns>
    private bool IsValidTarget(Collider other)
    {
        return other.CompareTag("SupportUnit") ||
               other.CompareTag("Player") ||
               other.CompareTag("Crystal") ||
               other.CompareTag("MiniCrystal");
    }

    /// <summary>
    /// ターゲットにダメージを与える処理
    /// </summary>
    /// <param name="other">ターゲット</param>
    private void ApplyDamage(Collider other)
    {
        float damage = GetComponentInParent<Animal>().damage;

        if (other.TryGetComponent<Animal>(out var animal))
        {
            if (!animal.isDead && animal.animalName != "ドラゴン")
            {
                animal.TakeDamage(damage);
            }
        }
        else if (other.TryGetComponent<CrystalGrowth>(out var crystal))
        {
            crystal.GetHit(damage);
        }
        else if (other.TryGetComponent<MiniCrystal>(out var minicrystal))
        {
            minicrystal.GetHit(damage);
        }
        else if (other.GetComponent<MouseMovement>())
        {
            PlayerState.Instance.AddHealth(-damage);
        }
    }

    /// <summary>
    /// ターゲットにクールダウンを設定
    /// </summary>
    /// <param name="target">ターゲット</param>
    private IEnumerator SetCooldown(Collider target)
    {
        targetCooldowns[target] = true;
        yield return new WaitForSeconds(damageInterval);
        targetCooldowns[target] = false; 
    }
}