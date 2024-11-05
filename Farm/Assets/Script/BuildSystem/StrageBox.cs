using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrageBox : MonoBehaviour
{
    public bool playerInRange;
    [SerializeField] float dis = 10f;

    [SerializeField] public List<string> items;

    public Animation animation;
    private int cnt = 0;

    private void Start()
    {
        animation = GetComponent<Animation>();
    }

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


        if (StorageManager.Instance.storageUIOpen)
        {
            
            if(cnt == 0)
            {
                animation.Play("A_SeaChest_Open");
                cnt++;
            }
           
        }
        else if(!StorageManager.Instance.storageUIOpen)
        {
            if(cnt == 1)
            {
                animation.Play("A_SeaChest_Close");
                cnt = 0;
            }
           
        }
    }
}
