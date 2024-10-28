using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_Movement : AI_Movement
{

	// �ǐՂ���߂�܂ł̎���
	public const float timeToGiveUpChase = 5.0f;
	// �G�������Ă���̎��ԁi�ĔF�����邽�тɃ��Z�b�g�j
	[SerializeField] float timeToFoundEnemy;


    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
	{
		// �J�E���^��i�߂�
		timeToFoundEnemy += Time.deltaTime;

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
        }

        base.Update();
    }

	void ChaseEnemy()
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
	}



	// �v���C���[���R���C�_�[�ɓ������Ƃ�
	private void OnTriggerStay(Collider other)
	{

		if ((other.CompareTag("Player") || other.CompareTag("SupportUnit")))  // �G�������āA�܂��Ǐ]���Ă��Ȃ��ꍇ
		{
			FoundTarget(other);
		}
	}

	// �G�𔭌������Ƃ��̏���
	void FoundTarget(Collider other)
	{
		state = MoveState.CHASE;

		timeToFoundEnemy = 0.0f;
		target = other.gameObject;
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
