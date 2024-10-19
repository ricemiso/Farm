using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour
{
    public bool isEmpty = true;

    public bool playerInRange;
    public string plantName;

    public Plant currentplant;

    public Material defaltMaterial;
    public Material waterMaterial;

    private void Update()
    {
        //Todo:‹——£‚ğ“¾‚é•û–@
        float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

        if (distance < 10f)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }
    }

    internal void PlantSeed()
    {
        InventoryItem selectedSeed = EquipSystem.Instance.selectedItem.GetComponent<InventoryItem>();
        isEmpty = false;

        //TODO:“ú–{Œê‚ÉC³‚·‚éSwitch•¶‚ğ‘‚­
        string onlyPlantName = selectedSeed.thisName.Split(new string[] { "‚Ìí" }, StringSplitOptions.None)[0];

        plantName = onlyPlantName;


        GameObject instancePlant = Instantiate(Resources.Load($"{onlyPlantName}Plant") as GameObject);

        instancePlant.transform.parent = gameObject.transform;
        Vector3 plantPos = Vector3.zero;
        plantPos.y = 0f;
        instancePlant.transform.localPosition = plantPos;


        currentplant = instancePlant.GetComponent<Plant>();
        currentplant.dayOfPlanting = TimeManager.Instance.dayInGame;
    }

    internal void MakeSoilWatered()
    {
        GetComponent<Renderer>().material = waterMaterial;
    }

    internal void MakeSoilNotWatered()
    {
        GetComponent<Renderer>().material = defaltMaterial;
    }
}
