using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{
    Animator animator;

    public float moveSpeed = 0.2f;

    Vector3 stopPosition;

    float walkTime;
    public float walkCounter;
    float waitTime;
    public float waitCounter;

    int WalkDirection;

    public bool isWalking;

    public Transform player;  // �Ǐ]����v���C���[��Transform
    public float followSpeed = 5f;  // �Ǐ]���x
    private bool isFollowing = false;  // �v���C���[���͈͓��ɂ��邩�ǂ���
    private bool isChaseEnemy = false; // �G��ǂ�������ϐ�
    private GameObject target; // �^�[�Q�b�g���̓G

    public bool isStopped = false; // �������~���Ă��邩�ǂ����̃t���O

    public float stopRange = 20f;  // ��~�ł���͈�
    public Color gizmoColor = Color.green;  // Gizmo�̐F

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        // �v���C���[�������I�Ɏ擾����
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }

        // �����_���ȕ��s���ԂƑҋ@���Ԃ�ݒ�
        walkTime = Random.Range(3, 6);
        waitTime = Random.Range(5, 7);

        waitCounter = waitTime;
        walkCounter = walkTime;

        ChooseDirection();  // ����̕�����I��
    }

    // Update is called once per frame
    void Update()
    {
        // player �� null �łȂ����Ƃ��m�F
        if (player == null || this.gameObject==null)
        {
            return;  
        }

        // �v���C���[�Ƃ̋������v�Z
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

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

        // �G�ɒǏ]����ꍇ
        if (isChaseEnemy)
        {
            ChaseEnemy();
        }
        else if (isFollowing)
        {
            FollowPlayer();
        }
        else if (isWalking)  // �ʏ�̃����_�����s
        {
            Walk();
        }
        else  // ���s���I�����A�ҋ@��
        {
            waitCounter -= Time.deltaTime;

            if (waitCounter <= 0)
            {
                ChooseDirection();
            }
        }
    }

    // �v���C���[�Ɍ�납��Ǐ]���郁�\�b�h
    void FollowPlayer()
    {
        animator.SetBool("isRunning", true);

        // �v���C���[�̐i�s�������擾���A���̈ʒu���v�Z
        Vector3 directionBehindPlayer = -player.forward;  // �v���C���[�̌�둤
        Vector3 followPosition = player.position + directionBehindPlayer * 2f;  // �v���C���[����2���j�b�g���

        Chase(followPosition);
    }

    void ChaseEnemy()
    {
        animator.SetBool("isRunning", true);

        // �v���C���[�̐i�s�������擾���A���̈ʒu���v�Z
        Vector3 followPosition = target.transform.position;  // �v���C���[����2���j�b�g���

        Chase(followPosition);
    }

    // �ǂ������郁�\�b�h
    void Chase(Vector3 followPosition)
    {
        // �ړI�n�ƌ��݈ʒu�̋���
        float distance = Vector3.Distance(followPosition, transform.position);

        // AI�L�����N�^�[���v���C���[�̌��Ɉړ�
        Vector3 directionToFollowPosition = (followPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToFollowPosition);  // AI�������������v�Z
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);  // �����]�����X���[�Y��

        // �v���C���[���痣�ꂷ���Ă��邩
        if (distance > 10.0f)  // ������1.5���j�b�g�ȏ�Ȃ�Ǐ]�𑱂���
        {
            transform.position += directionToFollowPosition * followSpeed * Time.deltaTime;
        }
        else
        {
            // �v���C���[�̌��ɏ\���߂Â������~
            animator.SetBool("isRunning", false);
        }
    }

    // �����_���ɕ��s���郁�\�b�h
    void Walk()
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
            isWalking = false;
            transform.position = stopPosition;
            animator.SetBool("isRunning", false);
            waitCounter = waitTime;
        }
    }

    // �����_���ȕ�����I�ԃ��\�b�h
    public void ChooseDirection()
    {
        WalkDirection = Random.Range(0, 4);
        isWalking = true;
        walkCounter = walkTime;
    }

    // �v���C���[���R���C�_�[�ɓ������Ƃ�
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isFollowing)  // �v���C���[�������āA�܂��Ǐ]���Ă��Ȃ��ꍇ
        {
            isFollowing = true;
            animator.SetBool("isRunning", true);

            target = other.gameObject;
        }

        if (other.CompareTag("Enemy") && !isChaseEnemy)  // �G�������āA�܂��Ǐ]���Ă��Ȃ��ꍇ
        {
            isChaseEnemy = true;
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
