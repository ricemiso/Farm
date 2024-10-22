using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_Movement : AI_Movement
{

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
	{

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

	// プレイヤーに後ろから追従するメソッド
	//void FollowPlayer()
	//{
	//	animator.SetBool("isRunning", true);

	//	// プレイヤーの進行方向を取得し、後ろの位置を計算
	//	Vector3 directionBehindPlayer = -player.forward;  // プレイヤーの後ろ側
	//	Vector3 followPosition = player.position + directionBehindPlayer * 2f;  // プレイヤーから2ユニット後ろ

	//	Chase(followPosition);
	//}

	void ChaseEnemy()
	{
		animator.SetBool("isRunning", true);

		// プレイヤーの進行方向を取得し、後ろの位置を計算
		Vector3 followPosition = target.transform.position;  // プレイヤーから2ユニット後ろ

		Chase(followPosition);
	}



	// プレイヤーがコライダーに入ったとき
	private void OnTriggerEnter(Collider other)
	{
		//if (other.CompareTag("Player") && !isFollowing)  // プレイヤーが入って、まだ追従していない場合
		//{
		//	isFollowing = true;
		//	animator.SetBool("isRunning", true);

		//	target = other.gameObject;
		//}

		if ((other.CompareTag("Player") || other.CompareTag("SupportUnit")) &&
			state != MoveState.CHASE)  // 敵が入って、まだ追従していない場合
		{
			state = MoveState.CHASE;
			animator.SetBool("isRunning", true);

			target = other.gameObject;
		}
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
