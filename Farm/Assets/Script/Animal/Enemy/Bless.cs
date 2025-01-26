using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�S���ҁ@�z�Y�W��

/// <summary>
/// �h���S���̃u���X���Ǘ�����v���O����
/// </summary>
public class Bless : MonoBehaviour
{

    /// <summary>
    /// �͈͓��̃^�[�Q�b�g��ǐ�
    /// </summary>
    private HashSet<Collider> targetsInRange = new HashSet<Collider>();

    /// <summary>
    /// �^�[�Q�b�g���Ƃ̃N�[���_�E��
    /// </summary>
    private Dictionary<Collider, bool> targetCooldowns = new Dictionary<Collider, bool>();

    /// <summary>
    /// �_���[�W�Ԋu
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

            // �͈͊O�̃^�[�Q�b�g�����O
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
    /// �u���X���ڐG�����Ώۂ�ǐՃ��X�g�ɒǉ�
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (IsValidTarget(other))
        {
            targetsInRange.Add(other);
            if (!targetCooldowns.ContainsKey(other))
            {
                targetCooldowns[other] = false; // ������ԂŃN�[���_�E���̓I�t
            }
        }
    }

    /// <summary>
    /// �͈͂���o���Ώۂ�ǐՃ��X�g����폜
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        RemoveTarget(other);
    }

    /// <summary>
    /// �͈͓��̃^�[�Q�b�g���m�F���A�s�K�؂Ȃ��̂��폜
    /// </summary>
    private void ValidateTargetsInRange()
    {
        var targetsToRemove = new List<Collider>();

        foreach (var target in targetsInRange)
        {
            // �^�[�Q�b�g���͈͊O�Ȃ�폜���X�g�ɒǉ�
            if (!IsTargetInRange(target))
            {
                targetsToRemove.Add(target);
            }
        }

        // �͈͊O�̃^�[�Q�b�g�����X�g����폜
        foreach (var target in targetsToRemove)
        {
            RemoveTarget(target);
        }
    }

    /// <summary>
    /// �^�[�Q�b�g���u���X�͈͓̔��ɂ��邩���m�F
    /// </summary>
    private bool IsTargetInRange(Collider target)
    {
        if (boxCollider == null) return false;

        Bounds bounds = boxCollider.bounds;

        return bounds.Contains(target.transform.position);
    }

    /// <summary>
    /// �L���ȃ^�[�Q�b�g���ǂ����𔻒�
    /// </summary>
    private bool IsValidTarget(Collider other)
    {
        return other.CompareTag("SupportUnit") ||
               other.CompareTag("Player") ||
               other.CompareTag("Crystal") ||
               other.CompareTag("MiniCrystal");
    }

    /// <summary>
    /// �^�[�Q�b�g�Ƀ_���[�W��^���鏈��
    /// </summary>
    private void ApplyDamage(Collider other)
    {
        float damage = GetComponentInParent<Animal>().damage;

        if (other.TryGetComponent<Animal>(out var animal))
        {
            if (!animal.isDead && animal.animalName != "�h���S��")
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
    /// �^�[�Q�b�g�ɃN�[���_�E����ݒ�
    /// </summary>
    private IEnumerator SetCooldown(Collider target)
    {
        targetCooldowns[target] = true;
        yield return new WaitForSeconds(damageInterval);
        targetCooldowns[target] = false;
    }

    /// <summary>
    /// �^�[�Q�b�g�����X�g����폜
    /// </summary>
    private void RemoveTarget(Collider target)
    {
        targetsInRange.Remove(target);
        targetCooldowns.Remove(target);
    }
}