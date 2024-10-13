using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class EquiableItem : MonoBehaviour
{

    public Animator animator;
    public bool SwingWait;
    public float AxeDelay = 1f;

    void Start()
    {
        animator = GetComponent<Animator>();
        SwingWait = false;
        AxeDelay = 1f;
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)
            && InventorySystem.Instance.isOpen == false
            && CraftingSystem.Instance.isOpen == false
            && SelectionManager.Instance.HandIsVisible == false
            && !ConstructionManager.Instance.inConstructionMode
            && MenuManager.Instance.isMenuOpen == false
            && SwingWait ==false) 
        {
            SwingWait = true;
           
            StartCoroutine(SwingAction());
        }
    }


    IEnumerator SwingAction()
    {
        //SwingWait = true; 
        animator.SetTrigger("hit");

        yield return new WaitForSeconds(0.2f);  // �X�C���O�T�E���h�̒x��
        StartCoroutine(SwingSoundDelay());

        yield return new WaitForSeconds(1f);  // 1�b�̒x����ǉ�
       
        SwingWait = false;
    }

    public void GetHit()
    {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;
        GameObject selectedCraft = SelectionManager.Instance.selectedCraft;

        if (selectedTree != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);
            StartCoroutine(HitSoundDelay());
           
        }

        if(selectedCraft != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);
            StartCoroutine(HitCraftSoundDelay());
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


    //Todo: ������؂�|���Ƃ��͂����ɒǉ�����
    IEnumerator HitSoundDelay()
    {
        yield return new WaitForSeconds(0.2f);

        GameObject selectedTree = SelectionManager.Instance.selectedTree;
        selectedTree.GetComponent<ChoppableTree>().GetHit();

    }



    IEnumerator HitCraftSoundDelay()
    {
        yield return new WaitForSeconds(0.2f);

        GameObject selectedCraft = SelectionManager.Instance.selectedCraft;
        selectedCraft.GetComponent<Choppablecraft>().GetHit();

    }
}
