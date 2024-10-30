using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class SupportAI_Movement : AI_Movement
{
	// ���U���\�ɂȂ�܂ł̃N�[���^�C��
	[SerializeField] float currentAttackCooltime;
	public const float attackCooltime = 2.0f;
	protected override void Start()
    {
		currentAttackCooltime = 0.0f;
		base.Start();
    }

    // Update is called once per frame
    protected override void Update()
	{
		// �v���C���[�Ƃ̋������v�Z
		float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

		// ���͈͓�����E�L�[�������ꂽ�ꍇ�̂ݒ�~�E�ĊJ��؂�ւ���
		if (distanceToPlayer <= stopRange && Input.GetKeyDown(KeyCode.E))  // E�L�[�œ�����~/�ĊJ
		{
			isStopped = !isStopped;  // E�L�[�œ�����~/�ĊJ
			if (isStopped)
			{
				animation.Stop("Run");
				//animator.SetBool("isRunning", false);  // �A�j���[�V�������~
			}
			else
			{
				animation.Play("Run");
			}
		}

		if (!isStopped && onGround)
		{
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
					Wait();
					break;
			}
		}

		currentAttackCooltime -= Time.deltaTime;

		base.Update();
    }

	// �v���C���[�Ɍ�납��Ǐ]���郁�\�b�h
	void FollowPlayer()
	{
		animation.Play("Run");
		//animator.SetBool("isRunning", true);

		//// �v���C���[�̐i�s�������擾���A���̈ʒu���v�Z
		//Vector3 directionBehindPlayer = -player.transform.forward;  // �v���C���[�̌�둤
		//Vector3 followPosition = player.transform.position + directionBehindPlayer * 2f;  // �v���C���[����2���j�b�g���

		Vector3 followPosition = player.transform.Find("GroundCheck").position;

		Chase(followPosition, true);
	}

	void ChaseEnemy()
	{
		animation.Play("Run");
		//animator.SetBool("isRunning", true);

		// �v���C���[�̐i�s�������擾���A���̈ʒu���v�Z
		Vector3 followPosition = target.transform.position;  // �v���C���[����2���j�b�g���

		Chase(followPosition);


		float distance = Vector3.Distance(followPosition, transform.position);
		if (distance <= attackRange &&
			currentAttackCooltime <= 0.0f)
		{
			checkAttack();
			currentAttackCooltime = attackCooltime;
		}
	}

	virtual protected void checkAttack()
	{}

	// �v���C���[���R���C�_�[�ɓ������Ƃ�
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") &&
			//state != MoveState.FOLLOWING &&
			state != MoveState.CHASE &&
			!isStopped)  // �v���C���[�������āA�܂��Ǐ]���Ă��Ȃ��A�~�j�I������~���Ă��Ȃ��ꍇ
		{
			state = MoveState.FOLLOWING;
			animation.Play("Run");
			//animator.SetBool("isRunning", true);

			target = other.gameObject;
		}

		if (other.CompareTag("Enemy") //&&
			//state != MoveState.CHASE)  // �G�������āA�܂��Ǐ]���Ă��Ȃ��ꍇ
			)
		{
			isStopped = false;

			state = MoveState.CHASE;
			animation.Play("Run");
			//animator.SetBool("isRunning", true);

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
