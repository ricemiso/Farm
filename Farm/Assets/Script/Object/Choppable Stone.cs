using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class ChoppableStone : MonoBehaviour
{
    public bool playerRange;
    public bool canBeChopped;
    public bool animcooltime;

    public float stoneMaxHealth;
    public float stoneHealth;

   //public Animator Animator;

    public float caloriesSpendChoppingWood;

    private void Start()
    {
        stoneHealth = stoneMaxHealth;
        caloriesSpendChoppingWood = 20;
       // Animator = transform.parent.GetComponent<Animator>();
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

        //Animator.SetTrigger("shake");

        stoneHealth -= 1;

        PlayerState.Instance.currentCalories -= caloriesSpendChoppingWood;

        if (stoneHealth <= 0)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.treeFallSound);

            StoneIsDead();

        }

    }

    void StoneIsDead()
    {
        Vector3 treePosition = transform.position;

        Destroy(transform.parent.gameObject);
        canBeChopped = false;
        SelectionManager.Instance.selectedStone = null;
        SelectionManager.Instance.chopHolder.gameObject.SetActive(false);

        GameObject brokenTree = Instantiate(Resources.Load<GameObject>("Stone_model"),
            new Vector3(treePosition.x, treePosition.y + 1, treePosition.z), Quaternion.Euler(0, 0, 0));


        Destroy(brokenTree, 60f);

    }

    private void Update()
    {
        if (canBeChopped)
        {
            GrobalState.Instance.resourceHelth = stoneHealth;
            GrobalState.Instance.resourceMaxHelth = stoneMaxHealth;

        }
    }
}
