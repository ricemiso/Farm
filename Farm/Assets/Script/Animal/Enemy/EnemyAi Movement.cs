 using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_Movement : AI_Movement
{

	// 追跡を諦めるまでの時間
	public const float timeToGiveUpChase = 5.0f;
	// 敵を見つけてからの時間（再認識するたびにリセット）
	[SerializeField] float timeToFoundEnemy;

	// 次攻撃可能になるまでのクールタイム
	[SerializeField] float currentAttackCooltime;
	public const float attackCooltime = 1.0f;

	// クリスタルへの参照
	public GameObject Crystal;
	// ミニクリスタルへの参照
	public GameObject CrystalMini;

	protected override void Start()
    {
		currentAttackCooltime = 0.0f;
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
			// 目標がいない時にクリスタルなどの攻撃対象に向かう
			if(state != MoveState.CHASE)
			{
				if(CrystalMini.GetComponent<MiniCrystal>().IsAlive())
				{
					// ミニクリスタルが生きているなら
					FoundTarget(CrystalMini);
				}
				else
				{
					// ミニクリスタルが死んでいたら
					FoundTarget(Crystal);
				}
				
			}
		}

		timeToFoundEnemy += Time.deltaTime;
		currentAttackCooltime -= Time.deltaTime;

		base.Update();
    }

	protected void ChaseEnemy()
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

		// 近くにターゲットがいたら攻撃処理
		float distance = Vector3.Distance(followPosition, transform.position);
		if(distance <= attackRange &&
			timeToFoundEnemy <= 0.1f && 
			currentAttackCooltime <= 0.0f) 
		{
			float damage = GetComponent<Animal>().damage;
			Attack(damage);

			currentAttackCooltime = attackCooltime;
		}
	}



	// プレイヤーがコライダーに入ったとき
	private void OnTriggerStay(Collider other)
	{

		if ((other.CompareTag("Player") || other.CompareTag("SupportUnit")))  // 敵が入って、まだ追従していない場合
		{
			FoundTarget(other.GameObject());
		}
	}

	// 敵を発見したときの処理
	void FoundTarget(GameObject other)
	{
		state = MoveState.CHASE;

		timeToFoundEnemy = 0.0f;
		target = other;
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
