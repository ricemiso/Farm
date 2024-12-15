 using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_Movement : AI_Movement
{
	// 追跡を諦めるまでの時間
	public const float timeToGiveUpChase = 5.0f;
	// 敵を見つけてからの時間（再認識するたびにリセット）
	[SerializeField] public float timeToFoundEnemy;

	// 次攻撃可能になるまでのクールタイム
	[SerializeField] public  float currentAttackCooltime;
	public const float attackCooltime = 1.0f;

	// クリスタルへの参照
	// nullでよい
	public GameObject Crystal;
	// ミニクリスタルへの参照
	// nullでよい
	public GameObject CrystalMini;

	// サポートユニットを見つけた際に追いかけるか
	public bool IsChaseSupportUnit = false;
	// プレイヤーユニットを見つけた際に追いかけるか
	public bool IsChasePlayer = false;




	protected override void Start()
    {
		currentAttackCooltime = 0.0f;
        base.Start();
	}

    // Update is called once per frame
    protected override void Update()
	{

		if (!isStopped)
		{
            switch (state)
            {
                case MoveState.CHASE:
					if (target != null)
					{
						ChaseEnemy();
                    }
                    else
                    {
						ChaseCrystal();
					}
					break;
				default:
					ChaseCrystal();
					break;
            }
		}

		timeToFoundEnemy += Time.deltaTime;
		currentAttackCooltime -= Time.deltaTime;

		base.Update();
    }

	protected void ChaseCrystal()
    {
		if (CrystalMini != null && Crystal != null)
		{
			if (CrystalMini.GetComponent<MiniCrystal>().IsAlive())
			{
				// ミニクリスタルが生きているなら
				FoundTarget(CrystalMini);
			}
			else
			{
				// ミニクリスタルが死んでいたら
				FoundTarget(Crystal);
			}
		}
	}

	virtual protected void ChaseEnemy()
	{

		if(animator != null)
        {
			animator.SetTrigger("Run");
		}
        else
        {
			if(!animation.isPlaying)
			{
				//animation.Stop("run");
				animation.Play("run");
			}


		}

		// 長い時間対象を認識していない場合諦める
		if(timeToFoundEnemy >= timeToGiveUpChase)
		{
			ChangeStateWait();
		}
	}


	// プレイヤーがコライダーに入ったとき
	private void OnTriggerStay(Collider other)
	{
		if(other.CompareTag("SupportUnit") && IsChaseSupportUnit)
		{
			FoundTarget(other.GameObject());
		}

		if(other.CompareTag("Player") && IsChasePlayer)
		{
			FoundTarget(other.GameObject());
		}

	}


	// 敵を発見したときの処理
	protected void FoundTarget(GameObject other)
	{
		state = MoveState.CHASE;

		timeToFoundEnemy = 0.0f;
		target = other;
	}

}
