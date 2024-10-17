using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi_Movement : Ai_Movement
{

	public new enum MoveState
	{
		WALKING,
		FOLLOWING,
		CHASE,
		WAITING,
	}

	public new MoveState state;


	// Update is called once per frame
	void Update()
	{

		if (isStopped) return;  // ��~���Ȃ珈���𒆒f

		switch (state)
		{
			case MoveState.CHASE:
				ChaseEnemy();
				break;
			case MoveState.WALKING:
				Walk();
				break;
			case MoveState.WAITING:
			default:
				waitCounter -= Time.deltaTime;

				if (waitCounter <= 0)
				{
					ChooseDirection();
				}
				break;
		}
	}

	// �v���C���[�Ɍ�납��Ǐ]���郁�\�b�h
	//void FollowPlayer()
	//{
	//	animator.SetBool("isRunning", true);

	//	// �v���C���[�̐i�s�������擾���A���̈ʒu���v�Z
	//	Vector3 directionBehindPlayer = -player.forward;  // �v���C���[�̌�둤
	//	Vector3 followPosition = player.position + directionBehindPlayer * 2f;  // �v���C���[����2���j�b�g���

	//	Chase(followPosition);
	//}

	void ChaseEnemy()
	{
		animator.SetBool("isRunning", true);

		// �v���C���[�̐i�s�������擾���A���̈ʒu���v�Z
		Vector3 followPosition = target.transform.position;  // �v���C���[����2���j�b�g���

		Chase(followPosition);
	}



	// �v���C���[���R���C�_�[�ɓ������Ƃ�
	private void OnTriggerEnter(Collider other)
	{
		//if (other.CompareTag("Player") && !isFollowing)  // �v���C���[�������āA�܂��Ǐ]���Ă��Ȃ��ꍇ
		//{
		//	isFollowing = true;
		//	animator.SetBool("isRunning", true);

		//	target = other.gameObject;
		//}

		if ((other.CompareTag("Player") || other.CompareTag("SupportUnit")) &&
			state != MoveState.CHASE)  // �G�������āA�܂��Ǐ]���Ă��Ȃ��ꍇ
		{
			state = MoveState.CHASE;
			animator.SetBool("isRunning", true);

			target = other.gameObject;
		}
	}

	// �v���C���[���R���C�_�[����o���Ƃ��i�Ǐ]���~���Ȃ��j
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			// �Ǐ]��~�̃R�[�h�͍폜�B�v���C���[���o�Ă��Ǐ]�𑱂���B
		}
	}
}
