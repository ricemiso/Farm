using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public void StartHost()
    {
        // ホスト開始
        NetworkManager.Singleton.StartHost();

        // シーンを切り替え（ホストが開始された後に実行）
        LoadMainScene();
    }

    public void StartClient()
    {
        // クライアントとして接続
        NetworkManager.Singleton.StartClient();

        // シーンを切り替え（クライアントが接続された後に実行）
        LoadMainScene();
    }

    private void LoadMainScene()
    {
        // 現在のシーンからMainSceneへ切り替え
        NetworkManager.Singleton.SceneManager.LoadScene("Online Main", LoadSceneMode.Single);
    }
}