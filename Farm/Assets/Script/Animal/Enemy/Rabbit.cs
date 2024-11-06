using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rabbit : EnemyAI_Movement
{

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();
	}

	protected override void ChaseEnemy()
	{
		if (player == null) return;

		base.ChaseEnemy();

		// プレイヤーの進行方向を取得し、後ろの位置を計算
		Vector3 followPosition = target.transform.position;  // プレイヤーから2ユニット後ろ

		Chase(followPosition);

		// 近くにターゲットがいたら攻撃処理
		float distance = Vector3.Distance(followPosition, transform.position);
		if (distance <= attackRange &&
			timeToFoundEnemy <= 0.1f &&
			currentAttackCooltime <= 0.0f)
		{
			float damage = GetComponent<Animal>().damage;
			Attack(damage);

			currentAttackCooltime = attackCooltime;
		}
	}

	// 攻撃範囲を指定しているコリジョンが攻撃対象を検知した際に動作
	public void CheckAttack(GameObject obj)
	{
		if (obj != target) return;

		if (currentAttackCooltime <= 0.0f)
		{
			float damage = GetComponent<Animal>().damage;
			Attack(damage);

			currentAttackCooltime = attackCooltime;
		}
	}
}
