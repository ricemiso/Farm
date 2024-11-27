using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyMagic : MonoBehaviour
{
    [SerializeField] GameObject attack;
    LongRange longrange;

    // �e�̃_���[�W
    private float Damage;

    private void Start()
    {
        longrange = attack.gameObject.transform.GetComponent<LongRange>();
    }

    public void SetDamage(float damage)
    {
        Damage = damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Crystal") 
            || collision.gameObject.CompareTag("MiniCrystal"))
        {
            longrange.Attack(Damage, collision.gameObject);
            Destroy(this.gameObject);
        }
    }


}
