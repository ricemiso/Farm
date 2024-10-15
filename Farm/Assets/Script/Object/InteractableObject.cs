using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string ItemName;
    public bool playerRange;

    public string GetItemName()
    {
        return ItemName;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && playerRange && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectgameObject == gameObject) 
        {
            if (InventorySystem.Instance.CheckSlotAvailable(1))
            {
                InventorySystem.Instance.AddToinventry(ItemName);

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


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRange = false;
        }
    }
}