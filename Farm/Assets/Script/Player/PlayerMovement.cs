using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Terrain terrain;
    private TerrainData terrainData;
    private Vector3 terrainPos;
    private AudioSource currentAudioSource; // 現在の足音用のAudioSource

    public GameObject animationModel; // Animationがついているモデル
    private Animation anim; // Animationコンポーネント

    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    public bool isGrounded;
    private Vector3 lastPosition;
    public bool isMoving;

    // foundationに乗っているかのフラグ
    private bool isOnFoundation = false;

    private void Start()
    {
        lastPosition = new Vector3(0f, 0f, 0f);
        terrainData = terrain.terrainData;
        terrainPos = terrain.transform.position;
        controller = GetComponent<CharacterController>();

        // animationModelからAnimationコンポーネントを取得
        anim = animationModel.GetComponent<Animation>();
    }

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

            // Element6のアニメーション（移動アニメーション）を再生
            if (!anim.IsPlaying("Run")) // すでに再生中でない場合のみ
            {
                anim.Play("Run");
            }

            UpdateFootstepSound();
        }
        else
        {
            isMoving = false;

            // アイドル状態では特にアニメーションを再生しない（初期のアニメーションを再生）
            if (anim.IsPlaying("Run"))
            {
                anim.Stop("Run"); // 移動アニメーションを停止
                anim.Play("Idle");
            }

            // 足音も停止
            if (currentAudioSource != null && currentAudioSource.isPlaying)
            {
                currentAudioSource.Stop();
            }
        }

        lastPosition = gameObject.transform.position;
        PlayerState.Instance.setPlayerPosition(lastPosition);
    }

    private void UpdateFootstepSound()
    {
        ////if (isOnFoundation)
        ////{
        ////    PlayFoundationFootstep();
        ////    return;
        ////}
        //else
        //{
            
        //}

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
            case 2: // 枯れ草
                audioSource = SoundManager.Instance.grassWalkSound;
                break;

            default:
                audioSource = SoundManager.Instance.grassWalkSound;
                break;
        }

        return audioSource;
    }

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

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    if (hit.collider.CompareTag("placedFoundation") && !hit.collider.CompareTag("Ground"))
    //    {
    //        isOnFoundation = true;
    //    }
    //    else
    //    {
    //        isOnFoundation = false;
    //    }
    //}

    //private void PlayFoundationFootstep()
    //{
    //    if (SoundManager.Instance.foundationWalkSound == null)
    //    {
    //        Debug.LogError("foundationWalkSound is not set in SoundManager.");
    //        return;
    //    }

    //    if (currentAudioSource != null && currentAudioSource.isPlaying)
    //    {
    //        currentAudioSource.Stop();
    //    }

    //    currentAudioSource = SoundManager.Instance.foundationWalkSound;
    //    Debug.Log("Assigned foundationWalkSound to currentAudioSource.");

    //    if (currentAudioSource.clip == null)
    //    {
    //        Debug.LogError("No AudioClip assigned to foundationWalkSound.");
    //    }
    //    else
    //    {
    //        Debug.Log("Assigned AudioClip: " + currentAudioSource.clip.name);
    //    }

    //    currentAudioSource.loop = true;
    //    currentAudioSource.Play();
    //    Debug.Log("Playing foundationWalkSound.");
    //}
}
