using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{

    public float mouseSensitivity = 100f;

    float xRotation = 0f;
    float YRotation = 0f;

    void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
    }

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