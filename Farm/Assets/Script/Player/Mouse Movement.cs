using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//担当者　越浦晃生

/// <summary>
/// マウスの動きを制御するクラス。
/// </summary>
public class MouseMovement : MonoBehaviour
{
    /// <summary>
    /// マウス感度。
    /// </summary>
    public float mouseSensitivity = 100f;

    /// <summary>
    /// X軸の回転角度。
    /// </summary>
    float xRotation = 0f;

    /// <summary>
    /// Y軸の回転角度。
    /// </summary>
    float YRotation = 0f;

    /// <summary>
    /// 初期設定を行います。
    /// </summary>
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// 毎フレームの更新処理を行います。
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
