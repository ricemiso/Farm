using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �A���𐧌䂷��N���X�B��������n�̊Ǘ�
/// </summary>
public class Plant : MonoBehaviour
{
    [SerializeField] GameObject seedModel; // ��̃��f��
    [SerializeField] GameObject youngPlantModel; // �Ⴂ�A���̃��f��
    [SerializeField] GameObject maturePlanetModel; // ���n�����A���̃��f��

    [SerializeField] List<GameObject> plantProduceSpawn; // ���������X�|�[��������ꏊ�̃��X�g

    [SerializeField] GameObject producePrefab; // ��������v���_�N�g�̃v���n�u

    public int dayOfPlanting; // �A������
    [SerializeField] int plantage = 0; // �A���̌��݂̔N��

    [SerializeField] int ageForYourModel; // �Ⴂ�A�����f���ւ̐؂�ւ��N��
    [SerializeField] int ageForMatureModel; // ���n���f���ւ̐؂�ւ��N��
    [SerializeField] int ageForFirstProduceBatch; // �ŏ��̐����������������N��

    [SerializeField] int daysForNewProduce; // ���̐����������������܂ł̓���
    [SerializeField] int daysRemainingForNewProduce; // ���̐����������܂ł̎c�����

    [SerializeField] bool isOneTimearvest; // ���n����x�������ǂ���

    private bool hasGeneratedProduce = false; // ���������������ꂽ���ǂ���

    /// <summary>
    /// �I�u�W�F�N�g���L�������ꂽ�Ƃ��Ƀ��X�i�[��o�^���܂��B
    /// </summary>
    private void OnEnable()
    {
        TimeManager.Instance.oneDayPass.AddListener(DayPass);
    }

    /// <summary>
    /// �I�u�W�F�N�g�����������ꂽ�Ƃ��Ƀ��X�i�[���폜���܂��B
    /// </summary>
    private void OnDisable()
    {
        TimeManager.Instance.oneDayPass.RemoveListener(DayPass);
    }

    /// <summary>
    /// �I�u�W�F�N�g���j�����ꂽ�Ƃ��̏����B
    /// �e��Soil�R���|�[�l���g�����Z�b�g���܂��B
    /// </summary>
    private void OnDestroy()
    {
        Soil soil = GetComponentInParent<Soil>();
        if (soil != null)
        {
            soil.isEmpty = true;
            soil.plantName = "";
            soil.currentplant = null;
        }
    }

    /// <summary>
    /// �A���𐬒������܂��B
    /// </summary>
    public void Grow()
    {
        plantage++;

        // �y�𐅂��Ȃ���ԂɕύX
        GetComponentInParent<Soil>().MakeSoilNotWatered();

        // �R���C�_�[�𖳌���
        SphereCollider collider = GetComponent<SphereCollider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        CheckRroduce();
    }

    /// <summary>
    /// ����o�ߎ��̏������s���܂��B
    /// </summary>
    public void DayPass()
    {
        if (!isOneTimearvest)
        {
            if (CheckGrows())
            {
                // ���n���𐶐�
                // GenerateProduceForEmptySpawn();
            }
            else
            {
                Grow();
            }
        }
        else
        {
            GetComponentInParent<Soil>().MakeSoilNotWatered();
        }
    }

    /// <summary>
    /// ���t���[���̍X�V�����B
    /// </summary>
    private void Update()
    {
        if (CheckGrows() && !hasGeneratedProduce)
        {
            GenerateProduceForEmptySpawn();
            hasGeneratedProduce = true; // �����ς݃t���O��ݒ�
        }
    }

    /// <summary>
    /// ���݂̐A���̃��f����؂�ւ��鏈���B
    /// </summary>
    private void CheckRroduce()
    {
        seedModel.SetActive(plantage < ageForFirstProduceBatch);
        youngPlantModel.SetActive(plantage >= ageForYourModel && plantage < ageForMatureModel);
        maturePlanetModel.SetActive(plantage >= ageForMatureModel);

        if (plantage >= ageForMatureModel && isOneTimearvest)
        {
            MakePlantPickable();
        }
    }

    /// <summary>
    /// �A�����C���^���N�g�\�ɂ��܂��B
    /// </summary>
    private void MakePlantPickable()
    {
        GetComponent<InteractableObject>().enabled = true;
        GetComponent<SphereCollider>().enabled = true;
    }

    /// <summary>
    /// �A���������������Ă��邩�ǂ������`�F�b�N���܂��B
    /// </summary>
    /// <returns>�����������Ă����true��Ԃ��܂��B</returns>
    public bool CheckGrows()
    {
        if (plantage >= ageForFirstProduceBatch) return true;
        return false;
    }

    /// <summary>
    /// ��̃X�|�[���|�C���g�ɐ������𐶐����܂��B
    /// </summary>
    private void GenerateProduceForEmptySpawn()
    {
        foreach (GameObject spawn in plantProduceSpawn)
        {
            if (spawn.transform.childCount == 0)
            {
                GameObject produce = Instantiate(producePrefab);
                Destroy(this.gameObject, 60); // ���̃I�u�W�F�N�g��60�b��ɔj��
                Destroy(producePrefab, 60); // �v���n�u��60�b��ɔj��

                produce.transform.parent = spawn.transform;

                Vector3 producePos = Vector3.zero;
                producePos.y = 0f;
                produce.transform.localPosition = producePos;
            }
        }
    }
}
