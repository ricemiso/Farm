using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float[] playerStats;
   
    public float[] playerPositionAndRotation;

    //public float[] inventortContent;

    public PlayerData(float[] PlayerState, float[] PlayerPosAndRot)
    {
        playerStats = PlayerState;
        playerPositionAndRotation = PlayerPosAndRot;
    }
}
