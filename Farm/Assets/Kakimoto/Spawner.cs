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
    [SerializeField] List<GameObject> EnemyList;

    // �����͈́i���S����͈͂����������̂܂ł̋����j
    [SerializeField] Vector3 Range;


	// ���������G�ɓn���N���X�^���ւ̎Q��
	[SerializeField] GameObject Crystal;
	// ���������G�ɓn���~�j�N���X�^���ւ̎Q��
	[SerializeField] GameObject CrystalMini;

	// �G���X�g�ւ̎Q��
	[SerializeField] GameObject EnemyParent;


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
    public void SummonEnemy(uint num, float health = 1.0f, float damageRate = 1.0f, float size = 1.0f)
    {
        for (uint cnt = 0; cnt < num; cnt++)
        {
            // ���W�̌���
            Vector3 gap;
            gap.x = Random.Range(-Range.x, Range.x);
            gap.y = Random.Range(-Range.y, Range.y);
            gap.z = Random.Range(-Range.z, Range.z);
            // ����
            GameObject obj = Instantiate(EnemyList[Random.Range(0, EnemyList.Count)],
                gap + this.transform.position, Quaternion.identity);
			obj.transform.SetParent(EnemyParent.transform); // �G���X�g�ɓo�^
			obj.GetComponent<Animal>().maxHealth = (int)(obj.GetComponent<Animal>().maxHealth * health);
			obj.GetComponent<Animal>().damage = (int)(obj.GetComponent<Animal>().damage * damageRate);
			obj.transform.localScale *= size;

            EnemyAI_Movement ai = obj.GetComponent<EnemyAI_Movement>();
            ai.Crystal = Crystal;
            ai.CrystalMini = CrystalMini;
		}
    }
}
