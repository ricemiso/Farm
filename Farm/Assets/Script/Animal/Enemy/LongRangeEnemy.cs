using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRange : EnemyAI_Movement
{
	[SerializeField]
	[Tooltip("弾の発射場所")]
	private GameObject shootPos;

	[SerializeField]
	[Tooltip("弾")]
	private GameObject bullet;

	[SerializeField]
	[Tooltip("弾の速さ")]
	private float speed = 30.0f;

	[SerializeField]
	[Tooltip("弾の生存時間")]
	private float lifeTime = 2.0f;

	bool wait = false;

	protected override void Start()
	{
		attackRange = 20.0f;
		base.Start();
	}

	protected override void Update()
	{
		base.Update();

		switch (state)
		{
			case MoveState.WALKING:
				Walk();
				break;

			case MoveState.CHASE:
				if (target != null)
				{
					ChaseEnemy();
				}
				break;

			case MoveState.WAITING:
				Wait();
				break;

			default:
				// 停止中など他の状態の処理
				break;
		}
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
		//if (obj != target) return;
		//TODO:anim-syonnotukeru
		animator.SetTrigger("Fire");
		StartCoroutine(Fire());

	}

	IEnumerator Fire()
    {
		yield return new WaitForSeconds(0.2f);
		InstanceFire();
    }

	public void InstanceFire()
	{
		if (!wait)
		{
			wait = true;

			// 弾を生成して発射
			Vector3 spawnPosition = shootPos.transform.position + shootPos.transform.forward;
			Quaternion spawnRotation = shootPos.transform.rotation;

			GameObject newBullet = Instantiate(bullet, spawnPosition, spawnRotation);

			Vector3 direction = newBullet.transform.forward;
			newBullet.GetComponent<Rigidbody>().AddForce(direction * 10.0f, ForceMode.Impulse);
			newBullet.name = bullet.name;

			Destroy(newBullet, lifeTime);

			float damage = GetComponent<Animal>().damage;
			newBullet.GetComponent<EnemyMagic>().SetDamage(damage);

			// クールダウンを開始
			StartCoroutine(FireCooldown());
		}
	}

	private IEnumerator FireCooldown()
	{
		// 指定されたクールダウン時間待機
		yield return new WaitForSeconds(2.0f);

		// waitを解除
		wait = false;
	}
}
