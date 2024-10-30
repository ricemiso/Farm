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
	}

	public MoveState state;

	public GameObject player;  // 追従するプレイヤーのTransform
	public float followSpeed = 5f;  // 追従速度
	public float rotateSpeed = 10.0f;   // 回転速度
	protected GameObject target; // ターゲット中の敵

	public bool isStopped = false; // 動きを停止しているかどうかのフラグ

	public float stopRange = 20f;  // 停止できる範囲
	public Color gizmoColor = Color.green;  // Gizmoの色

	const float stopDistance = 10.0f;   // 停止する距離

	public bool onGround;    // 接地しているか
	public const float maxAngleToTreatAsGround = 20.0f; // 地面と判定する傾き

	public float attackRange = 1.0f;	// 攻撃範囲

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

		// ランダムな歩行時間と待機時間を設定
		//walkTime = Random.Range(3, 6);
		//waitTime = Random.Range(5, 7);
		walkTime = Random.Range(1.0f, 3.0f);
		waitTime = Random.Range(2.0f, 4.0f);

		waitCounter = waitTime;
		walkCounter = walkTime;

		state = MoveState.WALKING;

		ChooseDirection();  // 初回の方向を選択

		onGround = false;
	}

	protected virtual void Update()
	{
		// カウンターを減らす
		// Walk()ないだと、接地していないときにカウントが減らなくなり、時間が伸びてしまっていたため
		walkCounter -= Time.deltaTime;
		waitCounter -= Time.deltaTime;


		onGround = false;
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

		// AIキャラクターをプレイヤーの後ろに移動
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
				animation.Stop("Run");
			}

		}
		else
		{
			// 近づく
			transform.position += direction * followSpeed * Time.deltaTime;
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
			animation.Play("Run");
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
		state = MoveState.WAITING;
		transform.position = stopPosition;

		if (animator != null)
		{
			animator.SetBool("isRunning", false);
		}
		else
		{
			animation.Stop("Run");
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
		state = MoveState.WALKING;
		walkCounter = walkTime;
	}

	// 対象に攻撃
	protected void Attack(float num)
	{
		// TODO : 体力管理を一つのクラスに統合させたい

		// タグごとに処理を分岐
		switch (target.tag)
		{
			case "Player":
				PlayerState.Instance.AddHealth(-num);
				break;
			case "Enemy":
				target.GetComponent<Animal>().TakeDamage(num);
				break;
			case "SupportUnit":
				target.GetComponent<Animal>().TakeDamage(num);
				break;
			case "Crystal":
				target.GetComponent<CrystalGrowth>().GetHit();
				break;
			default:
				break;
		}
	}

	protected void OnCollisionStay(Collision collision)
	{
		// 設置判定
		for (int i = 0; i < collision.contactCount; i++)
		{
			if (Vector3.Angle(Vector3.up, collision.GetContact(i).normal)
				< maxAngleToTreatAsGround)
			{
				//Debug.Log("接地");
				onGround = true;
				break;
			}
		}
	}
}
