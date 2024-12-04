using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRange : EnemyAI_Movement
{
	[SerializeField]
	[Tooltip("�e�̔��ˏꏊ")]
	private GameObject shootPos;

	[SerializeField]
	[Tooltip("�e")]
	private GameObject bullet;

	[SerializeField]
	[Tooltip("�e�̑���")]
	private float speed = 30.0f;

	[SerializeField]
	[Tooltip("�e�̐�������")]
	private float lifeTime = 2.0f;

	bool wait = false;

	protected override void Start()
	{
		attackRange = 20.0f;
		base.Start();
	}

	protected override void Update()
	{
		base.Update();

		switch (state)
		{
			case MoveState.WALKING:
				Walk();
				break;

			case MoveState.CHASE:
				if (target != null)
				{
					ChaseEnemy();
				}
				break;

			case MoveState.WAITING:
				Wait();
				break;

			default:
				// ��~���ȂǑ��̏�Ԃ̏���
				break;
		}
	}

	protected override void ChaseEnemy()
	{
		if (player == null || target == null) return;

		base.ChaseEnemy();

		// �v���C���[�̐i�s�������擾���A�ǔ�����
		if (target != null)
		{
			Vector3 followPosition = target.transform.position;

			// �ړ�����
			Chase(followPosition, true);

			// �A�j���[�V�����̐؂�ւ�
			if (animator != null)
			{  // �߂��Ƀ^�[�Q�b�g��������U������
				float distance = Vector3.Distance(followPosition, transform.position);
				animator.SetBool("isRunning", distance > attackRange);
			}
		}
	}

	public void CheckAttack(GameObject obj)
	{
		//if (obj != target) return;
		//TODO:anim-syonnotukeru
		animator.SetTrigger("Fire");
		StartCoroutine(Fire());

	}

	IEnumerator Fire()
    {
		yield return new WaitForSeconds(0.2f);
		InstanceFire();
    }

	public void InstanceFire()
	{
		if (!wait)
		{
			wait = true;

			// �e�𐶐����Ĕ���
			Vector3 spawnPosition = shootPos.transform.position + shootPos.transform.forward;
			Quaternion spawnRotation = shootPos.transform.rotation;

			GameObject newBullet = Instantiate(bullet, spawnPosition, spawnRotation);

			Vector3 direction = newBullet.transform.forward;
			newBullet.GetComponent<Rigidbody>().AddForce(direction * 10.0f, ForceMode.Impulse);
			newBullet.name = bullet.name;

			Destroy(newBullet, lifeTime);

			float damage = GetComponent<Animal>().damage;
			newBullet.GetComponent<EnemyMagic>().SetDamage(damage);

			// �N�[���_�E�����J�n
			StartCoroutine(FireCooldown());
		}
	}

	private IEnumerator FireCooldown()
	{
		// �w�肳�ꂽ�N�[���_�E�����ԑҋ@
		yield return new WaitForSeconds(2.0f);

		// wait������
		wait = false;
	}
}
