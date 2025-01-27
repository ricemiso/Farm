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

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] BoxCollider boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (boxCollider == null) return;

            // 範囲外のターゲットを除外
            ValidateTargetsInRange();

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
    private void OnTriggerExit(Collider other)
    {
        RemoveTarget(other);
    }

    /// <summary>
    /// 範囲内のターゲットを確認し、不適切なものを削除
    /// </summary>
    private void ValidateTargetsInRange()
    {
        var targetsToRemove = new List<Collider>();

        foreach (var target in targetsInRange)
        {
            // ターゲットが範囲外なら削除リストに追加
            if (!IsTargetInRange(target))
            {
                targetsToRemove.Add(target);
            }
        }

        // 範囲外のターゲットをリストから削除
        foreach (var target in targetsToRemove)
        {
            RemoveTarget(target);
        }
    }

    /// <summary>
    /// ターゲットがブレスの範囲内にいるかを確認（Bounds.Contains を使わない）
    /// </summary>
    /// <summary>
    /// ターゲットが拡張されたブレスの範囲内にいるかを確認
    /// </summary>
    private bool IsTargetInRange(Collider target)
    {
        if (boxCollider == null) return false;

        // 判定範囲の拡大倍率（必要に応じてインスペクターから調整可能）
        float rangeMultiplier = 1.5f; // 1.5倍の範囲

        // BoxCollider の中心とサイズを取得
        Vector3 boxCenter = boxCollider.transform.TransformPoint(boxCollider.center);
        Vector3 boxSize = (boxCollider.size * 0.5f) * rangeMultiplier; // 範囲を拡大

        // ターゲットの位置を BoxCollider のローカル空間に変換
        Vector3 localTargetPosition = boxCollider.transform.InverseTransformPoint(target.transform.position);

        // ローカル空間の範囲でチェック
        return Mathf.Abs(localTargetPosition.x) <= boxSize.x &&
               Mathf.Abs(localTargetPosition.y) <= boxSize.y &&
               Mathf.Abs(localTargetPosition.z) <= boxSize.z;
    }


    /// <summary>
    /// 有効なターゲットかどうかを判定
    /// </summary>
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
    private IEnumerator SetCooldown(Collider target)
    {
        targetCooldowns[target] = true;
        yield return new WaitForSeconds(damageInterval);
        targetCooldowns[target] = false;
    }

    /// <summary>
    /// ターゲットをリストから削除
    /// </summary>
    private void RemoveTarget(Collider target)
    {
        targetsInRange.Remove(target);
        targetCooldowns.Remove(target);
    }
}