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
	[SerializeField] public float timeToFoundEnemy;

	// ���U���\�ɂȂ�܂ł̃N�[���^�C��
	[SerializeField] public  float currentAttackCooltime;
	public const float attackCooltime = 1.0f;

	// �N���X�^���ւ̎Q��
	// null�ł悢
	public GameObject Crystal;
	// �~�j�N���X�^���ւ̎Q��
	// null�ł悢
	public GameObject CrystalMini;

	// �T�|�[�g���j�b�g���������ۂɒǂ������邩
	public bool IsChaseSupportUnit = false;
	// �v���C���[���j�b�g���������ۂɒǂ������邩
	public bool IsChasePlayer = false;




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
					if (target != null)
					{
						ChaseEnemy();
					}
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
				if(CrystalMini != null && Crystal != null)
                {
					if (CrystalMini.GetComponent<MiniCrystal>().IsAlive())
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
		}

		timeToFoundEnemy += Time.deltaTime;
		currentAttackCooltime -= Time.deltaTime;

		base.Update();
    }

	virtual protected void ChaseEnemy()
	{
		if(animator != null)
        {
			animator.SetTrigger("Run");
		}
        else
        {
			if(!animation.isPlaying)
			{
				//animation.Stop("run");
				animation.Play("run");
			}


		}

		// �������ԑΏۂ�F�����Ă��Ȃ��ꍇ���߂�
		if(timeToFoundEnemy >= timeToGiveUpChase)
		{
			ChangeStateWait();
		}
	}


	// �v���C���[���R���C�_�[�ɓ������Ƃ�
	private void OnTriggerStay(Collider other)
	{
		if(other.CompareTag("SupportUnit") && IsChaseSupportUnit)
		{
			FoundTarget(other.GameObject());
		}

		if(other.CompareTag("Player") && IsChasePlayer)
		{
			FoundTarget(other.GameObject());
		}

	}


	// �G�𔭌������Ƃ��̏���
	protected void FoundTarget(GameObject other)
	{
		state = MoveState.CHASE;

		timeToFoundEnemy = 0.0f;
		target = other;
	}

}
