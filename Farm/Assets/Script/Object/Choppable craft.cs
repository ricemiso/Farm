using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�S���ҁ@�z�Y�W���@
//���z�p�~�ɔ������ݎg�p���Ă��Ȃ�

/// <summary>
/// �j��\�ȃN���t�g�I�u�W�F�N�g���Ǘ�����N���X�B
/// </summary>
public class Choppablecraft : MonoBehaviour
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
    /// �ő�w���X�B
    /// </summary>
    public float MaxHealth;

    /// <summary>
    /// ���݂̃w���X�B
    /// </summary>
    public float Health;

    /// <summary>
    /// �N���t�g�A�C�e���̖��O�B
    /// </summary>
    public string craftName;

    /// <summary>
    /// ���苗���B
    /// </summary>
    [SerializeField] float dis = 50f;

    /// <summary>
    /// �؂��`���b�v����ۂɏ����J�����[�B
    /// </summary>
    public float caloriesSpendChoppingWood;

    /// <summary>
    /// �����ݒ���s���܂��B
    /// </summary>
    private void Start()
    {
        Health = MaxHealth;
        caloriesSpendChoppingWood = 20;
    }

    /// <summary>
    /// �N���t�g�A�C�e���̖��O��Ԃ��܂��B
    /// </summary>
    /// <returns>�N���t�g�A�C�e���̖��O</returns>
    public string CraftItemName()
    {
        return craftName;
    }

    /// <summary>
    /// ���t���[���̍X�V�������s���܂��B
    /// </summary>
    private void Update()
    {
        if (canBeChopped)
        {
            GrobalState.Instance.resourceHelth = Health;
            GrobalState.Instance.resourceMaxHelth = MaxHealth;
        }
    }

    /// <summary>
    /// ���̃I�u�W�F�N�g���R���C�_�[�ɓ��������̏������s���܂��B
    /// </summary>
    /// <param name="other">�R���C�_�[�ɓ��������̃I�u�W�F�N�g</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRange = true;
        }
    }

    /// <summary>
    /// ���̃I�u�W�F�N�g���R���C�_�[����o�����̏������s���܂��B
    /// </summary>
    /// <param name="other">�R���C�_�[����o�����̃I�u�W�F�N�g</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRange = false;
        }
    }

    /// <summary>
    /// �A�C�e�����q�b�g���ꂽ���̏������s���܂��B
    /// </summary>
    public void GetHit()
    {
        Health -= 1;
        PlayerState.Instance.currentCalories -= caloriesSpendChoppingWood;

        if (Health <= 0)
        {
            Destroy();
        }
    }

    /// <summary>
    /// �_���[�W���󂯂����̏������s���܂��B
    /// </summary>
    public void GetDamage()
    {
        //Todo EnemyAI_Movement�ɗ^����_���[�W�ʂ�����ϐ����쐬
        //Health -= EnemyAI_Movement.Instantiate.damage;

        if (Health <= 0)
        {
            Destroy();
        }
    }

    /// <summary>
    /// �I�u�W�F�N�g��j�󂵂܂��B
    /// </summary>
    public void Destroy()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.treeFallSound);

        Destroy(gameObject);
        canBeChopped = false;
        SelectionManager.Instance.selectedCraft = null;
        SelectionManager.Instance.chopHolder.gameObject.SetActive(false);
    }
}
