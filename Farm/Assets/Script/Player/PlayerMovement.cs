using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Terrain terrain;
    private TerrainData terrainData;
    private Vector3 terrainPos;
    private AudioSource currentAudioSource; // ���݂̑����p��AudioSource

    public GameObject animationModel; // Animation�����Ă��郂�f��
    private Animation anim; // Animation�R���|�[�l���g

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

    // foundation�ɏ���Ă��邩�̃t���O
    private bool isOnFoundation = false;

    private void Start()
    {
        lastPosition = new Vector3(0f, 0f, 0f);
        terrainData = terrain.terrainData;
        terrainPos = terrain.transform.position;
        controller = GetComponent<CharacterController>();

        // animationModel����Animation�R���|�[�l���g���擾
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

        // �ړ������ǂ����̔���
        if (isGrounded && move.magnitude > 0.1f)
        {
            isMoving = true;

            // Element6�̃A�j���[�V�����i�ړ��A�j���[�V�����j���Đ�
            if (!anim.IsPlaying("Run")) // ���łɍĐ����łȂ��ꍇ�̂�
            {
                anim.Play("Run");
            }

            UpdateFootstepSound();
        }
        else
        {
            isMoving = false;

            // �A�C�h����Ԃł͓��ɃA�j���[�V�������Đ����Ȃ��i�����̃A�j���[�V�������Đ��j
            if (anim.IsPlaying("Run"))
            {
                anim.Stop("Run"); // �ړ��A�j���[�V�������~
                anim.Play("Idle");
            }

            // ��������~
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

        // �ʏ�̒n�`���Ƃ̑������Đ�
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
            case 0: // ��
                audioSource = SoundManager.Instance.grassWalkSound;
                break;
            case 1: // ����
                audioSource = SoundManager.Instance.gravelWalkSound;
                break;
            case 2: // �͂ꑐ
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
