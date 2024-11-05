using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertDialogManager : MonoBehaviour
{
    public GameObject dialogBox;
    public Text messageText;
    public Button okBTN;
    public Button cancelBTN;

    private System.Action<bool> responceCallback;

    private void Start()
    {
        dialogBox.SetActive(false);

        okBTN.onClick.AddListener(() => HandleRespose(true));
        cancelBTN.onClick.AddListener(() => HandleRespose(false));
    }

    public void ShowDialog(string message,System.Action<bool> callback)
    {
        responceCallback = callback;
        messageText.text = message;
        dialogBox.SetActive(true);
    }

    private void HandleRespose(bool responce)
    {
        dialogBox.SetActive(false);
        responceCallback?.Invoke(responce);
    }
}
