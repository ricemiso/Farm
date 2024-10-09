using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float[] playerStats;
   
    public float[] playerPositionAndRotation;

    public string[] inventortContent;

    public string[] quickSlotContent;

    public PlayerData(float[] PlayerState, float[] PlayerPosAndRot,string[] InventortContent, string[] QuickSlotContent)
    {
        playerStats = PlayerState;
        playerPositionAndRotation = PlayerPosAndRot;
        inventortContent = InventortContent;
        quickSlotContent = QuickSlotContent;
    }
}
