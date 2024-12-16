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



    //�`���[�g���A���p�̕ϐ�
    public bool isTreeChopped = false;
    public bool isStoneChopped = false;
    public bool isTutorialEnd = false;
    public bool isSkip = false;
    public bool isWater = false;
    public bool isDamage = false;
    public bool isloot = false;
    public bool isFarm1 = false;
    public bool isDeath = false;
    public bool isManaCraft = false;
   


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
