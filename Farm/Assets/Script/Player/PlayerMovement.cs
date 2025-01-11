using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//担当者　越浦晃生

/// <summary>
/// プレイヤーの移動を管理するクラス。
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// キャラクターコントローラー。
    /// </summary>
    public CharacterController controller;

    /// <summary>
    /// 地形データを持つTerrainオブジェクト。
    /// </summary>
    public Terrain terrain;

    /// <summary>
    /// 地形データ。
    /// </summary>
    private TerrainData terrainData;

    /// <summary>
    /// 地形の位置。
    /// </summary>
    private Vector3 terrainPos;

    /// <summary>
    /// 現在の足音用のAudioSource。
    /// </summary>
    private AudioSource currentAudioSource;

    /// <summary>
    /// プレイヤーの移動速度。
    /// </summary>
    public float speed = 12f;

    /// <summary>
    /// 重力。
    /// </summary>
    public float gravity = -9.81f * 2;

    /// <summary>
    /// ジャンプの高さ。
    /// </summary>
    public float jumpHeight = 3f;

    /// <summary>
    /// 地面をチェックするためのTransform。
    /// </summary>
    public Transform groundCheck;

    /// <summary>
    /// 地面との距離。
    /// </summary>
    public float groundDistance = 0.4f;

    /// <summary>
    /// 地面のレイヤーマスク。
    /// </summary>
    public LayerMask groundMask;

    /// <summary>
    /// プレイヤーの速度。
    /// </summary>
    Vector3 velocity;

    /// <summary>
    /// プレイヤーが地面にいるかどうかのフラグ。
    /// </summary>
    public bool isGrounded;

    /// <summary>
    /// 最後の位置。
    /// </summary>
    private Vector3 lastPosition;

    /// <summary>
    /// プレイヤーが移動中かどうかのフラグ。
    /// </summary>
    public bool isMoving;

    /// <summary>
    /// foundationに乗っているかのフラグ。
    /// </summary>
    private bool isOnFoundation = false;

    /// <summary>
    /// 初期設定を行います。
    /// </summary>
    private void Start()
    {
        lastPosition = new Vector3(0f, 0f, 0f);
        terrainData = terrain.terrainData;
        terrainPos = terrain.transform.position;
        controller = GetComponent<CharacterController>();
    }

    /// <summary>
    /// 毎フレームの更新処理を行います。
    /// </summary>
    void Update()
    {
        if (StorageManager.Instance.storageUIOpen == true) return;

        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        float speedRate = PlayerState.Instance.getPlayerSpeedRate();
        controller.Move(move * speed * speedRate * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // 移動中かどうかの判定
        if (isGrounded && move.magnitude > 0.1f)
        {
            isMoving = true;
            UpdateFootstepSound();
        }
        else
        {
            isMoving = false;

            // 足音も停止
            if (currentAudioSource != null && currentAudioSource.isPlaying)
            {
                currentAudioSource.Stop();
            }
        }

        lastPosition = gameObject.transform.position;
        PlayerState.Instance.setPlayerPosition(lastPosition);
    }

    /// <summary>
    /// 足音を更新します。
    /// </summary>
    private void UpdateFootstepSound()
    {
        // 通常の地形ごとの足音を再生
        Vector3 playerPosition = transform.position;
        int layerIndex = GetCurrentTerrainLayer(playerPosition);
        AudioSource newAudioSource = GetFootstepSoundForLayer(layerIndex);

        if (newAudioSource != currentAudioSource)
        {
            if (currentAudioSource != null && currentAudioSource.isPlaying)
            {
                currentAudioSource.Stop();
            }

            currentAudioSource = newAudioSource;
            currentAudioSource.loop = true;
            currentAudioSource.Play();
        }
        else if (!currentAudioSource.isPlaying)
        {
            currentAudioSource.Play();
        }
    }

    /// <summary>
    /// 指定されたレイヤーの足音を取得します。
    /// </summary>
    /// <param name="layerIndex">レイヤーのインデックス</param>
    /// <returns>AudioSource</returns>
    private AudioSource GetFootstepSoundForLayer(int layerIndex)
    {
        AudioSource audioSource;

        switch (layerIndex)
        {
            case 0: // 草
                audioSource = SoundManager.Instance.grassWalkSound;
                break;
            case 1: // 砂利
                audioSource = SoundManager.Instance.gravelWalkSound;
                break;
            case 2: // 畑
                audioSource = SoundManager.Instance.FarmWalkSound;
                break;

            default:
                audioSource = SoundManager.Instance.grassWalkSound;
                break;
        }

        return audioSource;
    }

    /// <summary>
    /// 現在の地形レイヤーを取得します。
    /// </summary>
    /// <param name="position">プレイヤーの位置</param>
    /// <returns>レイヤーのインデックス</returns>
    private int GetCurrentTerrainLayer(Vector3 position)
    {
        TerrainData terrainData = terrain.terrainData;
        float[,,] splatmapData = terrainData.GetAlphamaps(
            (int)((position.x / terrainData.size.x) * terrainData.alphamapWidth),
            (int)((position.z / terrainData.size.z) * terrainData.alphamapHeight),
            1, 1);

        int maxTextureIndex = 0;
        float maxAlpha = 0f;

        for (int i = 0; i < splatmapData.GetLength(2); i++)
        {
            float alpha = splatmapData[0, 0, i];

            if (alpha > maxAlpha)
            {
                maxAlpha = alpha;
                maxTextureIndex = i;
            }
        }

        return maxTextureIndex;
    }
}
