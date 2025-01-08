using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    // �e�̃_���[�W
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
