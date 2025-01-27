using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMinion : SupportAI_Movement
{

	private bool isCheckingAttack = false;

	/// <summary>
	/// 自分とタンクミニオンの距離
	/// </summary>
	[SerializeField] float distance = 30.0f;

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();

	
		if (tankMinion != null && !tankMinion.gameObject.GetComponent<Animal>().isDead)  // タンクミニオンが生きているかチェック
		{

			Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, distance);

			foreach (var enemy in nearbyEnemies)
			{
				
				if (enemy.gameObject != gameObject && enemy.CompareTag("Enemy"))
				{
					
					EnemyAI_Movement enemyAI = enemy.GetComponent<EnemyAI_Movement>(); 
					if (enemyAI != null && enemyAI.target != tankMinion) 
					{
						
						enemyAI.target = tankMinion;
						enemyAI.state = MoveState.CHASE;  

					}
				}
			}
		}
	}

	protected override void checkAttack()
	{

		if(!target.CompareTag("Player") || gameObject.transform.parent.name != ConstructionManager.Instance.constructionHoldingSpot.name)
        {
			animation.Play("Attack1");

			float damage = GetComponent<Animal>().damage;
			Attack(damage, target, this.gameObject);
		}
		

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
