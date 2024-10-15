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

    [SerializeField] int dayOfPlanting;
    [SerializeField] int plantage = 0;

    [SerializeField] int ageForYourModel;
    [SerializeField] int ageForMatureModel;
    [SerializeField] int ageForFirstProduceBatch;

    [SerializeField] int daysForNewProduce;
    [SerializeField] int daysRemainingForNewProduce;

    [SerializeField] bool isOneTimearvest;
    [SerializeField] bool isWatered;

}
