using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMinion : SupportAI_Movement
{

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();
	}

	protected override void checkAttack()
	{
		float damage = GetComponent<Animal>().damage;
		Attack(damage);
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
			checkAttack();

		}
    }
}
