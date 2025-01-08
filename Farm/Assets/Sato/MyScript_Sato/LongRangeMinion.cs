using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeMinion : SupportAI_Movement
{
    [SerializeField]
    [Tooltip("弾の発射場所")]
    private GameObject shootPos;

    [SerializeField]
    [Tooltip("弾")]
    private GameObject bullet;

    [SerializeField]
    [Tooltip("弾の速さ")]
    private float speed = 30.0f;

    [SerializeField]
    [Tooltip("弾の生存時間")]
    private float lifeTime = 2.0f;

    private bool isCheckingAttack = false;
    private bool isCheckRange = false;  // 索敵範囲内かどうかを判断する変数

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

        // 索敵範囲内なら移動を停止
        if (isCheckRange)
        {
            state = MoveState.STOP;  // 移動を停止するためにSTOPにする
        }
        else
        {
            state = MoveState.FOLLOWING;  // 移動状態に戻す
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
        // "Enemy"タグのオブジェクトが接触した場合
        if (other.CompareTag("Enemy"))
        {
            isCheckRange = true;  // 索敵範囲に入ったので移動を停止
            if (!isCheckingAttack)
            {
                StartCoroutine(CheckAttackWithDelay());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // "Enemy"タグのオブジェクトが範囲外に出た場合
        if (other.CompareTag("Enemy"))
        {
            isCheckRange = false;  // 索敵範囲外に出たので移動再開
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
