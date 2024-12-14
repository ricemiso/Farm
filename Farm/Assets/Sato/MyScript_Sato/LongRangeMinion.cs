using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeMinion : SupportAI_Movement
{
	[SerializeField]
	[Tooltip("íeÇÃî≠éÀèÍèä")]
	private GameObject shootPos;

	[SerializeField]
	[Tooltip("íe")]
	private GameObject bullet;

	[SerializeField]
	[Tooltip("íeÇÃë¨Ç≥")]
	private float speed = 30.0f;

	[SerializeField]
	[Tooltip("íeÇÃê∂ë∂éûä‘")]
	private float lifeTime = 2.0f;


	private bool isCheckingAttack = false;

	protected override void Start()
	{
		attackRange = 20.0f;
		base.Start();

		target = player;
		state = MoveState.FOLLOWING;
	}

	protected override void Update()
	{
		base.Update();
	}

	protected override void checkAttack()
	{
		//if(state != MoveState.CHASE || target.tag == "Player") return;

		animation["Attack2"].speed = 1.7f;
		animation.Play("Attack2");

		Vector3 spawnPosition = shootPos.transform.position + shootPos.transform.forward;
		Quaternion spawnRotation = shootPos.transform.rotation;

		GameObject newBullet = Instantiate(bullet, spawnPosition, spawnRotation);

		Vector3 direction = newBullet.transform.forward;
		newBullet.GetComponent<Rigidbody>().AddForce(direction * 500.0f, ForceMode.Impulse);
		newBullet.name = bullet.name;
		Destroy(newBullet, lifeTime);
		float damage = GetComponent<Animal>().damage;
		newBullet.GetComponent<Magic>().SetDamage(damage);
	}


    private void OnTriggerStay(Collider other)
    {
		if (other.CompareTag("Enemy") && !isCheckingAttack)
		{
			StartCoroutine(CheckAttackWithDelay());
		}
	}

	private IEnumerator CheckAttackWithDelay()
	{
		isCheckingAttack = true;

		yield return new WaitForSeconds(2.0f);

		checkAttack();

		isCheckingAttack = false;
	}
}
