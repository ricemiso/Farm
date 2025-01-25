using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Movement : MonoBehaviour
{
	public Animator animator;
	public new Animation animation;

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
		//None,
		WALKING,
		FOLLOWING,
		CHASE,
		WAITING,
		STOP,
		ATTACK,
		GO_BACK,
	}

	public MoveState state;

	public GameObject player;  // �Ǐ]����v���C���[��Transform
	public GameObject tankMinion = null;
	public float followSpeed = 5f;  // �Ǐ]���x
	public float rotateSpeed = 10.0f;   // ��]���x
	public GameObject target; // �^�[�Q�b�g���̓G

	public bool isStopped = false; // �������~���Ă��邩�ǂ����̃t���O

	public float stopRange = 20f;  // ��~�ł���͈�
	public Color gizmoColor = Color.green;  // Gizmo�̐F

	[SerializeField]public float stopDistance = 10.0f;   // ��~���鋗��

	public const float maxAngleToTreatAsGround = 20.0f; // �n�ʂƔ��肷��X��

	public float attackRange = 1.0f;    // �U���͈�

	// Start is called before the first frame update
	protected virtual void Start()
	{
		animator = GetComponent<Animator>();

		if (animator == null)
		{
			animation = GetComponent<Animation>();
		}

		// �v���C���[�������I�Ɏ擾����
		if (player == null)
		{
			player = GameObject.FindWithTag("Player");  // �^�O��"Player"�̃I�u�W�F�N�g�������I�Ɏ擾
		}

		if(tankMinion == null)
        {
			tankMinion = GameObject.Find("TankAI2");
        }

		// �����_���ȕ��s���ԂƑҋ@���Ԃ�ݒ�
		//walkTime = Random.Range(3, 6);
		//waitTime = Random.Range(5, 7);
		walkTime = Random.Range(1.0f, 3.0f);
		waitTime = Random.Range(2.0f, 4.0f);

		waitCounter = waitTime;
		walkCounter = walkTime;

		state = MoveState.STOP;
		


		ChooseDirection();  // ����̕�����I��

	}

	protected virtual void Update()
	{
		// �J�E���^�[�����炷
		// Walk()�Ȃ����ƁA�ڒn���Ă��Ȃ��Ƃ��ɃJ�E���g������Ȃ��Ȃ�A���Ԃ��L�тĂ��܂��Ă�������
		walkCounter -= Time.deltaTime;
		waitCounter -= Time.deltaTime;


	}

	// �ǂ������郁�\�b�h
	// followPosition �ǔ�������W
	// isStopTooClose �������߂��ꍇ�~�܂邩
	protected void Chase(Vector3 followPosition, bool isStopTooClose = false)
	{
		// ���������낦��
		followPosition.y = transform.position.y;

		// �ړI�n�ƌ��݈ʒu�̋���
		float distance = Vector3.Distance(followPosition, transform.position);

		Vector3 direction = (followPosition - transform.position).normalized;
		Quaternion targetRotation = Quaternion.LookRotation(direction);  // AI�������������v�Z
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);  // �����]�����X���[�Y��

		// �������߂����Ȃ���΋߂Â�
		if (distance < stopDistance && isStopTooClose)
		{
			// �v���C���[�̌��ɏ\���߂Â������~
			if (animator != null)
			{
				animator.SetBool("isRunning", false);
			}
			else
			{
				animation.Stop("run");
			}
		}
		else
		{
			// �߂Â�
 			transform.position += direction * followSpeed * Time.deltaTime;

			// �A�j���[�V����
			if (animator != null)
			{
				animator.SetBool("isRunning", true);
			}
		}
	}

	// �����_���ɕ��s���郁�\�b�h
	protected void Walk()
	{

		if (animator != null)
		{
			animator.SetBool("isRunning", true);
		}
		else
		{
			animation.Play("run");
		}

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
			ChangeStateWait();
		}
	}

	protected void ChangeStateWait()
	{
		stopPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		state = MoveState.STOP;
		transform.position = stopPosition;

		if (animator != null)
		{
			animator.SetBool("isRunning", false);
		}
		else
		{
			animation.Stop("run");
		}

		waitCounter = waitTime;
	}

	// �ҋ@���郁�\�b�h
	protected void Wait()
	{
		if (waitCounter <= 0)
		{
			ChooseDirection();
		}
	}

	// �����_���ȕ�����I�ԃ��\�b�h
	protected void ChooseDirection()
	{
		WalkDirection = Random.Range(0, 4);
		state = MoveState.STOP;
		walkCounter = walkTime;

		if (animator != null)
		{
			animator.SetBool("isRunning", true);
		}
		else
		{
			if (animation.GetClip("Run") != null)
			{
				animation.Play("Run");
			}
			else if (animation.GetClip("run") != null)
			{
				animation.Play("run");
			}
		}
	}

	// �ΏۂɍU��
	public void Attack(float num, GameObject obj = null)
	{
		// TODO : �̗͊Ǘ�����̃N���X�ɓ�����������

		// �^�O���Ƃɏ����𕪊�
		if (obj == null) obj = target;

		switch (obj.tag)
		{
			case "Player":
				if(num >= 30)
				{
					num = 30;
				}
				PlayerState.Instance.bloodPannl.SetActive(true);
				PlayerState.Instance.AddHealth(-num);
				SoundManager.Instance.PlaySound(SoundManager.Instance.DamageSound);
				break;
			case "Enemy":
				GrobalState.Instance.isDamage = true;
				target.GetComponent<Animal>().TakeDamage(num);
				break;
			case "SupportUnit":
				target.GetComponent<Animal>().TakeDamage(num);
				break;
			case "Crystal":
				
				target.GetComponent<CrystalGrowth>().GetHit(num);
				break;
			case "MiniCrystal":
				target.GetComponent<MiniCrystal>().GetHit(num);
				break;
			default:
				break;
		}
	}

	

	//protected void OnCollisionStay(Collision collision)
	//{
	//	// �ݒu����
	//	for (int i = 0; i < collision.contactCount; i++)
	//	{
	//		if (Vector3.Angle(Vector3.up, collision.GetContact(i).normal)
	//			< maxAngleToTreatAsGround)
	//		{
	//			//Debug.Log("�ڒn");
	//			onGround = true;
	//			break;
	//		}
	//	}
	//}
}
