using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bless : MonoBehaviour
{

    LongRange Long;

    private void Start()
    {
        Long = GetComponentInParent<LongRange>();
    }

    private void OnTriggerStay(Collider other)
    {


        if (other.gameObject.GetComponent<Animal>())
        {
            if (other.gameObject.GetComponent<Animal>().isDead == false && other.CompareTag("SupportUnit"))
            {
                float damage = GetComponent<Animal>().damage;
                Long.Attack(damage);
               
            }
        }
        else
        {
            if (other.CompareTag("Player") || other.CompareTag("Crystal") || other.CompareTag("MiniCrystal"))
            {

                float damage = GetComponent<Animal>().damage;
                Long.Attack(damage);
            }
        }
    }
}
