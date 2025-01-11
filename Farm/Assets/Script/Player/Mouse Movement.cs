using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �}�E�X�̓����𐧌䂷��N���X�B
/// </summary>
public class MouseMovement : MonoBehaviour
{
    /// <summary>
    /// �}�E�X���x�B
    /// </summary>
    public float mouseSensitivity = 100f;

    /// <summary>
    /// X���̉�]�p�x�B
    /// </summary>
    float xRotation = 0f;

    /// <summary>
    /// Y���̉�]�p�x�B
    /// </summary>
    float YRotation = 0f;

    /// <summary>
    /// �����ݒ���s���܂��B
    /// </summary>
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// ���t���[���̍X�V�������s���܂��B
    /// </summary>
    void Update()
    {
        if (!InventorySystem.Instance.isOpen && !CraftingSystem.Instance.isOpen && !MenuManager.Instance.isMenuOpen && !StorageManager.Instance.storageUIOpen)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            YRotation += mouseX;

            transform.localRotation = Quaternion.Euler(xRotation, YRotation, 0f);
        }
    }
}
