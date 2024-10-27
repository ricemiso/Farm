using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentManager : MonoBehaviour
{
    public static EnviromentManager Instance { get; private set; }

    public GameObject allItems;

    public GameObject allTrees;

    public GameObject allPlaceItem;

    public GameObject allStones;

    public GameObject Storage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

