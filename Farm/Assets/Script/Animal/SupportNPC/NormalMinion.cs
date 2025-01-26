using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMinion : SupportAI_Movement
{

	private bool isCheckingAttack = false;

	/// <summary>
	/// 自分とタンクミニオンの距離
	/// </summary>
	private float distance = 0;

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();

	
		if (tankMinion != null && !tankMinion.gameObject.GetComponent<Animal>().isDead)  // タンクミニオンが生きているかチェック
		{

			Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, 50.0f); // 100.0f以内にいる敵を取得

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

		if(target != player)
        {
			animation.Play("Attack1");

			float damage = GetComponent<Animal>().damage;
			Attack(damage, target);
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
