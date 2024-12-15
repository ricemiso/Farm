using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rabbit : EnemyAI_Movement
{

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

        //switch (state)
        //{
        //    case MoveState.WALKING:
        //        Walk();
        //        break;

        //    case MoveState.CHASE:
        //        if (target != null)
        //        {
        //            ChaseEnemy();
        //        }
        //        break;

        //    case MoveState.WAITING:
        //        Wait();
        //        break;

        //    default:
        //        // 停止中など他の状態の処理
        //        break;
        //}
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
            Chase(followPosition,true);

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
        if (obj != target) return;

        if (currentAttackCooltime <= 0.0f)
        {
            if(animator == null)
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
        if (GetComponent<Animal>().isDead == false && target != player)
        {
            float damage = GetComponent<Animal>().damage;
            Attack(damage,target);
        }
       
    }

}
