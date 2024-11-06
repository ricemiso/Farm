using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string ItemName;
    [SerializeField] string InventryName;
    public bool playerRange;

    [SerializeField] float ditectionRange = 10f;

    public string GetItemName(GameObject objectname)
    {
        switch (objectname.name)
        {
            case "Mana_model":
                ItemName = "�}�i";
                break;
            case "Stone_model":
                ItemName = "�΂���";
                break;
            case "Log_model":
                ItemName = "�ۑ�";
                break;
            case "Mana_model(Clone)":
                ItemName = "�}�i";
                break;
            case "Stone_model(Clone)":
                ItemName = "�΂���";
                break;
            case "Log_model(Clone)":
                ItemName = "�ۑ�";
                break;

        }
        
        return ItemName;
    }

    private void Update()
    {
        //TODO : �����͂����Ŕ��肷�� 
        float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

        if (distance < ditectionRange)
        {
            playerRange = true;
        }
        else
        {
            playerRange = false;
        }


        if (Input.GetKeyDown(KeyCode.Mouse0) && playerRange && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectgameObject == gameObject) 
        {
            if (InventorySystem.Instance.CheckSlotAvailable(1))
            {
                InventorySystem.Instance.AddToinventry(InventryName);

                InventorySystem.Instance.itemsPickedup.Add(gameObject.name);
                print(gameObject.name);

                Destroy(gameObject);
            }
            else
            {
                Debug.Log("inventry is full");
            }
            
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        playerRange = true;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        playerRange = false;
    //    }
    //}
}