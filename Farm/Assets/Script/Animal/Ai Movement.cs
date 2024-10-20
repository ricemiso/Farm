using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Movement : MonoBehaviour
{
	public Animator animator;

	public float moveSpeed = 0.2f;

	Vector3 stopPosition;

	float walkTime;
	public float walkCounter;
	float waitTime;
	public float waitCounter;

	int WalkDirection;


	// ��ԃ��X�g
	public enum MoveState
	{
		WALKING,
		FOLLOWING,
		CHASE,
		WAITING,
	}

	public MoveState state;

	public GameObject player;  // �Ǐ]����v���C���[��Transform
	public float followSpeed = 5f;  // �Ǐ]���x
	protected GameObject target; // �^�[�Q�b�g���̓G

	public bool isStopped = false; // �������~���Ă��邩�ǂ����̃t���O

	public float stopRange = 20f;  // ��~�ł���͈�
	public Color gizmoColor = Color.green;  // Gizmo�̐F

	const float stopDistance = 10.0f;	// ��~���鋗��

	// Start is called before the first frame update
	protected virtual void Start()
	{
		animator = GetComponent<Animator>();

		// �v���C���[�������I�Ɏ擾����
		if (player == null)
		{
			player = GameObject.FindWithTag("Player");  // �^�O��"Player"�̃I�u�W�F�N�g�������I�Ɏ擾
		}

		// �����_���ȕ��s���ԂƑҋ@���Ԃ�ݒ�
		walkTime = Random.Range(3, 6);
		waitTime = Random.Range(5, 7);

		waitCounter = waitTime;
		walkCounter = walkTime;

		state = MoveState.WALKING;

		ChooseDirection();  // ����̕�����I��
	}


	// �ǂ������郁�\�b�h
	// followPosition �ǔ�������W
	// isStopTooClose �������߂��ꍇ�~�܂邩
	protected void Chase(Vector3 followPosition, bool isStopTooClose = false)
	{
		// �ړI�n�ƌ��݈ʒu�̋���
		float distance = Vector3.Distance(followPosition, transform.position);

		// AI�L�����N�^�[���v���C���[�̌��Ɉړ�
		Vector3 directionToFollowPosition = (followPosition - transform.position).normalized;
		Quaternion targetRotation = Quaternion.LookRotation(directionToFollowPosition);  // AI�������������v�Z
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);  // �����]�����X���[�Y��

		// �������߂����Ȃ���΋߂Â�
		if (distance < stopDistance && isStopTooClose)
		{
			// �v���C���[�̌��ɏ\���߂Â������~
			animator.SetBool("isRunning", false);
		}
		else
		{
			// �߂Â�
			transform.position += directionToFollowPosition * followSpeed * Time.deltaTime;
		}
	}

	// �����_���ɕ��s���郁�\�b�h
	protected void Walk()
	{
		animator.SetBool("isRunning", true);

		walkCounter -= Time.deltaTime;

		switch (WalkDirection)
		{
			case 0:
				transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
				transform.position += transform.forward * moveSpeed * Time.deltaTime;
				break;
			case 1:
				transform.localRotation = Quaternion.Euler(0f, 90, 0f);
				transform.position += transform.forward * moveSpeed * Time.deltaTime;
				break;
			case 2:
				transform.localRotation = Quaternion.Euler(0f, -90, 0f);
				transform.position += transform.forward * moveSpeed * Time.deltaTime;
				break;
			case 3:
				transform.localRotation = Quaternion.Euler(0f, 180, 0f);
				transform.position += transform.forward * moveSpeed * Time.deltaTime;
				break;
		}

		// ���s���Ԃ��I�������ҋ@����
		if (walkCounter <= 0)
		{
			stopPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			state = MoveState.WAITING;
			transform.position = stopPosition;
			animator.SetBool("isRunning", false);
			waitCounter = waitTime;
		}
	}

	// �����_���ȕ�����I�ԃ��\�b�h
	protected void ChooseDirection()
	{
		WalkDirection = Random.Range(0, 4);
		state = MoveState.WALKING;
		walkCounter = walkTime;
	}

}
