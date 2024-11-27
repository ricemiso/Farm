using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRange : EnemyAI_Movement
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

	protected override void Start()
	{
		attackRange = 20.0f;
		base.Start();
	}

	protected override void Update()
	{
		base.Update();
	}

	public void CheckAttack(GameObject obj)
	{
		if (obj != target) return;
		//TODO:anim-syonnotukeru
	}

	public void InstanceFire()
    {
		Vector3 spawnPosition = shootPos.transform.position + shootPos.transform.forward;
		Quaternion spawnRotation = shootPos.transform.rotation;

		GameObject newBullet = Instantiate(bullet, spawnPosition, spawnRotation);

		Vector3 direction = newBullet.transform.forward;
		newBullet.GetComponent<Rigidbody>().AddForce(direction * 10.0f, ForceMode.Impulse);
		newBullet.name = bullet.name;
		Destroy(newBullet, lifeTime);
		float damage = GetComponent<Animal>().damage;
		newBullet.GetComponent<Magic>().SetDamage(damage);
	}
}
