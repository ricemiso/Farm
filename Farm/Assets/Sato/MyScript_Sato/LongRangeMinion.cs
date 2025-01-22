using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeMinion : SupportAI_Movement
{
    [SerializeField]
    [Tooltip("’e‚Ì”­ËêŠ")]
    private GameObject shootPos;

    [SerializeField]
    [Tooltip("’e")]
    private GameObject bullet;

    [SerializeField]
    [Tooltip("’e‚Ì‘¬‚³")]
    private float speed = 30.0f;

    [SerializeField]
    [Tooltip("’e‚Ì¶‘¶ŠÔ")]
    private float lifeTime = 2.0f;

    private bool isCheckingAttack = false;

    // ”ÍˆÍ“à‚É“G‚ª‚¢‚È‚¢ŠÔ
    private float isNotRangeTime; 

    protected override void Start()
    {
        attackRange = 20.0f;
        base.Start();

        target = player;
        state = MoveState.FOLLOWING;

        isNotRangeTime = 0.0f;
    }

    protected override void Update()
    {
        base.Update();

        isNotRangeTime += Time.deltaTime;

		// õ“G”ÍˆÍ“à‚È‚çˆÚ“®‚ğ’â~
		if (isNotRangeTime < 3.0f)
        {
            state = MoveState.STOP;  // ˆÚ“®‚ğ’â~‚·‚é‚½‚ß‚ÉSTOP‚É‚·‚é
        }
        else
        {
            if (isStopped) return;

			if (stopPosition != Vector3.zero)
			{
				state = MoveState.GO_BACK;  // Œ³‚ÌˆÊ’u‚É–ß‚é
			}
            else
			{
				state = MoveState.FOLLOWING;  // ˆÚ“®ó‘Ô‚É–ß‚·
			}
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
        // ’e‚Ì¶¬ˆÊ’u‚Æ‰ñ“]‚ğİ’è
        Vector3 spawnPosition = shootPos.transform.position;
        Quaternion spawnRotation = Quaternion.LookRotation((target.transform.position - spawnPosition).normalized);

        // ’e‚ğ¶¬
        GameObject newBullet = Instantiate(bullet, spawnPosition, spawnRotation);

        // ’e‚Ì”ò‚Ô•ûŒü‚ğŒvZ
        Vector3 direction = (target.transform.position - spawnPosition).normalized;

        // ’e‚É—Í‚ğ‰Á‚¦‚é
        newBullet.GetComponent<Rigidbody>().AddForce(direction * 500.0f, ForceMode.Impulse);

        // ’e‚Ì–¼‘O‚Æõ–½‚ğİ’è
        newBullet.name = bullet.name;
        Destroy(newBullet, lifeTime);

        // ƒ_ƒ[ƒW‚ğİ’è
        float damage = GetComponent<Animal>().damage;
        newBullet.GetComponent<Magic>().SetDamage(damage);
    }

    private void OnTriggerStay(Collider other)
    {
        // "Enemy"ƒ^ƒO‚ÌƒIƒuƒWƒFƒNƒg‚ªÚG‚µ‚½ê‡
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
