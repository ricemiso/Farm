using System.Collections.Generic;
using UnityEngine;

//�S���ҁ@�z�Y�W��

/// <summary>
/// ���݉\�ȃI�u�W�F�N�g�𐧌䂷��N���X�B�ݒu�\���ǂ����̔����A���o�I�ȃt�B�[�h�o�b�N���Ǘ�����B
/// �v���C���[���I�u�W�F�N�g��ݒu�ł��邩�A���̃A�C�e���Əd�Ȃ��Ă��Ȃ������m�F���A�F��ύX���ď�Ԃ������B
/// </summary>
public class Constructable : MonoBehaviour
{
    // Validation
    /// <summary>
    /// �I�u�W�F�N�g���n�ʂɐڂ��Ă��邩�ǂ����������t���O�B
    /// </summary>
    public bool isGrounded;

    /// <summary>
    /// ���̃A�C�e���Əd�Ȃ��Ă��邩�ǂ����������t���O�B
    /// </summary>
    public bool isOverlappingItems;

    /// <summary>
    /// �I�u�W�F�N�g���ݒu�\���ǂ����������t���O�B
    /// </summary>
    public bool isValidToBeBuilt;

    /// <summary>
    /// �S�[�X�g�����o�[�����o���ꂽ���ǂ����������t���O�B
    /// </summary>
    public bool detectedGhostMemeber;

    // Material related
    /// <summary>
    /// �����_���[�R���|�[�l���g�̃��X�g�B
    /// </summary>
    private List<Renderer> renderers = new List<Renderer>();

    /// <summary>
    /// ���S�ɓ����ȃ}�e���A���B
    /// </summary>
    private Material fullTransparentnMat;

    /// <summary>
    /// �I�u�W�F�N�g�������ȏ�Ԃ������Ԃ��}�e���A���B
    /// </summary>
    public Material redMaterial;

    /// <summary>
    /// �I�u�W�F�N�g���L���ȏ�Ԃ������΂̃}�e���A���B
    /// </summary>
    public Material greenMaterial;

    /// <summary>
    /// �I�u�W�F�N�g�̃f�t�H���g�̃}�e���A���B
    /// </summary>
    public Material defaultMaterial;

    /// <summary>
    /// �S�[�X�g�I�u�W�F�N�g�̃��X�g�B
    /// </summary>
    public List<GameObject> ghostList = new List<GameObject>();

    /// <summary>
    /// �ő̃R���C�_�[�B�I�u�W�F�N�g�̕����I�ȏՓ˔���Ɏg�p�����B
    /// </summary>
    public BoxCollider solidCollider;

    /// <summary>
    /// ���������ɕK�v�ȃR���|�[�l���g��ݒ肷��B�q�I�u�W�F�N�g�̃����_���[�����X�g�ɒǉ����A�����̃}�e���A����ݒ肷��B
    /// �S�[�X�g�I�u�W�F�N�g�����X�g�ɒǉ�����B
    /// </summary>
    private void Start()
    {
        // �q�I�u�W�F�N�g�̃����_���[�R���|�[�l���g�����X�g�ɒǉ�
        Renderer[] allRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in allRenderers)
        {
            if (renderer != null) // �q�I�u�W�F�N�g�Ƀ����_���[�R���|�[�l���g�����邩���m�F
            {
                renderers.Add(renderer);
                if (gameObject.GetComponent<Animal>())
                {
                    return;
                }
                renderer.material = defaultMaterial; // �����̃}�e���A����ݒ�
            }
        }

        fullTransparentnMat = ConstructionManager.Instance.ghostFullTransparentMat; // �����ȃ}�e���A�����擾

        // �S�[�X�g�I�u�W�F�N�g�����X�g�ɒǉ�
        foreach (Transform child in transform)
        {
            ghostList.Add(child.gameObject);
        }
    }

    /// <summary>
    /// ���t���[���X�V����鏈���B�I�u�W�F�N�g���n�ʂɐڂ��Ă��邩�A�d�Ȃ��Ă���A�C�e�������邩���`�F�b�N���A
    /// �ݒu�\���ǂ����𔻒肷��B
    /// </summary>
    void Update()
    {
        if (isGrounded && isOverlappingItems == false)
        {
            isValidToBeBuilt = true;
        }
        else
        {
            isValidToBeBuilt = false;
        }

        // ���C�L���X�g���g���Ēn�ʂƂ̐ڐG���m�F
        var boxHeight = transform.lossyScale.y;
        RaycastHit groundHit;
        if (Physics.Raycast(transform.position, Vector3.down, out groundHit, boxHeight * 1f, LayerMask.GetMask("Ground", "placedFoundation")))
        {
            isGrounded = true;

            // �I�u�W�F�N�g�̉�]��n�ʂ̖@���ɍ��킹�Ē���
            Quaternion newRotation = Quaternion.FromToRotation(transform.up, groundHit.normal) * transform.rotation;
            transform.rotation = newRotation;
        }
        else
        {
            isGrounded = false;
        }
    }

    /// <summary>
    /// �g���K�[�͈͂ɓ������Ƃ��ɌĂ΂�鏈���B�n�ʂ⑼�̃A�C�e���Ƃ̏d�Ȃ�����o���A��Ԃ��X�V����B
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Ground") || other.CompareTag("placedFoundation")) && gameObject.CompareTag("activeConstructable"))
        {
            isGrounded = true;

            // �n�ʂɍ��킹�ăI�u�W�F�N�g�̉�]�𒲐�
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                Quaternion newRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
                transform.rotation = newRotation;
            }
        }

        if (other.CompareTag("Tree") || other.CompareTag("Pickable") && gameObject.CompareTag("activeConstructable"))
        {
            isOverlappingItems = true;
        }

        if (other.gameObject.CompareTag("ghost") && gameObject.CompareTag("activeConstructable"))
        {
            detectedGhostMemeber = true;
        }
    }

    /// <summary>
    /// �g���K�[�͈͂���o���Ƃ��ɌĂ΂�鏈���B�n�ʂ⑼�̃A�C�e���Ƃ̏d�Ȃ���������A��Ԃ��X�V����B
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("placedFoundation") && gameObject.CompareTag("activeConstructable"))
        {
            isGrounded = false;
        }

        if (other.CompareTag("Stone") || other.CompareTag("Tree") || other.CompareTag("Pickable") && gameObject.CompareTag("activeConstructable"))
        {
            isOverlappingItems = false;
        }

        if (other.gameObject.CompareTag("ghost") && gameObject.CompareTag("activeConstructable"))
        {
            detectedGhostMemeber = false;
        }
    }

    /// <summary>
    /// �����Ȑݒu��Ԃ��������߂ɁA�I�u�W�F�N�g�̐F��ԂɕύX����B
    /// </summary>
    public void SetInvalidColor()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.material = redMaterial;
        }
    }

    /// <summary>
    /// �L���Ȑݒu��Ԃ��������߂ɁA�I�u�W�F�N�g�̐F��΂ɕύX����B
    /// </summary>
    public void SetValidColor()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.material = greenMaterial;
        }
    }

    /// <summary>
    /// ���S�ɓ����ȏ�Ԃ��������߂ɁA�I�u�W�F�N�g�̐F�𓧖��ɕύX����B
    /// </summary>
    public void SetfullTransparentnColor()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.material = fullTransparentnMat;
        }
    }

    /// <summary>
    /// �f�t�H���g�̐ݒu��Ԃ��������߂ɁA�I�u�W�F�N�g�̐F���f�t�H���g�ɖ߂��B
    /// </summary>
    public void SetDefaultColor()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.material = defaultMaterial;
        }
    }

    /// <summary>
    /// �S�[�X�g�����o�[��e�I�u�W�F�N�g����؂藣���A�ő̃R���C�_�[�𖳌������Ĕz�u��Ԃ��X�V����B
    /// </summary>
    public void ExtractGhostMembers()
    {
        foreach (GameObject item in ghostList)
        {
            item.transform.SetParent(transform.parent, true);
            item.gameObject.GetComponent<GhostItem>().solidCollider.enabled = false;
            item.gameObject.GetComponent<GhostItem>().isPlaced = true;
        }
    }
}
