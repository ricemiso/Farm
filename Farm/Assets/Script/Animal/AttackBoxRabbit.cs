using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
// 
// �E�T�M�̍U������p�q�b�g�{�b�N�X�ɂ���X�N���v�g
// 
//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

public class AttackBoxRabbit : MonoBehaviour
{

    Rabbit RabbitScript;

    /// <summary>
    /// �v���C���[�ƓG�̊Ԃ̋����𑪂邽�߂̕ϐ��B
    /// </summary>
    float distance = 0;

    /// <summary>
    /// �v���C���[���y��̋߂��ɂ��邩�ǂ����������t���O�B
    /// </summary>
    public bool InRange;

    // Start is called before the first frame update
    void Start()
    {
        RabbitScript = GetComponentInParent<Rabbit>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

	private void OnTriggerStay(Collider other)
	{
        float distance = 0;
        distance = Vector3.Distance(other.gameObject.transform.position, transform.position);
        if (distance < 20f)
        {
            InRange = true;
        }
        else
        {
            InRange = false;
        }


        if (other.gameObject.GetComponent<Animal>())
        {
            if (other.gameObject.GetComponent<Animal>().isDead == false && other.CompareTag("SupportUnit")&& InRange)
            {
                RabbitScript.CheckAttack(other.GameObject());  
            }
        }
        else
        {
            if(other.CompareTag("Player")|| other.CompareTag("Crystal") || other.CompareTag("MiniCrystal")&&InRange)
            {
                RabbitScript.CheckAttack(other.GameObject());
                
            }
        }
       
	}
}
