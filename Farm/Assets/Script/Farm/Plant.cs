using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 植物を制御するクラス。成長や収穫の管理
/// </summary>
public class Plant : MonoBehaviour
{
    [SerializeField] GameObject seedModel; // 種のモデル
    [SerializeField] GameObject youngPlantModel; // 若い植物のモデル
    [SerializeField] GameObject maturePlanetModel; // 成熟した植物のモデル

    [SerializeField] List<GameObject> plantProduceSpawn; // 生成物をスポーンさせる場所のリスト

    [SerializeField] GameObject producePrefab; // 生成するプロダクトのプレハブ

    public int dayOfPlanting; // 植えた日
    [SerializeField] int plantage = 0; // 植物の現在の年齢

    [SerializeField] int ageForYourModel; // 若い植物モデルへの切り替え年齢
    [SerializeField] int ageForMatureModel; // 成熟モデルへの切り替え年齢
    [SerializeField] int ageForFirstProduceBatch; // 最初の生成物が生成される年齢

    [SerializeField] int daysForNewProduce; // 次の生成物が生成されるまでの日数
    [SerializeField] int daysRemainingForNewProduce; // 次の生成物生成までの残り日数

    [SerializeField] bool isOneTimearvest; // 収穫が一度だけかどうか

    private bool hasGeneratedProduce = false; // 生成物が生成されたかどうか

    /// <summary>
    /// オブジェクトが有効化されたときにリスナーを登録します。
    /// </summary>
    private void OnEnable()
    {
        TimeManager.Instance.oneDayPass.AddListener(DayPass);
    }

    /// <summary>
    /// オブジェクトが無効化されたときにリスナーを削除します。
    /// </summary>
    private void OnDisable()
    {
        TimeManager.Instance.oneDayPass.RemoveListener(DayPass);
    }

    /// <summary>
    /// オブジェクトが破棄されたときの処理。
    /// 親のSoilコンポーネントをリセットします。
    /// </summary>
    private void OnDestroy()
    {
        Soil soil = GetComponentInParent<Soil>();
        if (soil != null)
        {
            soil.isEmpty = true;
            soil.plantName = "";
            soil.currentplant = null;
        }
    }

    /// <summary>
    /// 植物を成長させます。
    /// </summary>
    public void Grow()
    {
        plantage++;

        // 土を水がない状態に変更
        GetComponentInParent<Soil>().MakeSoilNotWatered();

        // コライダーを無効化
        SphereCollider collider = GetComponent<SphereCollider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        CheckRroduce();
    }

    /// <summary>
    /// 一日経過時の処理を行います。
    /// </summary>
    public void DayPass()
    {
        if (!isOneTimearvest)
        {
            if (CheckGrows())
            {
                // 収穫物を生成
                // GenerateProduceForEmptySpawn();
            }
            else
            {
                Grow();
            }
        }
        else
        {
            GetComponentInParent<Soil>().MakeSoilNotWatered();
        }
    }

    /// <summary>
    /// 毎フレームの更新処理。
    /// </summary>
    private void Update()
    {
        if (CheckGrows() && !hasGeneratedProduce)
        {
            GenerateProduceForEmptySpawn();
            hasGeneratedProduce = true; // 処理済みフラグを設定
        }
    }

    /// <summary>
    /// 現在の植物のモデルを切り替える処理。
    /// </summary>
    private void CheckRroduce()
    {
        seedModel.SetActive(plantage < ageForFirstProduceBatch);
        youngPlantModel.SetActive(plantage >= ageForYourModel && plantage < ageForMatureModel);
        maturePlanetModel.SetActive(plantage >= ageForMatureModel);

        if (plantage >= ageForMatureModel && isOneTimearvest)
        {
            MakePlantPickable();
        }
    }

    /// <summary>
    /// 植物をインタラクト可能にします。
    /// </summary>
    private void MakePlantPickable()
    {
        GetComponent<InteractableObject>().enabled = true;
        GetComponent<SphereCollider>().enabled = true;
    }

    /// <summary>
    /// 植物が成長しきっているかどうかをチェックします。
    /// </summary>
    /// <returns>成長しきっていればtrueを返します。</returns>
    public bool CheckGrows()
    {
        if (plantage >= ageForFirstProduceBatch) return true;
        return false;
    }

    /// <summary>
    /// 空のスポーンポイントに生成物を生成します。
    /// </summary>
    private void GenerateProduceForEmptySpawn()
    {
        foreach (GameObject spawn in plantProduceSpawn)
        {
            if (spawn.transform.childCount == 0)
            {
                GameObject produce = Instantiate(producePrefab);
                Destroy(this.gameObject, 60); // このオブジェクトを60秒後に破棄
                Destroy(producePrefab, 60); // プレハブを60秒後に破棄

                produce.transform.parent = spawn.transform;

                Vector3 producePos = Vector3.zero;
                producePos.y = 0f;
                produce.transform.localPosition = producePos;
            }
        }
    }
}
