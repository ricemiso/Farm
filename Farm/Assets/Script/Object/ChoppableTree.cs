using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �j��\�Ȗ؃I�u�W�F�N�g���Ǘ�����N���X�B
/// </summary>
/// 

//�R���C�_�[���Z�b�g����
[RequireComponent(typeof(BoxCollider))]
public class ChoppableTree : MonoBehaviour
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
    /// �؂̍ő�w���X�B
    /// </summary>
    public float treeMaxHealth;

    /// <summary>
    /// �؂̌��݂̃w���X�B
    /// </summary>
    public float treeHealth;

    /// <summary>
    /// �A�j���[�^�[�R���|�[�l���g�B
    /// </summary>
    public Animator Animator;

    /// <summary>
    /// �؂��`���b�v����ۂɏ����J�����[�B
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
        if (treeHealth == 0)
        {
            treeHealth = treeMaxHealth;
        }

        caloriesSpendChoppingWood = 20;
        Animator = transform.parent.transform.parent.GetComponent<Animator>();
    }

    /// <summary>
    /// ���t���[���̍X�V�������s���܂��B
    /// </summary>
    private void Update()
    {
        if (canBeChopped)
        {
            GrobalState.Instance.resourceHelth = treeHealth;
            GrobalState.Instance.resourceMaxHelth = treeMaxHealth;
        }
        if (PlayerState.Instance.playerBody == null) return;
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
        Animator.SetTrigger("shake");

        treeHealth -= 1;

        PlayerState.Instance.currentCalories -= caloriesSpendChoppingWood;

        if (treeHealth <= 0)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.treeFallSound);
            TreeIsDead();
        }
    }

    /// <summary>
    /// �؂��j�󂳂ꂽ���̏������s���܂��B
    /// </summary>
    void TreeIsDead()
    {
        Vector3 treePosition = transform.position;
        GrobalState.Instance.isTreeChopped = true;
        Destroy(transform.parent.transform.parent.gameObject);
        canBeChopped = false;
        SelectionManager.Instance.selectedTree = null;
        SelectionManager.Instance.chopHolder.gameObject.SetActive(false);

        GameObject brokenTree = Instantiate(Resources.Load<GameObject>("ChoppedTree"), new Vector3(treePosition.x, treePosition.y + 1, treePosition.z), Quaternion.Euler(0, 0, 0));

        brokenTree.transform.SetParent(transform.parent.transform.parent.transform.parent);
        Destroy(brokenTree, 60f);
    }
}
