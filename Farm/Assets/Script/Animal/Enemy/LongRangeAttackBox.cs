using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
// 
// ウサギの攻撃判定用ヒットボックスにつけるスクリプト
// 
//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

public class LongRangeAttackBox : MonoBehaviour
{

    [SerializeField]GameObject RabbitScript;
    LongRange longrange;

    // Start is called before the first frame update
    void Start()
    {
        longrange = RabbitScript.gameObject.transform.GetComponent<LongRange>();
    }

    // Update is called once per frame
    void Update()
    {
        if (longrange.enabled == false)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Crystal") || other.CompareTag("MiniCrystal") || other.CompareTag("SupportUnit"))
        {
            longrange.CheckAttack(other.GameObject());
        }
    }
}
