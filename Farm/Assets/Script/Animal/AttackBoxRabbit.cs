using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
// 
// ウサギの攻撃判定用ヒットボックスにつけるスクリプト
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
