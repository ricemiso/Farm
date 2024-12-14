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
		animation.Play("Attack1");

		float damage = GetComponent<Animal>().damage;
		Attack(damage,target);
	}

    private void OnTriggerStay(Collider other)
    {
		base.OnTriggerStay(other);

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
