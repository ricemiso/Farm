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
        float damage = GetComponentInParent<Animal>().damage;
        if (other.CompareTag("SupportUnit") || other.CompareTag("Player") || other.CompareTag("Crystal") || other.CompareTag("MiniCrystal"))
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
        }


    }
}
