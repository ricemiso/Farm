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

    private void Update()
    {
        // �͈͓��̂��ׂẴ^�[�Q�b�g�Ƀ_���[�W��^����
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
    /// <param name="other">�ڐG�����G</param>
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
    /// <param name="other">���E�����G</param>
    private void OnTriggerExit(Collider other)
    {
        if (targetsInRange.Contains(other))
        {
            targetsInRange.Remove(other);
            targetCooldowns.Remove(other);
        }
    }

    /// <summary>
    /// �L���ȃ^�[�Q�b�g���ǂ����𔻒�
    /// </summary>
    /// <param name="other">�^�[�Q�b�g</param>
    /// <returns>�L�����ǂ���</returns>
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
    /// <param name="other">�^�[�Q�b�g</param>
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
    /// <param name="target">�^�[�Q�b�g</param>
    private IEnumerator SetCooldown(Collider target)
    {
        targetCooldowns[target] = true;
        yield return new WaitForSeconds(damageInterval);
        targetCooldowns[target] = false; 
    }
}