using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
// 
// 敵を召喚するクラス
// このスクリプトをつけたオブジェクトから指定した範囲内にランダムで召喚
// 
//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

public class Spawner : MonoBehaviour
{
    // 敵のリスト
    [SerializeField] List<GameObject> m_EnemyList;

    // 召喚範囲（中心から範囲を示す直方体までの距離）
    [SerializeField] Vector3 m_Range;

    // テスト用
    int counter = 0;
    [SerializeField] int SummonTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
    //    // テスト用
    //    if (++counter > SummonTime)
    //    {
    //        counter -= SummonTime;
    //        SummonEnemy(2);
    //    }
    }

    // 指定した数リスト内からランダムに敵を召喚する
    // num = 敵の数
    // health = 乗算する体力
    // size = 大きさ
    public void SummonEnemy(uint num, float health = 1.0f, float size = 1.0f)
    {
        for (uint cnt = 0; cnt < num; cnt++)
        {
            // 座標の決定
            Vector3 gap;
            gap.x = Random.Range(-m_Range.x, m_Range.x);
            gap.y = Random.Range(-m_Range.y, m_Range.y);
            gap.z = Random.Range(-m_Range.z, m_Range.z);
            // 召喚
            GameObject obj = Instantiate(m_EnemyList[Random.Range(0, m_EnemyList.Count)],
                gap + this.transform.position, Quaternion.identity);
            obj.GetComponent<Animal>().maxHealth = (int)(obj.GetComponent<Animal>().maxHealth * health);
            obj.transform.localScale *= size;


		}
    }
}
