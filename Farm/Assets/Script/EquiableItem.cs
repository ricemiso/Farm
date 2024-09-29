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
            && SelectionManager.Instance.HandIsVisible == false
            && !ConstructionManager.Instance.inConstructionMode)
        {

            StartCoroutine(SwingSoundDelay());

            animator.SetTrigger("hit");
        }
    }

    public void GetHit()
    {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;

        if (selectedTree != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);
            StartCoroutine(HitSoundDelay());
           
        }
    }

    IEnumerator SwingSoundDelay()
    {
        yield return new WaitForSeconds(0.2f);

        GameObject selectedTree = SelectionManager.Instance.selectedTree;

        if (selectedTree == null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);
        }
       
    }

    IEnumerator HitSoundDelay()
    {
        yield return new WaitForSeconds(0.2f);

        GameObject selectedTree = SelectionManager.Instance.selectedTree;
        selectedTree.GetComponent<ChoppableTree>().GetHit();

    }
}
