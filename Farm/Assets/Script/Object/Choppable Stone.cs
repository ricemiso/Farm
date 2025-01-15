using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�S���ҁ@�z�Y�W��

/// <summary>
/// �j��\�Ȑ΃I�u�W�F�N�g���Ǘ�����N���X�B
/// </summary>
/// 

//�R���C�_�[���Z�b�g����
[RequireComponent(typeof(BoxCollider))]
public class ChoppableStone : MonoBehaviour
{
    /// <summary>
    /// �v���C���[���͈͓��ɂ��邩�ǂ����������t���O�B
    /// </summary>
    public bool playerRange;

    /// <summary>
    /// �`���b�v�\���ǂ����������t���O�B
    /// </summary>
    public bool canBeChopped;

    /// <summary>
    /// �A�j���[�V�����̃N�[���^�C���������t���O�B
    /// </summary>
    public bool animcooltime;

    /// <summary>
    /// �΂̍ő�w���X�B
    /// </summary>
    public float stoneMaxHealth;

    /// <summary>
    /// �΂̌��݂̃w���X�B
    /// </summary>
    public float stoneHealth;

    // public Animator Animator;

    /// <summary>
    /// �΂��`���b�v����ۂɏ����J�����[�B
    /// </summary>
    public float caloriesSpendChoppingWood;

    /// <summary>
    /// ���苗���B
    /// </summary>
    [SerializeField] float dis = 10f;

    /// <summary>
    /// �����ݒ���s���܂��B
    /// </summary>
    private void Start()
    {
        if (stoneHealth == 0)
        {
            stoneHealth = stoneMaxHealth;
        }

        caloriesSpendChoppingWood = 20;
        // Animator = transform.parent.GetComponent<Animator>();
    }

    /// <summary>
    /// ���t���[���̍X�V�������s���܂��B
    /// </summary>
    private void Update()
    {
        if (canBeChopped)
        {
            GrobalState.Instance.resourceHelth = stoneHealth;
            GrobalState.Instance.resourceMaxHelth = stoneMaxHealth;
        }

        if (PlayerState.Instance == null) return;
        float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

        if (distance < dis)
        {
            playerRange = true;
        }
        else
        {
            playerRange = false;
        }
    }

    /// <summary>
    /// �A�C�e�����q�b�g���ꂽ���̏������s���܂��B
    /// </summary>
    public void GetHit()
    {
        // Animator.SetTrigger("shake");

        stoneHealth -= 1;

        PlayerState.Instance.currentCalories -= caloriesSpendChoppingWood;

        if (stoneHealth <= 0)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.Stonebreak);
            StoneIsDead();
        }
    }

    /// <summary>
    /// �΂��j�󂳂ꂽ���̏������s���܂��B
    /// </summary>
    void StoneIsDead()
    {
        Vector3 treePosition = transform.position;
        GrobalState.Instance.isStoneChopped = true;
        Destroy(transform.parent.gameObject);
        canBeChopped = false;
        SelectionManager.Instance.selectedStone = null;
        SelectionManager.Instance.chopHolder.gameObject.SetActive(false);

        // �I�u�W�F�N�g1
        GameObject brokenTree1 = Instantiate(Resources.Load<GameObject>("Stone_model"), treePosition, Quaternion.Euler(0, 0, 0));

        // �I�u�W�F�N�g2 (x ���W���������炷)
        GameObject brokenTree2 = Instantiate(Resources.Load<GameObject>("Stone_model"), new Vector3(treePosition.x + 1.5f, treePosition.y, treePosition.z), Quaternion.Euler(0, 0, 0));

        // �I�u�W�F�N�g3 (z ���W���������炷)
        GameObject brokenTree3 = Instantiate(Resources.Load<GameObject>("Stone_model"), new Vector3(treePosition.x, treePosition.y, treePosition.z + 1.5f), Quaternion.Euler(0, 0, 0));

        Destroy(brokenTree1, 60f);
        Destroy(brokenTree2, 60f);
        Destroy(brokenTree3, 60f);
    }
}
