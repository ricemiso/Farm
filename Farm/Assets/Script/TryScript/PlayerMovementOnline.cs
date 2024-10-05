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

    // �l�b�g���[�N�����p�̃v���C���[�̃|�W�V�����ƈړ����
    private NetworkVariable<Vector3> networkedPosition = new NetworkVariable<Vector3>(new Vector3(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<bool> isMovingNetwork = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<bool> isJumpingNetwork = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private SoundManager soundManager;
    private Camera playerCamera;

    private void Start()
    {
        if (IsLocalPlayer) // �������g�̃v���C���[�݈̂ړ�������
        {
            controller = GetComponent<CharacterController>();
            playerCamera = GetComponentInChildren<Camera>();
            if (playerCamera != null)
            {
                playerCamera.gameObject.SetActive(true);
            }
        }
        else
        {
            playerCamera = GetComponentInChildren<Camera>();
            if (playerCamera != null)
            {
                playerCamera.gameObject.SetActive(false);
            }
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        // �v���C���[�̏�Ԃ�������
        if (IsServer)
        {
            InitializePlayerState(); // �T�[�o�[�̂ݏ�����
        }
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
        if (!IsOwner) return; // �v���C���[�̃I�u�W�F�N�g�̃I�[�i�[�������ړ��������s��

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

        // �W�����v����
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            SetJumpStateServerRpc(true);
        }

        // �W�����v�I�������o
        if (isGrounded && velocity.y <= 0)
        {
            SetJumpStateServerRpc(false);
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

        // �N���C�A���g�ŃT�[�o�[����̈ʒu���󂯎��
        if (!IsServer)
        {
            transform.position = networkedPosition.Value;
        }

        // �����̍Đ���Ԃ��X�V
        UpdateFootstepSound(isMovingNow && !isJumpingNetwork.Value);
    }

    private void InitializePlayerState()
    {
        // �v���C���[�̏����ʒu��ݒ�
        networkedPosition.Value = transform.position;
        isMovingNetwork.Value = false;
        isJumpingNetwork.Value = false;

        // �����ʒu��S�N���C�A���g�ɑ��M
        UpdateClientPositionClientRpc(transform.position, false);
    }

    [ServerRpc]
    private void SetMoveInputServerRpc(float x, float z, bool moving)
    {
        Vector3 move = new Vector3(x, 0, z);
        transform.position += move * speed * Time.deltaTime;

        // �T�[�o�[���ł̂݃l�b�g���[�N�ϐ����X�V
        networkedPosition.Value = transform.position;
        isMovingNetwork.Value = moving; // �T�[�o�[���ňړ���Ԃ��X�V

        // �N���C�A���g�Ɉʒu�ƈړ���Ԃ𓯊�
        UpdateClientPositionClientRpc(transform.position, moving);
    }

    [ServerRpc]
    private void SetJumpStateServerRpc(bool isJumping)
    {
        // �T�[�o�[���ŃW�����v��Ԃ��X�V
        isJumpingNetwork.Value = isJumping;

        // �N���C�A���g�ɃW�����v��Ԃ𓯊�
        UpdateJumpStateClientRpc(isJumping);
    }

    [ClientRpc]
    private void UpdateClientPositionClientRpc(Vector3 newPosition, bool moving)
    {
        if (!IsOwner) // �����̃I�u�W�F�N�g�ȊO�̈ʒu���X�V
        {
            transform.position = newPosition; // �N���C�A���g�ł�NetworkVariable�ɂ͏������܂Ȃ�
        }
    }

    [ClientRpc]
    private void UpdateJumpStateClientRpc(bool isJumping)
    {
        isJumpingNetwork.Value = isJumping; // �W�����v��Ԃ𓯊�
    }

    private void UpdateFootstepSound(bool shouldPlaySound)
    {
        if (soundManager == null) return;

        if (shouldPlaySound)
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

        float normalizedX = (position.x - terrain.transform.position.x) / terrainData.size.x;
        float normalizedZ = (position.z - terrain.transform.position.z) / terrainData.size.z;

        int xIndex = Mathf.Clamp((int)(normalizedX * terrainData.alphamapWidth), 0, terrainData.alphamapWidth - 1);
        int zIndex = Mathf.Clamp((int)(normalizedZ * terrainData.alphamapHeight), 0, terrainData.alphamapHeight - 1);

        float[,,] alphaMap = terrainData.GetAlphamaps(xIndex, zIndex, 1, 1);

        int maxLayerIndex = 0;
        float maxValue = alphaMap[0, 0, 0];

        for (int i = 1; i < alphaMap.GetLength(2); i++)
        {
            if (alphaMap[0, 0, i] > maxValue)
            {
                maxValue = alphaMap[0, 0, i];
                maxLayerIndex = i;
            }
        }

        return maxLayerIndex;
    }
}
