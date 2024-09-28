using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class EquiableItem : MonoBehaviour
{

    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)
            && InventorySystem.Instance.isOpen == false
            && CraftingSystem.Instance.isOpen == false
            && SelectionManager.instance.HandIsVisible == false)
        {

            GameObject selectedTree = SelectionManager.instance.selectedTree;

            if(selectedTree != null)
            {
                selectedTree.GetComponent<ChoppableTree>().GetHit();
            }


            animator.SetTrigger("hit");
        }
    }
}
