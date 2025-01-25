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

	// 状態リスト
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

	public GameObject player;  // 追従するプレイヤーのTransform
	public GameObject tankMinion = null;
	public float followSpeed = 5f;  // 追従速度
	public float rotateSpeed = 10.0f;   // 回転速度
	public GameObject target; // ターゲット中の敵

	public bool isStopped = false; // 動きを停止しているかどうかのフラグ

	public float stopRange = 20f;  // 停止できる範囲
	public Color gizmoColor = Color.green;  // Gizmoの色

	[SerializeField]public float stopDistance = 10.0f;   // 停止する距離

	public const float maxAngleToTreatAsGround = 20.0f; // 地面と判定する傾き

	public float attackRange = 1.0f;    // 攻撃範囲

	// Start is called before the first frame update
	protected virtual void Start()
	{
		animator = GetComponent<Animator>();

		if (animator == null)
		{
			animation = GetComponent<Animation>();
		}

		// プレイヤーを自動的に取得する
		if (player == null)
		{
			player = GameObject.FindWithTag("Player");  // タグが"Player"のオブジェクトを自動的に取得
		}

		if(tankMinion == null)
        {
			tankMinion = GameObject.Find("TankAI2");
        }

		// ランダムな歩行時間と待機時間を設定
		//walkTime = Random.Range(3, 6);
		//waitTime = Random.Range(5, 7);
		walkTime = Random.Range(1.0f, 3.0f);
		waitTime = Random.Range(2.0f, 4.0f);

		waitCounter = waitTime;
		walkCounter = walkTime;

		state = MoveState.STOP;
		


		ChooseDirection();  // 初回の方向を選択

	}

	protected virtual void Update()
	{
		// カウンターを減らす
		// Walk()ないだと、接地していないときにカウントが減らなくなり、時間が伸びてしまっていたため
		walkCounter -= Time.deltaTime;
		waitCounter -= Time.deltaTime;


	}

	// 追いかけるメソッド
	// followPosition 追尾する座標
	// isStopTooClose 距離が近い場合止まるか
	protected void Chase(Vector3 followPosition, bool isStopTooClose = false)
	{
		// 高さをそろえる
		followPosition.y = transform.position.y;

		// 目的地と現在位置の距離
		float distance = Vector3.Distance(followPosition, transform.position);

		Vector3 direction = (followPosition - transform.position).normalized;
		Quaternion targetRotation = Quaternion.LookRotation(direction);  // AIが向く方向を計算
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);  // 方向転換をスムーズに

		// 距離が近すぎなければ近づく
		if (distance < stopDistance && isStopTooClose)
		{
			// プレイヤーの後ろに十分近づいたら停止
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
			// 近づく
 			transform.position += direction * followSpeed * Time.deltaTime;

			// アニメーション
			if (animator != null)
			{
				animator.SetBool("isRunning", true);
			}
		}
	}

	// ランダムに歩行するメソッド
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

		// 歩行時間が終わったら待機する
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

	// 待機するメソッド
	protected void Wait()
	{
		if (waitCounter <= 0)
		{
			ChooseDirection();
		}
	}

	// ランダムな方向を選ぶメソッド
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

	// 対象に攻撃
	public void Attack(float num, GameObject obj = null)
	{
		// TODO : 体力管理を一つのクラスに統合させたい

		// タグごとに処理を分岐
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
	//	// 設置判定
	//	for (int i = 0; i < collision.contactCount; i++)
	//	{
	//		if (Vector3.Angle(Vector3.up, collision.GetContact(i).normal)
	//			< maxAngleToTreatAsGround)
	//		{
	//			//Debug.Log("接地");
	//			onGround = true;
	//			break;
	//		}
	//	}
	//}
}
