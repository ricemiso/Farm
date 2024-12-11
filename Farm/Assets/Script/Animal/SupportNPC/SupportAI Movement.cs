using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class SupportAI_Movement : AI_Movement
{
	public bool isPushEKey = false;

	// ���U���\�ɂȂ�܂ł̃N�[���^�C��
	[SerializeField] float currentAttackCooltime;
	public const float attackCooltime = 2.0f;

	protected override void Start()
    {
		if(animation == null)
        {
			animation = GetComponent<Animation>();
        }
		currentAttackCooltime = 0.0f;
		base.Start();
    }

    // Update is called once per frame
    protected override void Update()
	{
		GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
		// �v���C���[�Ƃ̋������v�Z
		//float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

		// ���͈͓�����E�L�[�������ꂽ�ꍇ�̂ݒ�~�E�ĊJ��؂�ւ���
		if (isPushEKey && Input.GetKeyDown(KeyCode.Q))  // E�L�[�œ�����~/�ĊJ
		{
			isStopped = !isStopped;  // E�L�[�œ�����~/�ĊJ
			if (isStopped)
			{
				target = null;
				state = MoveState.STOP;
				//animator.SetBool("isRunning", false);  // �A�j���[�V�������~
			}
			else
			{
				animation.Play("Run");
				//animator.SetBool("isRunning", true);
				state = MoveState.FOLLOWING;
				target = player;
			}
		}

		switch (state)
		{
			case MoveState.CHASE:
				ChaseEnemy();
				break;
			case MoveState.FOLLOWING:
				FollowPlayer();
				break;
			case MoveState.WALKING:
				//Walk();
				break;
			case MoveState.WAITING:
				//Wait();
				break;
			case MoveState.STOP:
			default:
				animation.Play("Run");
				break;
		}

		currentAttackCooltime -= Time.deltaTime;
		base.Update();
    }

	// �v���C���[�Ɍ�납��Ǐ]���郁�\�b�h
	void FollowPlayer()
	{
		//animation.Play("Run");
		//animator.SetBool("isRunning", true);
		//animator.SetBool("isRunning", true);

		//// �v���C���[�̐i�s�������擾���A���̈ʒu���v�Z
		//Vector3 directionBehindPlayer = -player.transform.forward;  // �v���C���[�̌�둤
		//Vector3 followPosition = player.transform.position + directionBehindPlayer * 2f;  // �v���C���[����2���j�b�g���

		Vector3 followPosition = player.transform.Find("GroundCheck").position;

		Chase(followPosition, true);
	}

	void ChaseEnemy()
	{
		if (target == null) return;

		animation.Play("Run");
		//animator.SetBool("isRunning", true);

		// �v���C���[�̐i�s�������擾���A���̈ʒu���v�Z
		Vector3 followPosition = target.transform.position;  // �v���C���[����2���j�b�g���

		Chase(followPosition,true);

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

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && state != MoveState.FOLLOWING && state != MoveState.CHASE && !isStopped)
		{
			state = MoveState.FOLLOWING;
			if(animation != null)
            {
				animation.Play("Run");
			}
			
			target = other.gameObject;
		}

		if (other.CompareTag("Enemy"))
		{
			var animal = other.GetComponent<Animal>();
			if (animal != null && animal.currentHealth > 0)  // null�`�F�b�N��currentHealth�`�F�b�N
			{
				state = MoveState.CHASE;
				if(animation != null)
                {
					animation.Play("Run");
				}
				
				target = other.gameObject;

                if (animal.isDead)
                {
					state = MoveState.FOLLOWING;

				}
			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			isPushEKey = true;
			if (!isStopped)
			{
				state = MoveState.FOLLOWING;
			}
		}

		if (other.CompareTag("Enemy") && target != null) // target��null�`�F�b�N��ǉ�
		{
			var animal = target.GetComponent<Animal>();
			if (animal != null && animal.currentHealth > 0) // null�`�F�b�N��currentHealth�`�F�b�N
			{
				state = MoveState.CHASE;
				if(animation != null)
                {
					animation.Play("Run");
				}
				
				target = other.gameObject;

				if (animal.isDead)
				{
					state = MoveState.FOLLOWING;

				}
			}
			else
			{
				state = MoveState.FOLLOWING;
				target = null;
			}
		}
	}
}
