using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMinion : SupportAI_Movement
{

	private bool isCheckingAttack = false;

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

	private IEnumerator CheckAttackWithDelay()
	{
		isCheckingAttack = true;
		
		yield return new WaitForSeconds(0.2f);
		
		checkAttack();

		isCheckingAttack = false;
	}
}
