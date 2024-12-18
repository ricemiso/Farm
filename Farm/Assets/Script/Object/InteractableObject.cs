using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
	public string ItemName;
	[SerializeField] string InventryName;
	public bool playerRange;
	[SerializeField] bool FastPickup = false;

	[SerializeField] float ditectionRange = 10f;

	public string GetItemName(GameObject objectname)
	{
		switch (objectname.name)
		{
			case "Mana_model":
				ItemName = "マナ";
				break;
			case "Stone_model":
				ItemName = "石ころ";
				break;
			case "Log_model":
				ItemName = "丸太";
				break;
			case "Mana_model(Clone)":
				ItemName = "マナ";
				break;
			case "Stone_model(Clone)":
				ItemName = "石ころ";
				break;
			case "Log_model(Clone)":
				ItemName = "丸太";
				break;

		}

		return ItemName;
	}

	private void Update()
	{

		//TODO : 距離はここで判定する 
		if (PlayerState.Instance.playerBody != null && PlayerState.Instance.playerBody.gameObject != null)
		{
			float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

			if (distance < ditectionRange)
			{
				playerRange = true;
			}
			else
			{
				playerRange = false;
			}
		}


		if (playerRange)
		{
			// FastPickupがtrueか、選択したらクリックしたら
			if ((Input.GetKeyDown(KeyCode.Mouse0) && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectgameObject == gameObject)
				|| FastPickup)
			{
				if (InventorySystem.Instance.CheckSlotAvailable(1))
				{
					InventorySystem.Instance.AddToinventry(InventryName, true);

					InventorySystem.Instance.itemsPickedup.Add(gameObject.name);
					// Debug.Log("Adding item: " + gameObject.name + " to itemList");

					// print(gameObject.name);

					Destroy(gameObject);
				}
				else
				{
					Debug.Log("inventry is full");
				}
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