using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class SupportAI_Movement : AI_Movement
{
	// 次攻撃可能になるまでのクールタイム
	[SerializeField] float currentAttackCooltime;
	public const float attackCooltime = 2.0f;
	protected override void Start()
    {
		currentAttackCooltime = 0.0f;
		base.Start();
    }

    // Update is called once per frame
    protected override void Update()
	{
		// プレイヤーとの距離を計算
		float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

		// 一定範囲内かつEキーが押された場合のみ停止・再開を切り替える
		if (distanceToPlayer <= stopRange && Input.GetKeyDown(KeyCode.E))  // Eキーで動作を停止/再開
		{
			isStopped = !isStopped;  // Eキーで動作を停止/再開
			if (isStopped)
			{
				animation.Stop("Run");
				//animator.SetBool("isRunning", false);  // アニメーションを停止
			}
			else
			{
				animation.Play("Run");
			}
		}

		if (!isStopped && onGround)
		{
			switch (state)
			{
				case MoveState.CHASE:
					ChaseEnemy();
					break;
				case MoveState.FOLLOWING:
					FollowPlayer();
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

		currentAttackCooltime -= Time.deltaTime;

		base.Update();
    }

	// プレイヤーに後ろから追従するメソッド
	void FollowPlayer()
	{
		animation.Play("Run");
		//animator.SetBool("isRunning", true);

		//// プレイヤーの進行方向を取得し、後ろの位置を計算
		//Vector3 directionBehindPlayer = -player.transform.forward;  // プレイヤーの後ろ側
		//Vector3 followPosition = player.transform.position + directionBehindPlayer * 2f;  // プレイヤーから2ユニット後ろ

		Vector3 followPosition = player.transform.Find("GroundCheck").position;

		Chase(followPosition, true);
	}

	void ChaseEnemy()
	{
		animation.Play("Run");
		//animator.SetBool("isRunning", true);

		// プレイヤーの進行方向を取得し、後ろの位置を計算
		Vector3 followPosition = target.transform.position;  // プレイヤーから2ユニット後ろ

		Chase(followPosition);


		float distance = Vector3.Distance(followPosition, transform.position);
		if (distance <= attackRange &&
			currentAttackCooltime <= 0.0f)
		{
			checkAttack();
			currentAttackCooltime = attackCooltime;
		}
	}

	virtual protected void checkAttack()
	{}

	// プレイヤーがコライダーに入ったとき
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") &&
			//state != MoveState.FOLLOWING &&
			state != MoveState.CHASE &&
			!isStopped)  // プレイヤーが入って、まだ追従していない、ミニオンが停止していない場合
		{
			state = MoveState.FOLLOWING;
			animation.Play("Run");
			//animator.SetBool("isRunning", true);

			target = other.gameObject;
		}

		if (other.CompareTag("Enemy") //&&
			//state != MoveState.CHASE)  // 敵が入って、まだ追従していない場合
			)
		{
			isStopped = false;

			state = MoveState.CHASE;
			animation.Play("Run");
			//animator.SetBool("isRunning", true);

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
