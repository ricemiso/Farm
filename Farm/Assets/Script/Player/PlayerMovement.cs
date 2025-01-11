using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �v���C���[�̈ړ����Ǘ�����N���X�B
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// �L�����N�^�[�R���g���[���[�B
    /// </summary>
    public CharacterController controller;

    /// <summary>
    /// �n�`�f�[�^������Terrain�I�u�W�F�N�g�B
    /// </summary>
    public Terrain terrain;

    /// <summary>
    /// �n�`�f�[�^�B
    /// </summary>
    private TerrainData terrainData;

    /// <summary>
    /// �n�`�̈ʒu�B
    /// </summary>
    private Vector3 terrainPos;

    /// <summary>
    /// ���݂̑����p��AudioSource�B
    /// </summary>
    private AudioSource currentAudioSource;

    /// <summary>
    /// �v���C���[�̈ړ����x�B
    /// </summary>
    public float speed = 12f;

    /// <summary>
    /// �d�́B
    /// </summary>
    public float gravity = -9.81f * 2;

    /// <summary>
    /// �W�����v�̍����B
    /// </summary>
    public float jumpHeight = 3f;

    /// <summary>
    /// �n�ʂ��`�F�b�N���邽�߂�Transform�B
    /// </summary>
    public Transform groundCheck;

    /// <summary>
    /// �n�ʂƂ̋����B
    /// </summary>
    public float groundDistance = 0.4f;

    /// <summary>
    /// �n�ʂ̃��C���[�}�X�N�B
    /// </summary>
    public LayerMask groundMask;

    /// <summary>
    /// �v���C���[�̑��x�B
    /// </summary>
    Vector3 velocity;

    /// <summary>
    /// �v���C���[���n�ʂɂ��邩�ǂ����̃t���O�B
    /// </summary>
    public bool isGrounded;

    /// <summary>
    /// �Ō�̈ʒu�B
    /// </summary>
    private Vector3 lastPosition;

    /// <summary>
    /// �v���C���[���ړ������ǂ����̃t���O�B
    /// </summary>
    public bool isMoving;

    /// <summary>
    /// foundation�ɏ���Ă��邩�̃t���O�B
    /// </summary>
    private bool isOnFoundation = false;

    /// <summary>
    /// �����ݒ���s���܂��B
    /// </summary>
    private void Start()
    {
        lastPosition = new Vector3(0f, 0f, 0f);
        terrainData = terrain.terrainData;
        terrainPos = terrain.transform.position;
        controller = GetComponent<CharacterController>();
    }

    /// <summary>
    /// ���t���[���̍X�V�������s���܂��B
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

        // �ړ������ǂ����̔���
        if (isGrounded && move.magnitude > 0.1f)
        {
            isMoving = true;
            UpdateFootstepSound();
        }
        else
        {
            isMoving = false;

            // ��������~
            if (currentAudioSource != null && currentAudioSource.isPlaying)
            {
                currentAudioSource.Stop();
            }
        }

        lastPosition = gameObject.transform.position;
        PlayerState.Instance.setPlayerPosition(lastPosition);
    }

    /// <summary>
    /// �������X�V���܂��B
    /// </summary>
    private void UpdateFootstepSound()
    {
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

    /// <summary>
    /// �w�肳�ꂽ���C���[�̑������擾���܂��B
    /// </summary>
    /// <param name="layerIndex">���C���[�̃C���f�b�N�X</param>
    /// <returns>AudioSource</returns>
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
            case 2: // ��
                audioSource = SoundManager.Instance.FarmWalkSound;
                break;

            default:
                audioSource = SoundManager.Instance.grassWalkSound;
                break;
        }

        return audioSource;
    }

    /// <summary>
    /// ���݂̒n�`���C���[���擾���܂��B
    /// </summary>
    /// <param name="position">�v���C���[�̈ʒu</param>
    /// <returns>���C���[�̃C���f�b�N�X</returns>
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
