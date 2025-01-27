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
    private float speed = 200.0f;

    [SerializeField]
    [Tooltip("�e�̐�������")]
    private float lifeTime = 2.0f;

    private bool isCheckingAttack = false;

    // �͈͓��ɓG�����Ȃ�����
    private float isNotRangeTime;

    private float attackCheckTime;

    protected override void Start()
    {
        attackRange = 20.0f;
        base.Start();

        target = null;
        state = MoveState.STOP;

        isNotRangeTime = 0.0f;
    }

    protected override void Update()
    {
        
        // �q�N���X���̏������ɍs��
        isNotRangeTime += Time.deltaTime;

        

        // ���G�͈͓��Ȃ�ړ����~
        if (isNotRangeTime < 3.0f && target != null && target.transform.GetComponent<Animal>() != null && !target.transform.GetComponent<Animal>().isDead)
        {
            state = MoveState.STOP;  // �ړ����~���邽�߂�STOP�ɂ���
        }
        else
        {
            
            if (stopPosition != Vector3.zero)
            {
                state = MoveState.GO_BACK;  // ���̈ʒu�ɖ߂�
            }
            else if(!isStopped)
            {
                state = MoveState.FOLLOWING;  // �ړ���Ԃɖ߂�
            }
           
        }

        base.Update();
    }



    protected override void checkAttack()
    {
        
        if (target == null || target.tag == "Player" || target.transform == null || 
            gameObject.transform.parent.name == ConstructionManager.Instance.constructionHoldingSpot.name)
        {
            target = null;
            return;
        }


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
        // �e�̐����ʒu�Ɖ�]��ݒ�
        Vector3 spawnPosition = shootPos.transform.position;
        Quaternion spawnRotation = Quaternion.LookRotation((target.transform.position - spawnPosition).normalized);

        // �e�𐶐�
        GameObject newBullet = Instantiate(bullet, spawnPosition, spawnRotation);

        // �e�̔�ԕ������v�Z
        Vector3 direction = (target.transform.position - spawnPosition).normalized;

        // �e�ɗ͂�������
        newBullet.GetComponent<Rigidbody>().AddForce(direction * speed, ForceMode.Impulse);

        // �e�̖��O�Ǝ�����ݒ�
        newBullet.name = bullet.name;
        Destroy(newBullet, lifeTime);

        // �_���[�W��ݒ�
        float damage = GetComponent<Animal>().damage;
        newBullet.GetComponent<Magic>().SetDamage(damage);
    }

    private void OnTriggerStay(Collider other)
    {
        // "Enemy"�^�O�̃I�u�W�F�N�g���ڐG�����ꍇ
        if (other.CompareTag("Enemy"))
        {
            isNotRangeTime = 0.0f;
            if (!isCheckingAttack)
            {
                StartCoroutine(CheckAttackWithDelay());
            }
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
