using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Movement : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        // プレイヤーを自動的に取得する
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;  // タグが"Player"のオブジェクトを自動的に取得
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
        // プレイヤーに追従する場合
        if (isFollowing)
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

    // プレイヤーに追従するメソッド
    void FollowPlayer()
    {
        animator.SetBool("isRunning", true);

        // プレイヤーの方向に向ける
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);  // 方向転換をスムーズに

        // プレイヤーに向かって移動
        transform.position += directionToPlayer * followSpeed * Time.deltaTime;

        // プレイヤーとの距離が一定以内に近づいたら停止する
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < 1f)  // プレイヤーに接近した場合
        {
            stopPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
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
        if (other.CompareTag("Player"))
        {
            isFollowing = true;
            animator.SetBool("isRunning", true);
        }
    }

    // プレイヤーがコライダーから出たとき
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isFollowing = false;
            animator.SetBool("isRunning", false);
        }
    }
}
