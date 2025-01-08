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

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Enemy"))
		{
			other.gameObject.GetComponent<Animal>().TakeDamage(Damage);
			this.gameObject.SetActive(false);
			Destroy(this.gameObject);
		}
	}
	


}
