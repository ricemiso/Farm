using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
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

    public Transform player;  // 追従するプレイヤーのTransform
    public float followSpeed = 5f;  // 追従速度
    private bool isFollowing = false;  // プレイヤーが範囲内にいるかどうか
    private bool isChaseEnemy = false; // 敵を追いかける変数
    private GameObject target; // ターゲット中の敵

    public bool isStopped = false; // 動きを停止しているかどうかのフラグ

    public float stopRange = 20f;  // 停止できる範囲
    public Color gizmoColor = Color.green;  // Gizmoの色

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        // プレイヤーを自動的に取得する
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }

        // ランダムな歩行時間と待機時間を設定
        walkTime = Random.Range(3, 6);
        waitTime = Random.Range(5, 7);

        waitCounter = waitTime;
        walkCounter = walkTime;

        ChooseDirection();  // 初回の方向を選択
    }

    // Update is called once per frame
    void Update()
    {
        // player が null でないことを確認
        if (player == null || this.gameObject==null)
        {
            return;  
        }

        // プレイヤーとの距離を計算
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 一定範囲内かつEキーが押された場合のみ停止・再開を切り替える
        if (distanceToPlayer <= stopRange && Input.GetKeyDown(KeyCode.E))  // Eキーで動作を停止/再開
        {
            isStopped = !isStopped;  // Eキーで動作を停止/再開
            if (isStopped)
            {
                animator.SetBool("isRunning", false);  // アニメーションを停止
            }
        }

        if (isStopped) return;  // 停止中なら処理を中断

        // 敵に追従する場合
        if (isChaseEnemy)
        {
            ChaseEnemy();
        }
        else if (isFollowing)
        {
            FollowPlayer();
        }
        else if (isWalking)  // 通常のランダム歩行
        {
            Walk();
        }
        else  // 歩行を終了し、待機中
        {
            waitCounter -= Time.deltaTime;

            if (waitCounter <= 0)
            {
                ChooseDirection();
            }
        }
    }

    // プレイヤーに後ろから追従するメソッド
    void FollowPlayer()
    {
        animator.SetBool("isRunning", true);

        // プレイヤーの進行方向を取得し、後ろの位置を計算
        Vector3 directionBehindPlayer = -player.forward;  // プレイヤーの後ろ側
        Vector3 followPosition = player.position + directionBehindPlayer * 2f;  // プレイヤーから2ユニット後ろ

        Chase(followPosition);
    }

    void ChaseEnemy()
    {
        animator.SetBool("isRunning", true);

        // プレイヤーの進行方向を取得し、後ろの位置を計算
        Vector3 followPosition = target.transform.position;  // プレイヤーから2ユニット後ろ

        Chase(followPosition);
    }

    // 追いかけるメソッド
    void Chase(Vector3 followPosition)
    {
        // 目的地と現在位置の距離
        float distance = Vector3.Distance(followPosition, transform.position);

        // AIキャラクターをプレイヤーの後ろに移動
        Vector3 directionToFollowPosition = (followPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToFollowPosition);  // AIが向く方向を計算
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);  // 方向転換をスムーズに

        // プレイヤーから離れすぎているか
        if (distance > 10.0f)  // 距離が1.5ユニット以上なら追従を続ける
        {
            transform.position += directionToFollowPosition * followSpeed * Time.deltaTime;
        }
        else
        {
            // プレイヤーの後ろに十分近づいたら停止
            animator.SetBool("isRunning", false);
        }
    }

    // ランダムに歩行するメソッド
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

        // 歩行時間が終わったら待機する
        if (walkCounter <= 0)
        {
            stopPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            isWalking = false;
            transform.position = stopPosition;
            animator.SetBool("isRunning", false);
            waitCounter = waitTime;
        }
    }

    // ランダムな方向を選ぶメソッド
    public void ChooseDirection()
    {
        WalkDirection = Random.Range(0, 4);
        isWalking = true;
        walkCounter = walkTime;
    }

    // プレイヤーがコライダーに入ったとき
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isFollowing)  // プレイヤーが入って、まだ追従していない場合
        {
            isFollowing = true;
            animator.SetBool("isRunning", true);

            target = other.gameObject;
        }

        if (other.CompareTag("Enemy") && !isChaseEnemy)  // 敵が入って、まだ追従していない場合
        {
            isChaseEnemy = true;
            animator.SetBool("isRunning", true);

            target = other.gameObject;
        }
    }

    // プレイヤーがコライダーから出たとき（追従を停止しない）
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 追従停止のコードは削除。プレイヤーが出ても追従を続ける。
        }
    }
}
