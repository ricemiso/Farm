using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquiableItem : MonoBehaviour
{

    public Animator animator;
    public bool SwingWait;
    private bool wasWatering = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        SwingWait = false;
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)
            && InventorySystem.Instance.isOpen == false
            && CraftingSystem.Instance.isOpen == false
            && SelectionManager.Instance.HandIsVisible == false
            && !ConstructionManager.Instance.inConstructionMode
            && MenuManager.Instance.isMenuOpen == false
            && SwingWait ==false
            && EquipSystem.Instance.IsPlayerHooldingWateringCan()== false) 
        {
            SwingWait = true;
           
            StartCoroutine(SwingAction());
        }

        if (SelectionManager.Instance.Watering && !wasWatering)
        {
            animator.SetTrigger("Watering");
            wasWatering = true;
        }
        else if (!SelectionManager.Instance.Watering && wasWatering)
        {
            wasWatering = false;
        }



    }

    IEnumerator SwingAction()
    {
        //SwingWait = true; 
        animator.SetTrigger("hit");

        yield return new WaitForSeconds(0.2f);  // ÉXÉCÉìÉOÉTÉEÉìÉhÇÃíxâÑ
        StartCoroutine(SwingSoundDelay());

        yield return new WaitForSeconds(1f);  // 1ïbÇÃíxâÑÇí«â¡
       
        SwingWait = false;
    }

    //Todo: âΩÇ©ÇêÿÇËì|Ç∑Ç∆Ç´ÇÕÇ±Ç±Ç…í«â¡Ç∑ÇÈ
    public void GetHit()
    {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;
        GameObject selectedCraft = SelectionManager.Instance.selectedCraft;

        if (selectedTree != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);
            StartCoroutine(HitSoundDelay());
           
        }
        else if(selectedCraft != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);
            StartCoroutine(HitCraftSoundDelay());
        }
        else
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);
        }

       
    }

    public void GetStoneHit()
    {
        
        GameObject selectedStone = SelectionManager.Instance.selectedStone;

        if (selectedStone != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);
            StartCoroutine(HitStoneSoundDelay());
        }
        else
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);
        }
    }

    public void GetManaCharge()
    {
        GameObject selectedCrystal = SelectionManager.Instance.selectedCrystal;

        if (selectedCrystal != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);
            StartCoroutine(HitStoneSoundDelay());
        }
        else
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);
        }
    }


    IEnumerator SwingSoundDelay()
    {
        yield return new WaitForSeconds(0.2f);

        GameObject selectedTree = SelectionManager.Instance.selectedTree;
        GameObject selectedCraft = SelectionManager.Instance.selectedCraft;
        GameObject selectedStone = SelectionManager.Instance.selectedStone;

        if (selectedTree == null && selectedCraft == null && selectedStone == null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);
        }

    }


    IEnumerator HitCraftSoundDelay()
    {
        yield return new WaitForSeconds(0.2f);

        GameObject selectedCraft = SelectionManager.Instance.selectedCraft;
        selectedCraft.GetComponent<Choppablecraft>().GetHit();

    }

    IEnumerator HitSoundDelay()
    {
        yield return new WaitForSeconds(0.2f);

        GameObject selectedTree = SelectionManager.Instance.selectedTree;
        selectedTree.GetComponent<ChoppableTree>().GetHit();

    }

    IEnumerator HitStoneSoundDelay()
    {
        yield return new WaitForSeconds(0.2f);

        GameObject selectedCraft = SelectionManager.Instance.selectedStone;
        selectedCraft.GetComponent<ChoppableStone>().GetHit();

    }
}
