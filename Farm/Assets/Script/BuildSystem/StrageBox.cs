using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrageBox : MonoBehaviour
{
    public bool playerInRange;
    [SerializeField] float dis = 10f;

    [SerializeField] public List<string> items;

    public enum BoxType{
        smallBox,
        bigBox
    }

    public BoxType thisboxType;

    private void Update()
    {
        float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

        if (distance < dis)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }
    }
}
