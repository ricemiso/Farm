using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class MouseOnline : NetworkBehaviour
{
    public float mouseSensitivity = 100f;

    float xRotation = 0f;
    float yRotation = 0f; // Yの変数名を統一
    private InventorySystem inventorySystem;
    private CraftingSystem craftingSystem;

    private NetworkVariable<float> networkedXRotation = new NetworkVariable<float>();
    private NetworkVariable<float> networkedYRotation = new NetworkVariable<float>();

    void Start()
    {
        if (IsLocalPlayer) // 自分自身のプレイヤーのみ操作を許可
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        // シーンがロードされた後にシングルトンを取得
        SceneManager.sceneLoaded += OnSceneLoaded;

        // サーバー側で初期回転角を設定
        if (IsServer)
        {
            InitializeMouseState(); // サーバーのみ初期化
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // メインシーンがロードされたらシングルトンを取得
        if (scene.name == "Online Main")
        {
            inventorySystem = InventorySystem.Instance;
            if (inventorySystem == null)
            {
                Debug.LogError("InventorySystem instance is null.");
            }

            craftingSystem = CraftingSystem.Instance;
            if (craftingSystem == null)
            {
                Debug.LogError("CraftingSystem instance is null.");
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        if (!IsLocalPlayer) return; // ローカルプレイヤーのみ操作

        // インベントリやクラフトメニューが開いていないときにのみマウス操作を有効にする
        if (inventorySystem != null && craftingSystem != null && !inventorySystem.isOpen && !craftingSystem.isOpen)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            yRotation += mouseX;

            // サーバーにマウスの回転状態を送信
            SetMouseRotationServerRpc(xRotation, yRotation);
        }

        // サーバーからの回転情報をクライアントで受け取る
        if (!IsServer)
        {
            ApplyMouseRotation(networkedXRotation.Value, networkedYRotation.Value);
        }
    }

    private void InitializeMouseState()
    {
        // マウスの初期回転角を設定
        networkedXRotation.Value = xRotation;
        networkedYRotation.Value = yRotation;

        // 初期回転角を全クライアントに送信
        UpdateMouseRotationClientRpc(xRotation, yRotation);
    }

    [ServerRpc]
    private void SetMouseRotationServerRpc(float xRot, float yRot)
    {
        // サーバー側でネットワーク変数を更新
        networkedXRotation.Value = xRot;
        networkedYRotation.Value = yRot;

        // クライアントに回転状態を同期
        UpdateMouseRotationClientRpc(xRot, yRot);
    }

    [ClientRpc]
    private void UpdateMouseRotationClientRpc(float xRot, float yRot)
    {
        if (!IsOwner) // 自分のオブジェクト以外の回転を更新
        {
            ApplyMouseRotation(xRot, yRot);
        }
    }

    private void ApplyMouseRotation(float xRot, float yRot)
    {
        // クライアント側で回転を適用
        transform.localRotation = Quaternion.Euler(xRot, yRot, 0f);
    }
}
