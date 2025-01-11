using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �h���S���̃u���X���Ǘ�����v���O����
/// </summary>
public class Bless : MonoBehaviour
{

    LongRange Long;
    private bool isdamege;

    private void Start()
    {
        Long = GetComponentInParent<LongRange>();
        isdamege = false;
    }


    /// <summary>
    /// �u���X���ڐG�������̂��A�~�j�I���A�N���X�^���������ꍇ�_���[�W��^����
    /// </summary>
    /// <param name="other">�ڐG�����G</param>
    private void OnTriggerStay(Collider other)
    {
        float damage = GetComponentInParent<Animal>().damage;
        if ((other.CompareTag("SupportUnit") || other.CompareTag("Player") || other.CompareTag("Crystal") || other.CompareTag("MiniCrystal"))&&!isdamege)
        {
            if (other.gameObject.GetComponent<Animal>())
            {
                if (other.gameObject.GetComponent<Animal>().isDead == false)
                {
                   
                    Long.Attack(damage,other.gameObject);

                }
            }
            else
            {
                Long.Attack(damage, other.gameObject);

            }

            StartCoroutine(attackDelay());
        }


    }

    /// <summary>
    /// �A���q�b�g�����Ȃ����߂ɒx�������ăt���O�𗧂Ă�
    /// </summary>
    /// <returns></returns>
    IEnumerator attackDelay()
    {
        isdamege = true;
        yield return new WaitForSeconds(0.5f);
        isdamege = false;
    }
}
