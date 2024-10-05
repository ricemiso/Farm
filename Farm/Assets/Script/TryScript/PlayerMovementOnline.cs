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

    // ネットワーク同期用のプレイヤーのポジションと移動状態
    private NetworkVariable<Vector3> networkedPosition = new NetworkVariable<Vector3>(new Vector3(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<bool> isMovingNetwork = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<bool> isJumpingNetwork = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private SoundManager soundManager;
    private Camera playerCamera;

    private void Start()
    {
        if (IsLocalPlayer) // 自分自身のプレイヤーのみ移動を処理
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

        // プレイヤーの状態を初期化
        if (IsServer)
        {
            InitializePlayerState(); // サーバーのみ初期化
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Terrainの取得
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

        // SoundManagerシングルトンの取得
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
        if (!IsOwner) return; // プレイヤーのオブジェクトのオーナーだけが移動処理を行う

        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 入力処理
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // ジャンプ処理
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            SetJumpStateServerRpc(true);
        }

        // ジャンプ終了を検出
        if (isGrounded && velocity.y <= 0)
        {
            SetJumpStateServerRpc(false);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // サーバーに移動の状態を送信
        bool isMovingNow = move.magnitude > 0;
        if (isMovingNow != isMoving)
        {
            isMoving = isMovingNow;
            SetMoveInputServerRpc(x, z, isMovingNow);
        }

        // クライアントでサーバーからの位置を受け取る
        if (!IsServer)
        {
            transform.position = networkedPosition.Value;
        }

        // 足音の再生状態を更新
        UpdateFootstepSound(isMovingNow && !isJumpingNetwork.Value);
    }

    private void InitializePlayerState()
    {
        // プレイヤーの初期位置を設定
        networkedPosition.Value = transform.position;
        isMovingNetwork.Value = false;
        isJumpingNetwork.Value = false;

        // 初期位置を全クライアントに送信
        UpdateClientPositionClientRpc(transform.position, false);
    }

    [ServerRpc]
    private void SetMoveInputServerRpc(float x, float z, bool moving)
    {
        Vector3 move = new Vector3(x, 0, z);
        transform.position += move * speed * Time.deltaTime;

        // サーバー側でのみネットワーク変数を更新
        networkedPosition.Value = transform.position;
        isMovingNetwork.Value = moving; // サーバー側で移動状態も更新

        // クライアントに位置と移動状態を同期
        UpdateClientPositionClientRpc(transform.position, moving);
    }

    [ServerRpc]
    private void SetJumpStateServerRpc(bool isJumping)
    {
        // サーバー側でジャンプ状態を更新
        isJumpingNetwork.Value = isJumping;

        // クライアントにジャンプ状態を同期
        UpdateJumpStateClientRpc(isJumping);
    }

    [ClientRpc]
    private void UpdateClientPositionClientRpc(Vector3 newPosition, bool moving)
    {
        if (!IsOwner) // 自分のオブジェクト以外の位置を更新
        {
            transform.position = newPosition; // クライアントではNetworkVariableには書き込まない
        }
    }

    [ClientRpc]
    private void UpdateJumpStateClientRpc(bool isJumping)
    {
        isJumpingNetwork.Value = isJumping; // ジャンプ状態を同期
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
            case 0: // 草
                audioSource = soundManager.grassWalkSound;
                break;
            case 1: // 砂利
                audioSource = soundManager.gravelWalkSound;
                break;
            case 2: // 枯れ草
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
