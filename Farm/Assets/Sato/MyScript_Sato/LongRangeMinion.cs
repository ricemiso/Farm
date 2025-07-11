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
    private float speed = 200.0f;

    [SerializeField]
    [Tooltip("弾の生存時間")]
    private float lifeTime = 2.0f;

    private bool isCheckingAttack = false;

    // 範囲内に敵がいない時間
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
        
        // 子クラス側の処理を先に行う
        isNotRangeTime += Time.deltaTime;

        

        // 索敵範囲内なら移動を停止
        if (isNotRangeTime < 3.0f && target != null && target.transform.GetComponent<Animal>() != null && !target.transform.GetComponent<Animal>().isDead)
        {
            state = MoveState.STOP;  // 移動を停止するためにSTOPにする
        }
        else
        {
            
            if (stopPosition != Vector3.zero)
            {
                state = MoveState.GO_BACK;  // 元の位置に戻る
            }
            else if(!isStopped)
            {
                state = MoveState.FOLLOWING;  // 移動状態に戻す
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
		//自身の向きを敵に向ける
		this.transform.LookAt(target.transform);

		// 弾の生成位置と回転を設定
		Vector3 spawnPosition = shootPos.transform.position;
        Quaternion spawnRotation = Quaternion.LookRotation((target.transform.position - spawnPosition).normalized);

		// 弾を生成
		GameObject newBullet = Instantiate(bullet, spawnPosition, spawnRotation);

        // 弾の飛ぶ方向を計算
        Vector3 direction = (target.transform.position - spawnPosition).normalized;
        direction.y = 0;

		// 弾に力を加える
		newBullet.GetComponent<Rigidbody>().AddForce(direction * speed, ForceMode.Impulse);

        // 弾の名前と寿命を設定
        newBullet.name = bullet.name;
        Destroy(newBullet, lifeTime);

        // ダメージを設定
        float damage = GetComponent<Animal>().damage;
        newBullet.GetComponent<Magic>().SetDamage(damage);
    }

    private void OnTriggerStay(Collider other)
    {
        // "Enemy"タグのオブジェクトが接触した場合
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
