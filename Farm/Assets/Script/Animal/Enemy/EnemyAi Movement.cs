 using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_Movement : AI_Movement
{

	// �ǐՂ���߂�܂ł̎���
	public const float timeToGiveUpChase = 5.0f;
	// �G�������Ă���̎��ԁi�ĔF�����邽�тɃ��Z�b�g�j
	[SerializeField] float timeToFoundEnemy;

	// ���U���\�ɂȂ�܂ł̃N�[���^�C��
	[SerializeField] float currentAttackCooltime;
	public const float attackCooltime = 1.0f;

	// �N���X�^���ւ̎Q��
	public GameObject Crystal;
	// �~�j�N���X�^���ւ̎Q��
	public GameObject CrystalMini;

	protected override void Start()
    {
		currentAttackCooltime = 0.0f;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
	{

		if (!isStopped && onGround)
		{
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
					Wait();
					break;
            }
			// �ڕW�����Ȃ����ɃN���X�^���Ȃǂ̍U���ΏۂɌ�����
			if(state != MoveState.CHASE)
			{
				if(CrystalMini.GetComponent<MiniCrystal>().IsAlive())
				{
					// �~�j�N���X�^���������Ă���Ȃ�
					FoundTarget(CrystalMini);
				}
				else
				{
					// �~�j�N���X�^��������ł�����
					FoundTarget(Crystal);
				}
				
			}
		}

		timeToFoundEnemy += Time.deltaTime;
		currentAttackCooltime -= Time.deltaTime;

		base.Update();
    }

	protected void ChaseEnemy()
	{
		animator.SetBool("isRunning", true);

		// �������ԑΏۂ�F�����Ă��Ȃ��ꍇ���߂�
		if(timeToFoundEnemy >= timeToGiveUpChase)
		{
			ChangeStateWait();
		}

		// �v���C���[�̐i�s�������擾���A���̈ʒu���v�Z
		Vector3 followPosition = target.transform.position;  // �v���C���[����2���j�b�g���

		Chase(followPosition);

		// �߂��Ƀ^�[�Q�b�g��������U������
		float distance = Vector3.Distance(followPosition, transform.position);
		if(distance <= attackRange &&
			timeToFoundEnemy <= 0.1f && 
			currentAttackCooltime <= 0.0f) 
		{
			float damage = GetComponent<Animal>().damage;
			Attack(damage);

			currentAttackCooltime = attackCooltime;
		}
	}



	// �v���C���[���R���C�_�[�ɓ������Ƃ�
	private void OnTriggerStay(Collider other)
	{

		if ((other.CompareTag("Player") || other.CompareTag("SupportUnit")))  // �G�������āA�܂��Ǐ]���Ă��Ȃ��ꍇ
		{
			FoundTarget(other.GameObject());
		}
	}

	// �G�𔭌������Ƃ��̏���
	void FoundTarget(GameObject other)
	{
		state = MoveState.CHASE;

		timeToFoundEnemy = 0.0f;
		target = other;
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
