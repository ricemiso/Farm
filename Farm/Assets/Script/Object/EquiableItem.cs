using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �����\�ȃA�C�e�����Ǘ�����N���X�B
/// </summary>
[RequireComponent(typeof(Animator))]
public class EquiableItem : MonoBehaviour
{
    /// <summary>
    /// �A�j���[�^�[�R���|�[�l���g�B
    /// </summary>
    public Animator animator;

    /// <summary>
    /// �X�C���O�����ǂ����������t���O�B
    /// </summary>
    public bool Swinging = false;

    /// <summary>
    /// ����蒆���ǂ����������t���O�B
    /// </summary>
    private bool wasWatering = false;

    /// <summary>
    /// �[�d�����ǂ����������t���O�B
    /// </summary>
    private bool wasChargeing = false;

    /// <summary>
    /// ���x���A�b�v�����ǂ����������t���O�B
    /// </summary>
    private bool wasleveling = false;

    /// <summary>
    /// �����ݒ���s���܂��B
    /// </summary>
    void Start()
    {
        animator = GetComponent<Animator>();
        Swinging = false;
    }

    /// <summary>
    /// �����U�������̃A�j���[�V�����̍Đ�
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
    /// �X�C���O�A�N�V���������s����R���[�`���B
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator SwingAction()
    {
        animator.SetTrigger("hit");

		//yield return new WaitForSeconds(0.2f);  // �X�C���O�T�E���h�̒x��
		//StartCoroutine(SwingSoundDelay());
		//yield return new WaitForSeconds(1f);  // 1�b�̒x����ǉ�

		//SwingSoundDelay���\�b�h�̏������s�v�̂��߁A�x���b���𓝍�
		yield return new WaitForSeconds(1.4f);

		Swinging = false;
    }

    /// <summary>
    /// �A�C�e�����q�b�g���ꂽ���̏������s���܂��B
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
    /// �΂��q�b�g���ꂽ���̏������s���܂��B
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
    /// �}�i�̃`���[�W���q�b�g���ꂽ���̏������s���܂��B
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
    /// �X�C���O�T�E���h�̒x�����s���R���[�`���B
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
    /// �N���t�g�A�C�e�����q�b�g���ꂽ���̏������s���R���[�`���B
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator HitCraftSoundDelay()
    {
        yield return new WaitForSeconds(0.2f);

        GameObject selectedCraft = SelectionManager.Instance.selectedCraft;
        selectedCraft.GetComponent<Choppablecraft>().GetHit();
    }

    /// <summary>
    /// �؂��q�b�g���ꂽ���̏������s���R���[�`���B
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator HitSoundDelay()
    {
        yield return new WaitForSeconds(0.2f);

        GameObject selectedTree = SelectionManager.Instance.selectedTree;
        selectedTree.GetComponent<ChoppableTree>().GetHit();
    }

    /// <summary>
    /// �΂��q�b�g���ꂽ���̏������s���R���[�`���B
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator HitStoneSoundDelay()
    {
        yield return new WaitForSeconds(0.2f);

        GameObject selectedCraft = SelectionManager.Instance.selectedStone;
        selectedCraft.GetComponent<ChoppableStone>().GetHit();
    }
}
