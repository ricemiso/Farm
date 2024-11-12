using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] GameObject seedModel;
    [SerializeField] GameObject youngPlantModel;
    [SerializeField] GameObject maturePlanetModel;

    [SerializeField] List<GameObject> plantProduceSpawn;

    [SerializeField] GameObject producePrefab;

    public int dayOfPlanting;
    [SerializeField] int plantage = 0;

    [SerializeField] int ageForYourModel;
    [SerializeField] int ageForMatureModel;
    [SerializeField] int ageForFirstProduceBatch;

    [SerializeField] int daysForNewProduce;
    [SerializeField] int daysRemainingForNewProduce;

    [SerializeField] bool isOneTimearvest;
    public bool isWatered = false;


    private void OnEnable()
    {
        TimeManager.Instance.oneDayPass.AddListener(DayPass);
    }

    private void OnDisable()
    {
        TimeManager.Instance.oneDayPass.RemoveListener(DayPass);
    }


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



    private void DayPass()
    {
        if (isWatered)
        {
            plantage++;

            isWatered = false;
            GetComponentInParent<Soil>().MakeSoilNotWatered();

            SphereCollider collider = GetComponent<SphereCollider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
        }

        CheckRroduce();


        if (!isOneTimearvest)
        {
            CheckGrows();
        }
       
    }

    private void CheckRroduce()
    {
        seedModel.SetActive(plantage < ageForFirstProduceBatch);
        youngPlantModel.SetActive(plantage >= ageForYourModel && plantage < ageForMatureModel);
        maturePlanetModel.SetActive(plantage >= ageForMatureModel);


        if(plantage >= ageForMatureModel && isOneTimearvest)
        {
            MakePlantPickable();
        }
    }

    private void MakePlantPickable()
    {
        GetComponent<InteractableObject>().enabled = true;
        GetComponent<SphereCollider>().enabled = true;
    }

    private void CheckGrows()
    {
       if(plantage == ageForFirstProduceBatch)
        {
            GenerateProduceForEmptySpawn();
        }

       if(plantage > ageForFirstProduceBatch)
        {
            if (daysRemainingForNewProduce == 0)
            {
                GenerateProduceForEmptySpawn();

                daysRemainingForNewProduce = daysForNewProduce;
            }
            else
            {
                daysRemainingForNewProduce--;
            }
        }
    }

    private void GenerateProduceForEmptySpawn()
    {
        foreach(GameObject spawn in plantProduceSpawn)
        {
            if (spawn.transform.childCount == 0)
            {
                GameObject produce = Instantiate(producePrefab);
                Destroy(this.gameObject, 24 * 10);
                Destroy(producePrefab, 24 * 10);

                produce.transform.parent = spawn.transform;

                Vector3 producePos = Vector3.zero;
                producePos.y = 0f;
                produce.transform.localPosition = producePos;

            }
        }


    }
}
