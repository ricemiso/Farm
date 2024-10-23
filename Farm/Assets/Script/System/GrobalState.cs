using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrobalState : MonoBehaviour
{
    public static GrobalState Instance { get; set; }

    public float resourceHelth;
    public float resourceMaxHelth;
    public float resourceMana;

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
    }
}
