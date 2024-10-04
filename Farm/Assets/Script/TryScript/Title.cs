using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class Title : NetworkBehaviour
{
    private const ushort portNumber = 7777; // �g�p����|�[�g�ԍ�
    private NetworkManager networkManager; // NetworkManager��ێ�����ϐ�

    private void Start()
    {
        // NetworkManager�����݂��邩�m�F
        networkManager = NetworkManager.Singleton;
        if (networkManager == null)
        {
            Debug.LogError("NetworkManager is not initialized. Please ensure it is added to the scene.");
            return; // NetworkManager���Ȃ��ꍇ�͏����𒆒f
        }

        // �N���C�A���g�ڑ��̃R�[���o�b�N��ݒ�
        networkManager.OnClientConnectedCallback += OnClientConnected;
    }

    public void StartHost()
    {
        // �l�b�g���[�N�}�l�[�W���̐ݒ���X�V
        networkManager.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>().SetConnectionData("0.0.0.0", portNumber); // ���b�X���A�h���X�ƃ|�[�g��ݒ�

        // �z�X�g���J�n
        if (networkManager.StartHost())
        {
            Debug.Log("Host started successfully.");
            // �z�X�g������������V�[�������[�h
            // �z�X�g�����g�̂��߂ɃV�[�������[�h����ꍇ�́A�����ōs��
            // �K�v�ɉ����Ēǉ��̃v���C���[�I�u�W�F�N�g�̏������s��
            networkManager.SceneManager.LoadScene("Online Main", LoadSceneMode.Single);
        }
        else
        {
            Debug.LogError("Failed to start host.");
        }
    }

    public void StartClient()
    {
        // �z�X�g��IP�A�h���X��ݒ�
        string hostIp = "127.0.0.1"; // �������g��PC��IP�A�h���X
        networkManager.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>().SetConnectionData(hostIp, portNumber); // �z�X�g�Ɠ����|�[�g���w��

        if (networkManager.StartClient())
        {
            Debug.Log("Client started successfully.");
            // �V�[���J�ڂ� OnClientConnected �ōs��
        }
        else
        {
            Debug.LogError("Failed to start client.");
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        // �N���C�A���g���ڑ����ꂽ�Ƃ��Ƀz�X�g���V�[�������[�h
        Debug.Log("Client connected: " + clientId + ", loading scene...");

        // �z�X�g���ڑ����󂯓��ꂽ�ꍇ�ɂ̂݃V�[�������[�h
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
        // �N���[���A�b�v
        if (networkManager != null)
        {
            networkManager.OnClientConnectedCallback -= OnClientConnected;
        }
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        response.Approved = true;
        response.CreatePlayerObject = true; // �v���C���[�I�u�W�F�N�g�������ō쐬����
        response.Pending = false; // �����ɏ��F
    }
}
