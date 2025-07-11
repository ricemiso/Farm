using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
	// 弾のダメージ
	private float Damage;

	public void SetDamage(float damage)
	{
		Damage = damage;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("SupportUnit"))
		{
			collision.gameObject.GetComponent<Animal>().TakeDamage(Damage);
			Destroy(this.gameObject);
		}
	}
}
