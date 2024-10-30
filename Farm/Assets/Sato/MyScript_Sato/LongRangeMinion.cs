using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeMinion : SupportAI_Movement
{
	[SerializeField]
	[Tooltip("�e�̔��ˏꏊ")]
	private GameObject shootPos;

	[SerializeField]
	[Tooltip("�e")]
	private GameObject bullet;

	[SerializeField]
	[Tooltip("�e�̑���")]
	private float speed = 30f;

	protected override void Start()
	{
		attackRange = 10.0f;
		base.Start();
	}

	protected override void Update()
	{
		base.Update();
	}

	protected override void checkAttack()
	{
		//// �e�𔭎˂���ꏊ���擾
		//Vector3 bulletPosition = shootPos.transform.position;
		// �v���C���[�̈ʒu�Ɖ�]����ɖ��@�̔��ˈʒu�ƌ���������
		Vector3 spawnPosition = shootPos.transform.position + shootPos.transform.forward;
		Quaternion spawnRotation = shootPos.transform.rotation;
		// ��Ŏ擾�����ꏊ�ɁA"bullet"��Prefab���o��������BBullet�̌�����Muzzle�̃��[�J���l�Ɠ����ɂ���i3�ڂ̈����j
		GameObject newBullet = Instantiate(bullet, spawnPosition, spawnRotation);
		// �o���������e��up(Y������)���擾�iMuzzle�̃��[�J��Y�������̂��Ɓj
		Vector3 direction = newBullet.transform.forward;
		// �e�̔��˕�����newBall��Y����(���[�J�����W)�����A�e�I�u�W�F�N�g��rigidbody�ɏՌ��͂�������
		newBullet.GetComponent<Rigidbody>().AddForce(direction * 10.0f, ForceMode.Impulse);
		// �o���������e�̖��O��"bullet"�ɕύX
		newBullet.name = bullet.name;
		// �o���������e��0.8�b��ɏ���
		Destroy(newBullet, 0.8f);
	}
}
