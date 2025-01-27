using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    // íeÇÃÉ_ÉÅÅ[ÉW
    private float Damage;

    public void SetDamage(float damage)
    {
        Damage = damage;
    }

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
		{
			GrobalState.Instance.isDamage = true;
			collision.gameObject.GetComponent<Animal>().TakeDamage(Damage);
			Destroy(this.gameObject);
		}
	}

	//private void OnTriggerEnter(Collider other)
	//{
	//	if (other.gameObject.CompareTag("Enemy"))
	//	{
	//		GrobalState.Instance.isDamage = true;
	//		other.gameObject.GetComponent<Animal>().TakeDamage(Damage);
	//		Destroy(this.gameObject);
	//	}
	//}
}
