using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_Movement : AI_Movement
{

	// 追跡を諦めるまでの時間
	public const float timeToGiveUpChase = 5.0f;
	// 敵を見つけてからの時間（再認識するたびにリセット）
	[SerializeField] float timeToFoundEnemy;


    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
	{
		// カウンタを進める
		timeToFoundEnemy += Time.deltaTime;

		if (!isStopped && onGround)
		{
            switch (state)
            {
                case MoveState.CHASE:
                    ChaseEnemy();
                    break;
                case MoveState.WALKING:
                    Walk();
                    break;
                case MoveState.WAITING:
                default:
					Wait();
					break;
            }
        }

        base.Update();
    }

	void ChaseEnemy()
	{
		animator.SetBool("isRunning", true);

		// 長い時間対象を認識していない場合諦める
		if(timeToFoundEnemy >= timeToGiveUpChase)
		{
			ChangeStateWait();
		}

		// プレイヤーの進行方向を取得し、後ろの位置を計算
		Vector3 followPosition = target.transform.position;  // プレイヤーから2ユニット後ろ

		Chase(followPosition);
	}



	// プレイヤーがコライダーに入ったとき
	private void OnTriggerStay(Collider other)
	{

		if ((other.CompareTag("Player") || other.CompareTag("SupportUnit")))  // 敵が入って、まだ追従していない場合
		{
			FoundTarget(other);
		}
	}

	// 敵を発見したときの処理
	void FoundTarget(Collider other)
	{
		state = MoveState.CHASE;

		timeToFoundEnemy = 0.0f;
		target = other.gameObject;
	}

	// プレイヤーがコライダーから出たとき（追従を停止しない）
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			// 追従停止のコードは削除。プレイヤーが出ても追従を続ける。
		}
	}
}
