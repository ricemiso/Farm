using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class SupportAI_Movement : AI_Movement
{
	public bool isPushQKey = false;
	public bool isAttackAnim = false;

	// 次攻撃可能になるまでのクールタイム
	[SerializeField] float currentAttackCooltime;
	public const float attackCooltime = 2.0f;

	protected override void Start()
    {
		if(animation == null)
        {
			animation = GetComponent<Animation>();
        }
		currentAttackCooltime = 0.0f;
		base.Start();
    }

    // Update is called once per frame
    protected override void Update()
	{
		if(target == null)
        {
			target = player;
        }

		if (state == MoveState.CHASE && (animation.IsPlaying("Attack1") || animation.IsPlaying("Attack2")))
		{
			isAttackAnim = true;
		}
		else
		{
			isAttackAnim = false;
		}

		// プレイヤーとの距離を計算
		//float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

		// 一定範囲内かつQキーが押された場合のみ停止・再開を切り替える
		if (isPushQKey && Input.GetKeyDown(KeyCode.Q))  // Eキーで動作を停止/再開
		{
			isStopped = !isStopped;  // Eキーで動作を停止/再開
			if (isStopped)
			{
				target = null;
				state = MoveState.STOP;
				//animator.SetBool("isRunning", false);  // アニメーションを停止
			}
			else
			{
				if (!isAttackAnim)
				{
					animation.Play("Walk");
				}
				//animator.SetBool("isRunning", true);
				state = MoveState.FOLLOWING;
				target = player;
			}
		}

		switch (state)
		{
			case MoveState.CHASE:
				ChaseEnemy();
				break;
			case MoveState.FOLLOWING:
				FollowPlayer();
				break;
			case MoveState.WALKING:
				//Walk();
				if (!isAttackAnim)
				{
					animation.Play("Walk");
				}
				break;
			case MoveState.WAITING:
				//Wait();
				//break;
			case MoveState.STOP:
			default:
				if (!isAttackAnim)
				{
					animation.Play("Idle");
				}
				break;
		}

		currentAttackCooltime -= Time.deltaTime;
		base.Update();

		
    }

	// プレイヤーに後ろから追従するメソッド
	void FollowPlayer()
	{
		if (!isAttackAnim)
		{
			animation.Play("Walk");
		}
		//animation.Play("Run");
		//animator.SetBool("isRunning", true);
		//animator.SetBool("isRunning", true);

		//// プレイヤーの進行方向を取得し、後ろの位置を計算
		//Vector3 directionBehindPlayer = -player.transform.forward;  // プレイヤーの後ろ側
		//Vector3 followPosition = player.transform.position + directionBehindPlayer * 2f;  // プレイヤーから2ユニット後ろ

		Vector3 followPosition = player.transform.Find("GroundCheck").position;

		Chase(followPosition, true);
	}

	void ChaseEnemy()
	{
		if (target == null) return;

		if (!isAttackAnim)
		{
			animation.Play("Walk");
		}
		//animator.SetBool("isRunning", true);

		// プレイヤーの進行方向を取得し、後ろの位置を計算
		Vector3 followPosition = target.transform.position;  // プレイヤーから2ユニット後ろ

		Chase(followPosition,true);

		float distance = Vector3.Distance(followPosition, transform.position);
		if (distance <= attackRange &&
			currentAttackCooltime <= 0.0f)
		{
			//checkAttack();
			animation.Stop(); //攻撃アニメーションをすぐに再生させるため
			currentAttackCooltime = attackCooltime;
		}
	}

	virtual protected void checkAttack()
	{}

	private void OnTriggerEnter(Collider other)
	{
		//GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);

		if (other.CompareTag("Player") && state != MoveState.FOLLOWING && state != MoveState.CHASE && !isStopped)
		{
			isPushQKey = true;

			state = MoveState.FOLLOWING;
			if(animation != null && !isAttackAnim)
            {
				animation.Play("Walk");
			}

			target = other.gameObject;
		}

		if (other.CompareTag("Enemy"))
		{
			var animal = other.GetComponent<Animal>();
			if (animal != null && animal.currentHealth > 0)  // nullチェックとcurrentHealthチェック
			{
				state = MoveState.CHASE;
				//if(animation != null)
    //            {
				//	animation.Play("Walk");
				//}
				
				target = other.gameObject;

                if (animal.isDead)
                {
					target = null;
					state = MoveState.WAITING;
				}
			}
		}
	}

	protected void OnTriggerStay(Collider other)
	{
		//Debug.Log(this.gameObject.name + "  " + other.name + " + " + other.tag);

		//GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);

		if (other.CompareTag("Player"))
		{
			isPushQKey = true;
			if (!isStopped)
			{
				state = MoveState.FOLLOWING;
			}
		}

		if (other.CompareTag("Enemy") && target != null) // targetのnullチェックを追加
		{
			var animal = target.GetComponent<Animal>();
			if (animal != null && animal.currentHealth > 0) // nullチェックとcurrentHealthチェック
			{
				state = MoveState.CHASE;
				//if(animation != null)
				//{
				//	animation.Play("Walk");
				//}
				
				target = other.gameObject;

				if (animal.isDead)
				{
					state = MoveState.FOLLOWING;

				}
			}
			else
			{
				state = MoveState.WAITING;
				target = null;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			isPushQKey = false;
		}
	}
}
