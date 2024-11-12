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
    [SerializeField] List<GameObject> EnemyList;

    // 召喚範囲（中心から範囲を示す直方体までの距離）
    [SerializeField] Vector3 Range;


	// 召喚した敵に渡すクリスタルへの参照
	[SerializeField] GameObject Crystal;
	// 召喚した敵に渡すミニクリスタルへの参照
	[SerializeField] GameObject CrystalMini;

	// 敵リストへの参照
	[SerializeField] GameObject EnemyParent;


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
    public void SummonEnemy(uint num, float health = 1.0f, float damageRate = 1.0f, float size = 1.0f)
    {
        for (uint cnt = 0; cnt < num; cnt++)
        {
            // 座標の決定
            Vector3 gap;
            gap.x = Random.Range(-Range.x, Range.x);
            gap.y = Random.Range(-Range.y, Range.y);
            gap.z = Random.Range(-Range.z, Range.z);
            // 召喚
            GameObject obj = Instantiate(EnemyList[Random.Range(0, EnemyList.Count)],
                gap + this.transform.position, Quaternion.identity);
			obj.transform.SetParent(EnemyParent.transform); // 敵リストに登録
			obj.GetComponent<Animal>().maxHealth = (int)(obj.GetComponent<Animal>().maxHealth * health);
			obj.GetComponent<Animal>().damage = (int)(obj.GetComponent<Animal>().damage * damageRate);
			obj.transform.localScale *= size;

            EnemyAI_Movement ai = obj.GetComponent<EnemyAI_Movement>();
            ai.Crystal = Crystal;
            ai.CrystalMini = CrystalMini;
		}
    }
}
