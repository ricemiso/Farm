using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choppablecraft : MonoBehaviour
{
    public bool playerRange;
    public bool canBeChopped;
    public bool animcooltime;

    public float MaxHealth;
    public float Health;

    public string craftName;


    public float caloriesSpendChoppingWood;

    private void Start()
    {
        Health = MaxHealth;
        caloriesSpendChoppingWood = 20;
    }

    public string CraftItemName()
    {
        return craftName;
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

    public void GetHit()
    {

        Health -= 1;

        PlayerState.Instance.currentCalories -= caloriesSpendChoppingWood;

        if (Health <= 0)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.treeFallSound);

            Destroy(gameObject);
            canBeChopped = false;
            SelectionManager.Instance.selectedCraft = null;
            SelectionManager.Instance.chopHolder.gameObject.SetActive(false);

        }

    }

    private void Update()
    {
        if (canBeChopped)
        {
            GrobalState.Instance.resourceHelth = Health;
            GrobalState.Instance.resourceMaxHelth = MaxHealth;

        }
    }
}
