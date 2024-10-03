using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Movement : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        // �v���C���[�������I�Ɏ擾����
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;  // �^�O��"Player"�̃I�u�W�F�N�g�������I�Ɏ擾
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
        // �v���C���[�ɒǏ]����ꍇ
        if (isFollowing)
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

    // �v���C���[�ɒǏ]���郁�\�b�h
    void FollowPlayer()
    {
        animator.SetBool("isRunning", true);

        // �v���C���[�̕����Ɍ�����
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);  // �����]�����X���[�Y��

        // �v���C���[�Ɍ������Ĉړ�
        transform.position += directionToPlayer * followSpeed * Time.deltaTime;

        // �v���C���[�Ƃ̋��������ȓ��ɋ߂Â������~����
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < 1f)  // �v���C���[�ɐڋ߂����ꍇ
        {
            stopPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
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
        if (other.CompareTag("Player"))
        {
            isFollowing = true;
            animator.SetBool("isRunning", true);
        }
    }

    // �v���C���[���R���C�_�[����o���Ƃ�
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isFollowing = false;
            animator.SetBool("isRunning", false);
        }
    }
}
