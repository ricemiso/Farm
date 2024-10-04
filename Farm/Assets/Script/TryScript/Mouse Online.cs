using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class MouseOnline : NetworkBehaviour
{
    public float mouseSensitivity = 100f;

    float xRotation = 0f;
    float yRotation = 0f; // Y�̕ϐ����𓝈�
    private InventorySystem inventorySystem;
    private CraftingSystem craftingSystem;

    private NetworkVariable<float> networkedXRotation = new NetworkVariable<float>();
    private NetworkVariable<float> networkedYRotation = new NetworkVariable<float>();

    void Start()
    {
        if (IsLocalPlayer) // �������g�̃v���C���[�̂ݑ��������
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        // �V�[�������[�h���ꂽ��ɃV���O���g�����擾
        SceneManager.sceneLoaded += OnSceneLoaded;

        // �T�[�o�[���ŏ�����]�p��ݒ�
        if (IsServer)
        {
            InitializeMouseState(); // �T�[�o�[�̂ݏ�����
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���C���V�[�������[�h���ꂽ��V���O���g�����擾
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
        if (!IsLocalPlayer) return; // ���[�J���v���C���[�̂ݑ���

        // �C���x���g����N���t�g���j���[���J���Ă��Ȃ��Ƃ��ɂ̂݃}�E�X�����L���ɂ���
        if (inventorySystem != null && craftingSystem != null && !inventorySystem.isOpen && !craftingSystem.isOpen)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            yRotation += mouseX;

            // �T�[�o�[�Ƀ}�E�X�̉�]��Ԃ𑗐M
            SetMouseRotationServerRpc(xRotation, yRotation);
        }

        // �T�[�o�[����̉�]�����N���C�A���g�Ŏ󂯎��
        if (!IsServer)
        {
            ApplyMouseRotation(networkedXRotation.Value, networkedYRotation.Value);
        }
    }

    private void InitializeMouseState()
    {
        // �}�E�X�̏�����]�p��ݒ�
        networkedXRotation.Value = xRotation;
        networkedYRotation.Value = yRotation;

        // ������]�p��S�N���C�A���g�ɑ��M
        UpdateMouseRotationClientRpc(xRotation, yRotation);
    }

    [ServerRpc]
    private void SetMouseRotationServerRpc(float xRot, float yRot)
    {
        // �T�[�o�[���Ńl�b�g���[�N�ϐ����X�V
        networkedXRotation.Value = xRot;
        networkedYRotation.Value = yRot;

        // �N���C�A���g�ɉ�]��Ԃ𓯊�
        UpdateMouseRotationClientRpc(xRot, yRot);
    }

    [ClientRpc]
    private void UpdateMouseRotationClientRpc(float xRot, float yRot)
    {
        if (!IsOwner) // �����̃I�u�W�F�N�g�ȊO�̉�]���X�V
        {
            ApplyMouseRotation(xRot, yRot);
        }
    }

    private void ApplyMouseRotation(float xRot, float yRot)
    {
        // �N���C�A���g���ŉ�]��K�p
        transform.localRotation = Quaternion.Euler(xRot, yRot, 0f);
    }
}
