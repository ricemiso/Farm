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
    private bool wasChargeing = false;
    private bool wasleveling = false;
    private bool isTriggered = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        SwingWait = false;
    }


    void Update()
    {
        // トリガーを呼び出す条件
        if ((SelectionManager.Instance.Watering && !wasWatering)
            || (SelectionManager.Instance.Chargeing && !wasChargeing)
            || (SelectionManager.Instance.leveling && !wasleveling && !isTriggered))
        {
            isTriggered = true; // トリガーが発動したことを記録
            animator.SetTrigger("Watering");

            wasWatering = true;
            wasChargeing = true;
            wasleveling = true;
        }
        else if ((!SelectionManager.Instance.Watering && wasWatering)
            || (!SelectionManager.Instance.Chargeing && wasChargeing)
            || (!SelectionManager.Instance.leveling && wasleveling))
        {
            // 条件が満たされなくなったときにフラグをリセット
            animator.ResetTrigger("Watering");
            isTriggered = false; // トリガー再発動を許可する

            wasWatering = false;
            wasChargeing = false;
            wasleveling = false;
        }
    }



    IEnumerator SwingAction()
    {
        //SwingWait = true; 
        animator.SetTrigger("hit");

        yield return new WaitForSeconds(0.2f);  // スイングサウンドの遅延
        StartCoroutine(SwingSoundDelay());

        yield return new WaitForSeconds(1f);  // 1秒の遅延を追加
       
        SwingWait = false;
    }

    //Todo: 何かを切り倒すときはここに追加する
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
