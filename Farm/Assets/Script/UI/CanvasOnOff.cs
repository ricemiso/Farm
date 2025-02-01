using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class CanvasOnOff : MonoBehaviour
{
    [SerializeField] private GameObject canvas;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (canvas != null)
            {
                canvas.SetActive(!canvas.activeSelf);
            }
        }
    }
}
