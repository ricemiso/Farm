using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class SupportAI_Movement : AI_Movement
{
	public bool isPushQKey = false;
	public bool isAttackAnim = false;

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
		if(target == null)
        {
			target = player;
        }

		if (state == MoveState.CHASE && (animation.IsPlaying("Attack1") || animation.IsPlaying("Attack2")))
		{
			isAttackAnim = true;
		}
		else
		{
			isAttackAnim = false;
		}

		// �v���C���[�Ƃ̋������v�Z
		//float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

		// ���͈͓�����Q�L�[�������ꂽ�ꍇ�̂ݒ�~�E�ĊJ��؂�ւ���
		if (isPushQKey && Input.GetKeyDown(KeyCode.Q))  // E�L�[�œ�����~/�ĊJ
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
				if (!isAttackAnim)
				{
					animation.Play("Walk");
				}
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
				if (!isAttackAnim)
				{
					animation.Play("Walk");
				}
				break;
			case MoveState.WAITING:
				//Wait();
				//break;
			case MoveState.STOP:
			default:
				if (!isAttackAnim)
				{
					animation.Play("Idle");
				}
				break;
		}

		currentAttackCooltime -= Time.deltaTime;
		base.Update();

		
    }

	// �v���C���[�Ɍ�납��Ǐ]���郁�\�b�h
	void FollowPlayer()
	{
		if (!isAttackAnim)
		{
			animation.Play("Walk");
		}
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

		if (!isAttackAnim)
		{
			animation.Play("Walk");
		}
		//animator.SetBool("isRunning", true);

		// �v���C���[�̐i�s�������擾���A���̈ʒu���v�Z
		Vector3 followPosition = target.transform.position;  // �v���C���[����2���j�b�g���

		Chase(followPosition,true);

		float distance = Vector3.Distance(followPosition, transform.position);
		if (distance <= attackRange &&
			currentAttackCooltime <= 0.0f)
		{
			//checkAttack();
			animation.Stop(); //�U���A�j���[�V�����������ɍĐ������邽��
			currentAttackCooltime = attackCooltime;
		}
	}

	virtual protected void checkAttack()
	{}

	private void OnTriggerEnter(Collider other)
	{
		//GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);

		if (other.CompareTag("Player") && state != MoveState.FOLLOWING && state != MoveState.CHASE && !isStopped)
		{
			isPushQKey = true;

			state = MoveState.FOLLOWING;
			if(animation != null && !isAttackAnim)
            {
				animation.Play("Walk");
			}

			target = other.gameObject;
		}

		if (other.CompareTag("Enemy"))
		{
			var animal = other.GetComponent<Animal>();
			if (animal != null && animal.currentHealth > 0)  // null�`�F�b�N��currentHealth�`�F�b�N
			{
				state = MoveState.CHASE;
				//if(animation != null)
    //            {
				//	animation.Play("Walk");
				//}
				
				target = other.gameObject;

                if (animal.isDead)
                {
					target = null;
					state = MoveState.WAITING;
				}
			}
		}
	}

	protected void OnTriggerStay(Collider other)
	{
		//Debug.Log(this.gameObject.name + "  " + other.name + " + " + other.tag);

		//GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);

		if (other.CompareTag("Player"))
		{
			isPushQKey = true;
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
				//if(animation != null)
				//{
				//	animation.Play("Walk");
				//}
				
				target = other.gameObject;

				if (animal.isDead)
				{
					state = MoveState.FOLLOWING;

				}
			}
			else
			{
				state = MoveState.WAITING;
				target = null;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			isPushQKey = false;
		}
	}
}
