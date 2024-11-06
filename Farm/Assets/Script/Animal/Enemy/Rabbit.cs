using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rabbit : EnemyAI_Movement
{

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();
	}

	protected override void ChaseEnemy()
	{
		if (player == null) return;

		base.ChaseEnemy();

		// �v���C���[�̐i�s�������擾���A���̈ʒu���v�Z
		Vector3 followPosition = target.transform.position;  // �v���C���[����2���j�b�g���

		Chase(followPosition);

		// �߂��Ƀ^�[�Q�b�g��������U������
		float distance = Vector3.Distance(followPosition, transform.position);
		if (distance <= attackRange &&
			timeToFoundEnemy <= 0.1f &&
			currentAttackCooltime <= 0.0f)
		{
			float damage = GetComponent<Animal>().damage;
			Attack(damage);

			currentAttackCooltime = attackCooltime;
		}
	}

	// �U���͈͂��w�肵�Ă���R���W�������U���Ώۂ����m�����ۂɓ���
	public void CheckAttack(GameObject obj)
	{
		if (obj != target) return;

		if (currentAttackCooltime <= 0.0f)
		{
			float damage = GetComponent<Animal>().damage;
			Attack(damage);

			currentAttackCooltime = attackCooltime;
		}
	}
}
