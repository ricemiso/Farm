using System.Collections;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class PlayerMovementOnline : NetworkBehaviour
{
    public CharacterController controller;

    public Terrain terrain;
    private TerrainData terrainData;
    private Vector3 terrainPos;
    private AudioSource currentAudioSource;

    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    public bool isGrounded;
    public bool isMoving;
    private Vector2 m_moveInput = Vector2.zero;

    // �l�b�g���[�N�����p�̃v���C���[�̃|�W�V����
    private NetworkVariable<Vector3> networkedPosition = new NetworkVariable<Vector3>();
    private NetworkVariable<bool> isMovingNetwork = new NetworkVariable<bool>();

    private SoundManager soundManager;

    private void Start()
    {
        if (IsLocalPlayer) // �������g�̃v���C���[�݈̂ړ�������
        {
            controller = GetComponent<CharacterController>();
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Terrain�̎擾
        if (terrain == null)
        {
            GameObject terrainObject = GameObject.Find("Terrain");
            if (terrainObject != null)
            {
                terrain = terrainObject.GetComponent<Terrain>();
            }
            else
            {
                Debug.LogError("Terrain object not found in the loaded scene.");
            }

            terrainData = terrain.terrainData;
            terrainPos = terrain.transform.position;
        }

        // SoundManager�V���O���g���̎擾
        if (SoundManager.Instance == null)
        {
            Debug.LogError("SoundManager singleton instance not found in the loaded scene.");
        }
        else
        {
            soundManager = SoundManager.Instance;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (!IsOwner) return;

        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // ���͏���
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // �T�[�o�[�Ɉړ��̏�Ԃ𑗐M
        bool isMovingNow = move.magnitude > 0;
        if (isMovingNow != isMoving)
        {
            isMoving = isMovingNow;
            SetMoveInputServerRpc(x, z, isMovingNow);
        }

        // �v���C���[�̈ʒu�����l�b�g���[�N�ϐ��ɔ��f
        networkedPosition.Value = transform.position;
    }

    [ServerRpc]
    private void SetMoveInputServerRpc(float x, float z, bool moving)
    {
        Vector3 move = new Vector3(x, 0, z);
        transform.position += move * speed * Time.deltaTime;

        // �N���C�A���g�Ɉʒu�ƈړ���Ԃ𓯊�
        UpdateClientPositionClientRpc(transform.position, moving);
    }

    [ClientRpc]
    private void UpdateClientPositionClientRpc(Vector3 newPosition, bool moving)
    {
        if (!IsOwner)
        {
            transform.position = newPosition;
        }
        isMovingNetwork.Value = moving;
        UpdateFootstepSound(moving);
    }

    private void UpdateFootstepSound(bool moving)
    {
        if (soundManager == null) return; // SoundManager���܂��擾����Ă��Ȃ��ꍇ�͏������X�L�b�v

        if (moving)
        {
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
        else if (currentAudioSource != null && currentAudioSource.isPlaying)
        {
            currentAudioSource.Stop();
        }
    }

    private AudioSource GetFootstepSoundForLayer(int layerIndex)
    {
        AudioSource audioSource;

        switch (layerIndex)
        {
            case 0: // ��
                audioSource = soundManager.grassWalkSound;
                break;
            case 1: // ����
                audioSource = soundManager.gravelWalkSound;
                break;
            case 2: // �͂ꑐ
                audioSource = soundManager.grassWalkSound;
                break;

            default:
                audioSource = soundManager.grassWalkSound;
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
}
