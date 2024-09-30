using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class ChoppableTree : MonoBehaviour
{


    public bool playerRange;
    public bool canBeChopped;
    public bool animcooltime;

    public float treeMaxHealth;
    public float treeHealth;

    public Animator Animator;

    public float caloriesSpendChoppingWood;

    private void Start()
    {
        treeHealth = treeMaxHealth;
        caloriesSpendChoppingWood = 20;
        Animator = transform.parent.transform.parent.GetComponent<Animator>();
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

        Animator.SetTrigger("shake");

        treeHealth -= 1;

        PlayerState.Instance.currentCalories -= caloriesSpendChoppingWood;

        if (treeHealth <= 0)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.treeFallSound);

            TreeIsDead();

        }
        
    }

    void TreeIsDead()
    {
        Vector3 treePosition = transform.position;

        Destroy(transform.parent.transform.parent.gameObject);
        canBeChopped = false;
        SelectionManager.Instance.selectedTree = null;
        SelectionManager.Instance.chopHolder.gameObject.SetActive(false);

        GameObject brokenTree = Instantiate(Resources.Load<GameObject>("ChoppedTree"),
            new Vector3(treePosition.x, treePosition.y + 1, treePosition.z), Quaternion.Euler(0, 0, 0));

    }



    private void Update()
    {
        if (canBeChopped)
        {
            GrobalState.Instance.resourceHelth = treeHealth;
            GrobalState.Instance.resourceMaxHelth = treeMaxHealth;

        }
    }
}
