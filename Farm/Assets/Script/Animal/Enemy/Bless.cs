using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ドラゴンのブレスを管理するプログラム
/// </summary>
public class Bless : MonoBehaviour
{
    //担当者　越浦晃生

    private LongRange Long;
    private HashSet<Collider> targetsInRange = new HashSet<Collider>(); // 範囲内のオブジェクトを追跡
    public float damageInterval = 0.5f; // ダメージ間隔

    private void Start()
    {
        Long = GetComponentInParent<LongRange>();
    }

    private void Update()
    {
        // 範囲内のすべてのターゲットにダメージを与える
        foreach (var target in targetsInRange)
        {
            ApplyDamage(target);
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

        if (!IsCooldownActive(other))
        {
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

            // クールダウンを設定
            StartCoroutine(SetCooldown(other));
        }
    }

    /// <summary>
    /// クールダウンがアクティブかどうかを確認
    /// </summary>
    /// <param name="other">ターゲット</param>
    /// <returns>クールダウン中かどうか</returns>
    private bool IsCooldownActive(Collider other)
    {
        return other.GetComponent<TargetCooldown>()?.IsCooldown ?? false;
    }

    /// <summary>
    /// ターゲットにクールダウンを設定
    /// </summary>
    /// <param name="other">ターゲット</param>
    private IEnumerator SetCooldown(Collider other)
    {
        var cooldown = other.GetComponent<TargetCooldown>();
        if (cooldown == null)
        {
            cooldown = other.gameObject.AddComponent<TargetCooldown>();
        }

        cooldown.StartCooldown(damageInterval);
        yield return new WaitForSeconds(damageInterval);
    }
}

/// <summary>
/// クールダウン管理クラス
/// </summary>
public class TargetCooldown : MonoBehaviour
{
    public bool IsCooldown { get; private set; }

    public void StartCooldown(float cooldownTime)
    {
        IsCooldown = true;
        Invoke(nameof(EndCooldown), cooldownTime);
    }

    private void EndCooldown()
    {
        IsCooldown = false;
    }
}
