using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMinion : SupportAI_Movement
{
	// ���U���\�ɂȂ�܂ł̃N�[���^�C��
	[SerializeField] float currentAttackCooltime;
	public const float attackCooltime = 1.0f;

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
}
