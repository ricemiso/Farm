using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �N���t�g���s���ۂ̑f�ނ��Ǘ�����
/// </summary>
public class BluePrint : MonoBehaviour
{
    /// <summary>
    /// ���Y�����A�C�e���̖��O�B
    /// </summary>
    public string itemName;

    /// <summary>
    /// �K�v�ȑf��1�̖��O�B
    /// </summary>
    public string Req1;

    /// <summary>
    /// �K�v�ȑf��2�̖��O�B
    /// </summary>
    public string Req2;

    /// <summary>
    /// �K�v�ȑf��1�̗ʁB
    /// </summary>
    public int Req1amount;

    /// <summary>
    /// �K�v�ȑf��2�̗ʁB
    /// </summary>
    public int Req2amount;

    /// <summary>
    /// �K�v�ȑf�ނ̎�ނ̐��B
    /// </summary>
    public int numOfRequirement;

    /// <summary>
    /// ���Y�����A�C�e���̐��B
    /// </summary>
    public int numberOfItemsToProduce;

    /// <summary>
    /// BluePrint�N���X�̃R���X�g���N�^�B
    /// </summary>
    /// <param name="name">�A�C�e���̖��O�B</param>
    /// <param name="producedItems">���Y�����A�C�e���̐��B</param>
    /// <param name="reqNum">�K�v�ȑf�ނ̎�ނ̐��B</param>
    /// <param name="R1">�K�v�ȑf��1�̖��O�B</param>
    /// <param name="R1Num">�K�v�ȑf��1�̗ʁB</param>
    /// <param name="R2">�K�v�ȑf��2�̖��O�B</param>
    /// <param name="R2Num">�K�v�ȑf��2�̗ʁB</param>
    public BluePrint(string name, int producedItems, int reqNum, string R1, int R1Num, string R2, int R2Num)
    {
        itemName = name;
        numOfRequirement = reqNum;
        numberOfItemsToProduce = producedItems;
        Req1 = R1;
        Req2 = R2;
        Req1amount = R1Num;
        Req2amount = R2Num;
    }
}
