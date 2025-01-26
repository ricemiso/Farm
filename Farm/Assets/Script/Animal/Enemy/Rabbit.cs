using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rabbit : EnemyAI_Movement
{

    bool isInRange = false;

    protected override void Start()
    {
        base.Start();

        if (player == null)
        {
            Debug.LogError("Player object is not assigned or cannot be found!");
        }

        if (target == null)
        {
            Debug.LogWarning("Target is not assigned. Rabbit won't chase!");
        }
    }

    protected override void Update()
    {
        base.Update();

        // �^�[�Q�b�g�ƌ��݈ʒu�Ƃ̋������v�Z
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        // ���肷�鋗����臒l (�K�X����)
        float detectionRange = 15.0f;

        // ���������͈͓̔��ł���� true�A����ȊO�Ȃ� false
        isInRange = distanceToTarget <= detectionRange;
    }

    protected override void ChaseEnemy()
    {
        if (player == null || target == null) return;

        base.ChaseEnemy();

        // �v���C���[�̐i�s�������擾���A�ǔ�����
        if (target != null)
        {
            Vector3 followPosition = target.transform.position;

            // �ړ�����
            Chase(followPosition, true);

            // �A�j���[�V�����̐؂�ւ�
            if (animator != null)
            {  // �߂��Ƀ^�[�Q�b�g��������U������
                float distance = Vector3.Distance(followPosition, transform.position);
                animator.SetBool("isRunning", distance > attackRange);
            }
        }
    }

    public void CheckAttack(GameObject obj)
    {
        if (obj != target || !isInRange) return;

        if (currentAttackCooltime <= 0.0f)
        {
            if (animator == null)
            {
                if (animation.GetClip("attack01") != null)
                {
                    animation.Play("attack01");
                }

                if (animation.GetClip("attack02") != null)
                {
                    animation.Play("attack02");
                }

                if (animation.GetClip("attack03") != null)
                {
                    animation.Play("attack03");
                }
            }

            currentAttackCooltime = attackCooltime;
        }
    }

    public void attackwait()
    {
        if (GetComponent<Animal>().isDead == false)
        {
            float damage = GetComponent<Animal>().damage;
            Attack(damage, target);
        }

    }
}
