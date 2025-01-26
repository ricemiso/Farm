using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//担当者　越浦晃生

/// <summary>
/// 装備可能なアイテムを管理するクラス。
/// </summary>
[RequireComponent(typeof(Animator))]
public class EquiableItem : MonoBehaviour
{
    /// <summary>
    /// アニメーターコンポーネント。
    /// </summary>
    public Animator animator;

    /// <summary>
    /// スイング中かどうかを示すフラグ。
    /// </summary>
    public bool Swinging = false;

    /// <summary>
    /// 水やり中かどうかを示すフラグ。
    /// </summary>
    private bool wasWatering = false;

    /// <summary>
    /// 充電中かどうかを示すフラグ。
    /// </summary>
    private bool wasChargeing = false;

    /// <summary>
    /// レベルアップ中かどうかを示すフラグ。
    /// </summary>
    private bool wasleveling = false;

    /// <summary>
    /// 初期設定を行います。
    /// </summary>
    void Start()
    {
        animator = GetComponent<Animator>();
        Swinging = false;
    }

    /// <summary>
    /// 武器を振った時のアニメーションの再生
    /// </summary>
    void Update()
    {
        if (Input.GetMouseButtonDown(0)
            && InventorySystem.Instance.isOpen == false
            && CraftingSystem.Instance.isOpen == false
            && SelectionManager.Instance.HandIsVisible == false
            && !ConstructionManager.Instance.inConstructionMode
            && MenuManager.Instance.isMenuOpen == false
            && EquipSystem.Instance.IsPlayerHooldingWateringCan() == false)
        {
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
        else if ((!SelectionManager.Instance.Watering && wasWatering) || (!SelectionManager.Instance.Chargeing && wasChargeing)
            || (!SelectionManager.Instance.leveling && wasleveling))
        {
            animator.ResetTrigger("Watering");
            wasWatering = false;
            wasChargeing = false;
            wasleveling = false;
        }
    }

    /// <summary>
    /// スイングアクションを実行するコルーチン。
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator SwingAction()
    {
        animator.SetTrigger("hit");

		//yield return new WaitForSeconds(0.2f);  // スイングサウンドの遅延
		//StartCoroutine(SwingSoundDelay());
		//yield return new WaitForSeconds(1f);  // 1秒の遅延を追加

		//SwingSoundDelayメソッドの処理が不要のため、遅延秒数を統合
		yield return new WaitForSeconds(1.4f);

		Swinging = false;
    }

    /// <summary>
    /// アイテムがヒットされた時の処理を行います。
    /// </summary>
    public void GetHit()
    {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;
        GameObject selectedCraft = SelectionManager.Instance.selectedCraft;

        if (selectedTree != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);
            StartCoroutine(HitSoundDelay());
        }
        else if (selectedCraft != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);
            StartCoroutine(HitCraftSoundDelay());
        }
        else
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);
        }
    }

    /// <summary>
    /// 石をヒットされた時の処理を行います。
    /// </summary>
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

    /// <summary>
    /// マナのチャージをヒットされた時の処理を行います。
    /// </summary>
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

    /// <summary>
    /// スイングサウンドの遅延を行うコルーチン。
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator SwingSoundDelay()
    {
        yield return new WaitForSeconds(0.2f);

        //GameObject selectedTree = SelectionManager.Instance.selectedTree;
        //GameObject selectedCraft = SelectionManager.Instance.selectedCraft;
        //GameObject selectedStone = SelectionManager.Instance.selectedStone;

        //if (selectedTree == null && selectedCraft == null && selectedStone == null)
        //{
        //    SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);
        //}
    }

    /// <summary>
    /// クラフトアイテムをヒットされた時の処理を行うコルーチン。
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator HitCraftSoundDelay()
    {
        yield return new WaitForSeconds(0.2f);

        GameObject selectedCraft = SelectionManager.Instance.selectedCraft;
        selectedCraft.GetComponent<Choppablecraft>().GetHit();
    }

    /// <summary>
    /// 木をヒットされた時の処理を行うコルーチン。
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator HitSoundDelay()
    {
        yield return new WaitForSeconds(0.2f);

        GameObject selectedTree = SelectionManager.Instance.selectedTree;
        selectedTree.GetComponent<ChoppableTree>().GetHit();
    }

    /// <summary>
    /// 石をヒットされた時の処理を行うコルーチン。
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator HitStoneSoundDelay()
    {
        yield return new WaitForSeconds(0.2f);

        GameObject selectedCraft = SelectionManager.Instance.selectedStone;
        selectedCraft.GetComponent<ChoppableStone>().GetHit();
    }
}
