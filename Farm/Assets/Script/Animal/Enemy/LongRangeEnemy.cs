using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRange : EnemyAI_Movement
{
	[SerializeField]
	[Tooltip("’e‚Ì”­ŽËêŠ")]
	private GameObject shootPos;

	[SerializeField]
	[Tooltip("’e")]
	private GameObject bullet;

	[SerializeField]
	[Tooltip("’e‚Ì‘¬‚³")]
	private float speed = 30.0f;

	[SerializeField]
	[Tooltip("’e‚Ì¶‘¶ŽžŠÔ")]
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
				// ’âŽ~’†‚È‚Ç‘¼‚Ìó‘Ô‚Ìˆ—
				break;
		}
	}

	protected override void ChaseEnemy()
	{
		if (player == null || target == null) return;

		base.ChaseEnemy();

		// ƒvƒŒƒCƒ„[‚Ìis•ûŒü‚ðŽæ“¾‚µA’Ç”öˆ—
		if (target != null)
		{
			Vector3 followPosition = target.transform.position;

			// ˆÚ“®ˆ—
			Chase(followPosition, true);

			// ƒAƒjƒ[ƒVƒ‡ƒ“‚ÌØ‚è‘Ö‚¦
			if (animator != null)
			{  // ‹ß‚­‚Éƒ^[ƒQƒbƒg‚ª‚¢‚½‚çUŒ‚ˆ—
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
		
		if (wait == false)
		{
			wait = true;

			Vector3 spawnPosition = shootPos.transform.position + shootPos.transform.forward;
			Quaternion spawnRotation = shootPos.transform.rotation;

			GameObject newBullet = Instantiate(bullet, spawnPosition, spawnRotation);

			Vector3 direction = newBullet.transform.forward;
			newBullet.GetComponent<Rigidbody>().AddForce(direction * 10.0f, ForceMode.Impulse);
			newBullet.name = bullet.name;
			Destroy(newBullet, lifeTime);
			float damage = GetComponent<Animal>().damage;
			newBullet.GetComponent<EnemyMagic>().SetDamage(damage);
			//StartCoroutine(Fire2());
			wait = false;
		}

	}
	IEnumerator Fire2()
	{
		yield return new WaitForSeconds(3.0f);
		wait = false;
	}
}
