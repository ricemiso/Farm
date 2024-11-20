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
        if(other.CompareTag("Player"))
        {
            RabbitScript.CheckAttack(other.GameObject());
        }
	}
}
