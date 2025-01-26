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

	// 停止命令をおらった位置（Vector3.zeroの場合は未設定）
	protected Vector3 stopPosition;

	// 次攻撃可能になるまでのクールタイム
	[SerializeField] float currentAttackCooltime;
	public const float attackCooltime = 2.0f;

	[SerializeField] private LayerMask groundLayer;  // 地面のレイヤーマスク
	[SerializeField] private Transform groundCheck;  // 接地判定用の位置（キャラクターの足元）

	private float groundCheckRadius = 0.2f;  // 接地判定の半径
	private bool isGrounded = false;        // 接地しているかどうか

	protected override void Start()
    {
		if(animation == null)
        {
			animation = GetComponent<Animation>();
        }
		currentAttackCooltime = 0.0f;

		stopPosition = transform.position;

		isStopped = true;

		base.Start();
    }

    // Update is called once per frame
    protected override void Update()
	{
		// 接地判定
		isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

		if (!isGrounded)
		{
			// 地面に戻るロジック（例: 重力を適用）
			Vector3 velocity = gameObject.GetComponent<Rigidbody>().velocity;
			velocity.y += Physics.gravity.y * Time.deltaTime;
			gameObject.GetComponent<Rigidbody>().velocity = velocity;

			return;
		}

		if (target == null)
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
		if (Input.GetKeyDown(KeyCode.Q))  // Eキーで動作を停止/再開
		{
			// プレイヤーの正面から少し前の座標
			Vector3 followPosition = player.transform.position + player.transform.forward * 4f;

			// プレイヤーと現在位置の距離が近いなら停止
			if (Vector3.Distance(transform.position, followPosition) <= 10)
			{
				isStopped = !isStopped;  // Eキーで動作を停止/再開
				if (isStopped)
				{
					target = null;
					state = MoveState.STOP;
					if (!animation.IsPlaying("Idle"))
					{
						animation.Play("Idle");
					}

					stopPosition = gameObject.transform.position;
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

					stopPosition = Vector3.zero;
				}
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
			case MoveState.GO_BACK:
				// 停止位置から離れていたら戻る
				if (Vector3.Distance(stopPosition, gameObject.transform.position) >= 1.0f)
				{
					ReturnStopPos();
				}
				else
				{
					state = MoveState.STOP;
				}
				break;
			case MoveState.WAITING:
				//Wait();
				//break;
			case MoveState.STOP:
			default:
				if (!isAttackAnim && !animation.IsPlaying("Idle"))
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

		// 移動処理
		Chase(followPosition, true);
	}

	// 待機命令の出された場所に移動する
	void ReturnStopPos()
	{
		if (!isAttackAnim)
		{
			animation.Play("Walk");
		}

		Chase(stopPosition, true);
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
			//state = MoveState.FOLLOWING;
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
				if(stopPosition != Vector3.zero)
				{
					state = MoveState.GO_BACK;
				}
				else
				{
					state = MoveState.FOLLOWING;
				}
				target = null;
			}
		}
	}
}
