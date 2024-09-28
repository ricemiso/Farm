using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class ChoppableTree : MonoBehaviour
{
    public bool playerRange;
    public bool canBeChopped;

    public float treeMaxHealth;
    public float treeHealth;

    private void Start()
    {
        treeHealth = treeMaxHealth;
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
        StartCoroutine(hit()); ;
        
    }

    public IEnumerator hit()
    {
        yield return new WaitForSeconds(0.6f);
        treeHealth -= 1;
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
