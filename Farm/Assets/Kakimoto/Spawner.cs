using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
// 
// �G����������N���X
// ���̃X�N���v�g�������I�u�W�F�N�g����w�肵���͈͓��Ƀ����_���ŏ���
// 
//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

public class Spawner : MonoBehaviour
{
    // �G�̃��X�g
    [SerializeField] List<GameObject> m_EnemyList;

    // �����͈́i���S����͈͂����������̂܂ł̋����j
    [SerializeField] Vector3 m_Range;

    // �e�X�g�p
    int counter = 0;
    [SerializeField] int SummonTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
    //    // �e�X�g�p
    //    if (++counter > SummonTime)
    //    {
    //        counter -= SummonTime;
    //        SummonEnemy(2);
    //    }
    }

    // �w�肵�������X�g�����烉���_���ɓG����������
    // num = �G�̐�
    // health = ��Z����̗�
    // size = �傫��
    public void SummonEnemy(uint num, float health = 1.0f, float size = 1.0f)
    {
        for (uint cnt = 0; cnt < num; cnt++)
        {
            // ���W�̌���
            Vector3 gap;
            gap.x = Random.Range(-m_Range.x, m_Range.x);
            gap.y = Random.Range(-m_Range.y, m_Range.y);
            gap.z = Random.Range(-m_Range.z, m_Range.z);
            // ����
            GameObject obj = Instantiate(m_EnemyList[Random.Range(0, m_EnemyList.Count)],
                gap + this.transform.position, Quaternion.identity);
            obj.GetComponent<Animal>().maxHealth = (int)(obj.GetComponent<Animal>().maxHealth * health);
            obj.transform.localScale *= size;


		}
    }
}
