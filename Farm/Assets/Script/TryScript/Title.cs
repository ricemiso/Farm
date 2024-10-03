using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public void StartHost()
    {
        // �z�X�g�J�n
        NetworkManager.Singleton.StartHost();

        // �V�[����؂�ւ��i�z�X�g���J�n���ꂽ��Ɏ��s�j
        LoadMainScene();
    }

    public void StartClient()
    {
        // �N���C�A���g�Ƃ��Đڑ�
        NetworkManager.Singleton.StartClient();

        // �V�[����؂�ւ��i�N���C�A���g���ڑ����ꂽ��Ɏ��s�j
        LoadMainScene();
    }

    private void LoadMainScene()
    {
        // ���݂̃V�[������MainScene�֐؂�ւ�
        NetworkManager.Singleton.SceneManager.LoadScene("Online Main", LoadSceneMode.Single);
    }
}