using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class Title : NetworkBehaviour
{
    private const ushort portNumber = 7777; // 使用するポート番号
    private NetworkManager networkManager; // NetworkManagerを保持する変数

    private void Start()
    {
        // NetworkManagerが存在するか確認
        networkManager = NetworkManager.Singleton;
        if (networkManager == null)
        {
            Debug.LogError("NetworkManager is not initialized. Please ensure it is added to the scene.");
            return; // NetworkManagerがない場合は処理を中断
        }

        // クライアント接続のコールバックを設定
        networkManager.OnClientConnectedCallback += OnClientConnected;
    }

    public void StartHost()
    {
        // ネットワークマネージャの設定を更新
        networkManager.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>().SetConnectionData("0.0.0.0", portNumber); // リッスンアドレスとポートを設定

        // ホストを開始
        if (networkManager.StartHost())
        {
            Debug.Log("Host started successfully.");
            // ホストが成功したらシーンをロード
            // ホストが自身のためにシーンをロードする場合は、ここで行う
            // 必要に応じて追加のプレイヤーオブジェクトの処理を行う
            networkManager.SceneManager.LoadScene("Online Main", LoadSceneMode.Single);
        }
        else
        {
            Debug.LogError("Failed to start host.");
        }
    }

    public void StartClient()
    {
        // ホストのIPアドレスを設定
        string hostIp = "127.0.0.1"; // 自分自身のPCのIPアドレス
        networkManager.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>().SetConnectionData(hostIp, portNumber); // ホストと同じポートを指定

        if (networkManager.StartClient())
        {
            Debug.Log("Client started successfully.");
            // シーン遷移は OnClientConnected で行う
        }
        else
        {
            Debug.LogError("Failed to start client.");
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        // クライアントが接続されたときにホストがシーンをロード
        Debug.Log("Client connected: " + clientId + ", loading scene...");

        // ホストが接続を受け入れた場合にのみシーンをロード
        if (networkManager.IsHost)
        {
            networkManager.SceneManager.LoadScene("Online Main", LoadSceneMode.Single);
        }
        else
        {
            Debug.Log("Client connected, but not the host.");
        }
    }

    private void OnDestroy()
    {
        // クリーンアップ
        if (networkManager != null)
        {
            networkManager.OnClientConnectedCallback -= OnClientConnected;
        }
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        response.Approved = true;
        response.CreatePlayerObject = true; // プレイヤーオブジェクトを自動で作成する
        response.Pending = false; // すぐに承認
    }
}
