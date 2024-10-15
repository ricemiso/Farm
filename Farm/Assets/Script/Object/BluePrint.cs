using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePrint : MonoBehaviour
{
    public string itemName;

    public string Req1;
    public string Req2;

    public int Req1amount;
    public int Req2amount;

    public int numOfRequirement;

    public int numberOfItemsToProduce;


    public BluePrint(string name, int producedItems, int reqNum, string R1, int R1Num, string R2, int R2Num)
    {
        itemName = name;

        numOfRequirement = reqNum;

        numberOfItemsToProduce = producedItems;

        Req1 = R1;
        Req2 = R2;

        Req1amount = R1Num;
        Req2amount = R2Num;
    }
}
