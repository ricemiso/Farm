using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquiableItem : MonoBehaviour
{

    public Animator animator;
    public bool Swinging = false;
    private bool wasWatering = false;
    private bool wasChargeing = false;
    private bool wasleveling = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        Swinging = false;
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)
            && InventorySystem.Instance.isOpen == false
            && CraftingSystem.Instance.isOpen == false
            && SelectionManager.Instance.HandIsVisible == false
            && !ConstructionManager.Instance.inConstructionMode
            && MenuManager.Instance.isMenuOpen == false
            && EquipSystem.Instance.SwingWait ==false
            && EquipSystem.Instance.IsPlayerHooldingWateringCan()== false) 
        {
            EquipSystem.Instance.SwingWait = true;
            Swinging = true;
            StartCoroutine(SwingAction());
        }

        if ((SelectionManager.Instance.Watering && !wasWatering) || (SelectionManager.Instance.Chargeing && !wasChargeing)
            || (SelectionManager.Instance.leveling && !wasleveling)) 
        {
            animator.SetTrigger("Watering");
            wasWatering = true;
            wasChargeing = true;
            wasleveling = true;
        }
        else if ((!SelectionManager.Instance.Watering && wasWatering)||(!SelectionManager.Instance.Chargeing && wasChargeing)
            || (!SelectionManager.Instance.leveling && wasleveling))
        {
            animator.ResetTrigger("Watering");
            wasWatering = false;
            wasChargeing = false;
            wasleveling = false;
        }



    }

    IEnumerator SwingAction()
    {
        //SwingWait = true; 
        animator.SetTrigger("hit");

        yield return new WaitForSeconds(0.2f);  // �X�C���O�T�E���h�̒x��
        StartCoroutine(SwingSoundDelay());

        yield return new WaitForSeconds(1f);  // 1�b�̒x����ǉ�

        EquipSystem.Instance.SwingWait = false;
        Swinging = false;
    }

    //Todo: ������؂�|���Ƃ��͂����ɒǉ�����
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
