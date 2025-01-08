using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeMinion : SupportAI_Movement
{
    [SerializeField]
    [Tooltip("�e�̔��ˏꏊ")]
    private GameObject shootPos;

    [SerializeField]
    [Tooltip("�e")]
    private GameObject bullet;

    [SerializeField]
    [Tooltip("�e�̑���")]
    private float speed = 30.0f;

    [SerializeField]
    [Tooltip("�e�̐�������")]
    private float lifeTime = 2.0f;

    private bool isCheckingAttack = false;
    private bool isCheckRange = false;  // ���G�͈͓����ǂ����𔻒f����ϐ�

    protected override void Start()
    {
        attackRange = 20.0f;
        base.Start();

        target = player;
        state = MoveState.FOLLOWING;
    }

    protected override void Update()
    {
        base.Update();

        // ���G�͈͓��Ȃ�ړ����~
        if (isCheckRange)
        {
            state = MoveState.STOP;  // �ړ����~���邽�߂�STOP�ɂ���
        }
        else
        {
            state = MoveState.FOLLOWING;  // �ړ���Ԃɖ߂�
        }
    }

    protected override void checkAttack()
    {
        if (target.tag == "Player") return;

        animation["Attack2"].speed = 1.7f;
        animation.Play("Attack2");
        StartCoroutine(createDelay());
    }

    IEnumerator createDelay()
    {
        yield return new WaitForSeconds(0.2f);
        createMagic();
    }

    public void createMagic()
    {
        Vector3 spawnPosition = shootPos.transform.position + shootPos.transform.forward;
        Quaternion spawnRotation = shootPos.transform.rotation;

        GameObject newBullet = Instantiate(bullet, spawnPosition, spawnRotation);

        Vector3 direction = newBullet.transform.forward;
        newBullet.GetComponent<Rigidbody>().AddForce(direction * 500.0f, ForceMode.Impulse);
        newBullet.name = bullet.name;
        Destroy(newBullet, lifeTime);
        float damage = GetComponent<Animal>().damage;
        newBullet.GetComponent<Magic>().SetDamage(damage);
    }

    private void OnTriggerStay(Collider other)
    {
        // "Enemy"�^�O�̃I�u�W�F�N�g���ڐG�����ꍇ
        if (other.CompareTag("Enemy"))
        {
            isCheckRange = true;  // ���G�͈͂ɓ������̂ňړ����~
            if (!isCheckingAttack)
            {
                StartCoroutine(CheckAttackWithDelay());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // "Enemy"�^�O�̃I�u�W�F�N�g���͈͊O�ɏo���ꍇ
        if (other.CompareTag("Enemy"))
        {
            isCheckRange = false;  // ���G�͈͊O�ɏo���̂ňړ��ĊJ
        }
    }

    private IEnumerator CheckAttackWithDelay()
    {
        isCheckingAttack = true;

        yield return new WaitForSeconds(2.0f);

        checkAttack();

        isCheckingAttack = false;
    }
}
