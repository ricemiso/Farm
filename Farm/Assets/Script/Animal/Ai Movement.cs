using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Movement : MonoBehaviour
{
	public Animator animator;

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
		WALKING,
		FOLLOWING,
		CHASE,
		WAITING,
	}

	public MoveState state;

	public GameObject player;  // 追従するプレイヤーのTransform
	public float followSpeed = 5f;  // 追従速度
	protected GameObject target; // ターゲット中の敵

	public bool isStopped = false; // 動きを停止しているかどうかのフラグ

	public float stopRange = 20f;  // 停止できる範囲
	public Color gizmoColor = Color.green;  // Gizmoの色

	const float stopDistance = 10.0f;	// 停止する距離

	// Start is called before the first frame update
	protected virtual void Start()
	{
		animator = GetComponent<Animator>();

		// プレイヤーを自動的に取得する
		if (player == null)
		{
			player = GameObject.FindWithTag("Player");  // タグが"Player"のオブジェクトを自動的に取得
		}

		// ランダムな歩行時間と待機時間を設定
		walkTime = Random.Range(3, 6);
		waitTime = Random.Range(5, 7);

		waitCounter = waitTime;
		walkCounter = walkTime;

		state = MoveState.WALKING;

		ChooseDirection();  // 初回の方向を選択
	}


	// 追いかけるメソッド
	// followPosition 追尾する座標
	// isStopTooClose 距離が近い場合止まるか
	protected void Chase(Vector3 followPosition, bool isStopTooClose = false)
	{
		// 目的地と現在位置の距離
		float distance = Vector3.Distance(followPosition, transform.position);

		// AIキャラクターをプレイヤーの後ろに移動
		Vector3 directionToFollowPosition = (followPosition - transform.position).normalized;
		Quaternion targetRotation = Quaternion.LookRotation(directionToFollowPosition);  // AIが向く方向を計算
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);  // 方向転換をスムーズに

		// 距離が近すぎなければ近づく
		if (distance < stopDistance && isStopTooClose)
		{
			// プレイヤーの後ろに十分近づいたら停止
			animator.SetBool("isRunning", false);
		}
		else
		{
			// 近づく
			transform.position += directionToFollowPosition * followSpeed * Time.deltaTime;
		}
	}

	// ランダムに歩行するメソッド
	protected void Walk()
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

		// 歩行時間が終わったら待機する
		if (walkCounter <= 0)
		{
			stopPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			state = MoveState.WAITING;
			transform.position = stopPosition;
			animator.SetBool("isRunning", false);
			waitCounter = waitTime;
		}
	}

	// ランダムな方向を選ぶメソッド
	protected void ChooseDirection()
	{
		WalkDirection = Random.Range(0, 4);
		state = MoveState.WALKING;
		walkCounter = walkTime;
	}

}
