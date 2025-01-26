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

        // ターゲットと現在位置との距離を計算
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        // 判定する距離の閾値 (適宜調整)
        float detectionRange = 15.0f;

        // 距離が一定の範囲内であれば true、それ以外なら false
        isInRange = distanceToTarget <= detectionRange;
    }

    protected override void ChaseEnemy()
    {
        if (player == null || target == null) return;

        base.ChaseEnemy();

        // プレイヤーの進行方向を取得し、追尾処理
        if (target != null)
        {
            Vector3 followPosition = target.transform.position;

            // 移動処理
            Chase(followPosition, true);

            // アニメーションの切り替え
            if (animator != null)
            {  // 近くにターゲットがいたら攻撃処理
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
