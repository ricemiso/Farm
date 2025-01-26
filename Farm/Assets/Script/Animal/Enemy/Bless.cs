using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �h���S���̃u���X���Ǘ�����v���O����
/// </summary>
public class Bless : MonoBehaviour
{
    //�S���ҁ@�z�Y�W��

    private LongRange Long;
    private HashSet<Collider> targetsInRange = new HashSet<Collider>(); // �͈͓��̃I�u�W�F�N�g��ǐ�
    public float damageInterval = 0.5f; // �_���[�W�Ԋu

    private void Start()
    {
        Long = GetComponentInParent<LongRange>();
    }

    private void Update()
    {
        // �͈͓��̂��ׂẴ^�[�Q�b�g�Ƀ_���[�W��^����
        foreach (var target in targetsInRange)
        {
            ApplyDamage(target);
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

        if (!IsCooldownActive(other))
        {
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

            // �N�[���_�E����ݒ�
            StartCoroutine(SetCooldown(other));
        }
    }

    /// <summary>
    /// �N�[���_�E�����A�N�e�B�u���ǂ������m�F
    /// </summary>
    /// <param name="other">�^�[�Q�b�g</param>
    /// <returns>�N�[���_�E�������ǂ���</returns>
    private bool IsCooldownActive(Collider other)
    {
        return other.GetComponent<TargetCooldown>()?.IsCooldown ?? false;
    }

    /// <summary>
    /// �^�[�Q�b�g�ɃN�[���_�E����ݒ�
    /// </summary>
    /// <param name="other">�^�[�Q�b�g</param>
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
/// �N�[���_�E���Ǘ��N���X
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
