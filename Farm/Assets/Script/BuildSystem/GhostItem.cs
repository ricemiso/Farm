using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�S���ҁ@�z�Y�W��

/// <summary>
/// ���݃��[�h�Ŕz�u�\�ȃS�[�X�g�A�C�e����\���N���X�B
/// </summary>
public class GhostItem : MonoBehaviour
{
    /// <summary>
    /// �S�[�X�g�A�C�e���̃R���C�_�[�B
    /// </summary>
    public BoxCollider solidCollider;

    /// <summary>
    /// �S�[�X�g�A�C�e���̃����_���[�B
    /// </summary>
    public Renderer mRenderer;

    /// <summary>
    /// �������}�e���A���B
    /// </summary>
    private Material semiTransparentMat;

    /// <summary>
    /// ���S�����}�e���A���B
    /// </summary>
    private Material fullTransparentnMat;

    /// <summary>
    /// �I�����ꂽ�}�e���A���B
    /// </summary>
    private Material selectedMaterial;

    /// <summary>
    /// �S�[�X�g�A�C�e�����ݒu���ꂽ���ǂ����B
    /// </summary>
    public bool isPlaced;

    /// <summary>
    /// �����ʒu�������ǂ����B
    /// </summary>
    public bool hasSamePosition = false;

    /// <summary>
    /// �S�[�X�g�A�C�e�������������A�S�S�[�X�g���X�g�ɒǉ����܂��B
    /// </summary>
    private void Awake()
    {
        ConstructionManager.Instance.allGhostsInExistence.Add(this.gameObject);
    }

    /// <summary>
    /// �S�[�X�g�A�C�e���̃}�e���A���Ə�����Ԃ�ݒ肵�܂��B
    /// </summary>
    private void Start()
    {
        mRenderer = GetComponent<Renderer>();
        semiTransparentMat = ConstructionManager.Instance.ghostSemiTransparentMat;
        fullTransparentnMat = ConstructionManager.Instance.ghostFullTransparentMat;
        selectedMaterial = ConstructionManager.Instance.ghostSelectedMat;

        mRenderer.material = fullTransparentnMat;

        solidCollider.enabled = false;
    }

    /// <summary>
    /// ���݃��[�h�̏�ԂɊ�Â��ăS�[�X�g�A�C�e���̏�Ԃ��X�V���܂��B
    /// </summary>
    private void Update()
    {
        if (ConstructionManager.Instance.inConstructionMode)
        {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), ConstructionManager.Instance.player.GetComponent<Collider>());
        }

        if (ConstructionManager.Instance.inConstructionMode && isPlaced)
        {
            solidCollider.enabled = true;
        }

        if (!ConstructionManager.Instance.inConstructionMode)
        {
            solidCollider.enabled = false;
        }

        if (ConstructionManager.Instance.selectedGhost == this.gameObject)
        {
            mRenderer.material = selectedMaterial;
        }
        else
        {
            mRenderer.material = fullTransparentnMat;
        }
    }

    /// <summary>
    /// �S�[�X�g�A�C�e�������̃R���C�_�[���ɂ���Ƃ��̏Փˌ��o���������܂��B
    /// </summary>
    /// <param name="other">���̃S�[�X�g�A�C�e�����ڐG���Ă���R���C�_�[�B</param>
    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject);

        if (!this.gameObject.CompareTag("wallghost") && other.CompareTag("placedFoundation") && !ConstructionManager.Instance.inConstructionMode)
        {
            this.gameObject.SetActive(false);
            Debug.Log("�S�[�X�g��placedFoundation�Ƀq�b�g���܂���" + gameObject.name);
        }
    }
}
