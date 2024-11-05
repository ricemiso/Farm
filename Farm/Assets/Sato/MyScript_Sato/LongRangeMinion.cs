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

	protected override void Start()
	{
		attackRange = 20.0f;
		base.Start();
	}

	protected override void Update()
	{
		base.Update();
	}

	protected override void checkAttack()
	{


		Vector3 spawnPosition = shootPos.transform.position + shootPos.transform.forward;
		Quaternion spawnRotation = shootPos.transform.rotation;

		GameObject newBullet = Instantiate(bullet, spawnPosition, spawnRotation);

		Vector3 direction = newBullet.transform.forward;
		newBullet.GetComponent<Rigidbody>().AddForce(direction * 10.0f, ForceMode.Impulse);
		newBullet.name = bullet.name;
		Destroy(newBullet, 0.8f);
	}
}
