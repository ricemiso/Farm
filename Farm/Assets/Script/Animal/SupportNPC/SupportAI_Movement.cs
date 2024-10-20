using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class SupportAI_Movement : AI_Movement
{

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
	{
		// �v���C���[�Ƃ̋������v�Z
		float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

		// ���͈͓�����E�L�[�������ꂽ�ꍇ�̂ݒ�~�E�ĊJ��؂�ւ���
		if (distanceToPlayer <= stopRange && Input.GetKeyDown(KeyCode.E))  // E�L�[�œ�����~/�ĊJ
		{
			isStopped = !isStopped;  // E�L�[�œ�����~/�ĊJ
			if (isStopped)
			{
				animator.SetBool("isRunning", false);  // �A�j���[�V�������~
			}
		}

		if (isStopped) return;  // ��~���Ȃ珈���𒆒f

		switch (state)
		{
			case MoveState.CHASE:
				ChaseEnemy();
				break;
			case MoveState.FOLLOWING:
				FollowPlayer();
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
	void FollowPlayer()
	{
		animator.SetBool("isRunning", true);

		//// �v���C���[�̐i�s�������擾���A���̈ʒu���v�Z
		//Vector3 directionBehindPlayer = -player.transform.forward;  // �v���C���[�̌�둤
		//Vector3 followPosition = player.transform.position + directionBehindPlayer * 2f;  // �v���C���[����2���j�b�g���

		Vector3 followPosition = player.transform.Find("GroundCheck").position;

		Chase(followPosition, true);
	}

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
		if (other.CompareTag("Player") &&
			state != MoveState.FOLLOWING &&
			state != MoveState.CHASE)  // �v���C���[�������āA�܂��Ǐ]���Ă��Ȃ��ꍇ
		{
			state = MoveState.FOLLOWING;
			animator.SetBool("isRunning", true);

			target = other.gameObject;
		}

		if (other.CompareTag("Enemy") &&
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
