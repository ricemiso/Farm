using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrobalState : MonoBehaviour
{
    public static GrobalState Instance { get; set; }

    public float resourceHelth;
    public float resourceMaxHelth;
    public float resourceMana;
    public int level;
    public int damage;



    //チュートリアル用の変数
    public bool isTreeChopped = false;
    public bool isStoneChopped = false;
    public bool isTutorialEnd = false;


    private void Awake()
    {
        if(Instance != null &&Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }


    public void initiarize()
    {
        resourceHelth = 0;
        resourceMaxHelth = 0;
        resourceMana = 0;
        level = 0;
}
}
