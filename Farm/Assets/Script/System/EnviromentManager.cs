using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnviromentManager : MonoBehaviour
{
    public static EnviromentManager Instance { get; private set; }

    public GameObject allItems;

    public GameObject allTrees;

    public GameObject allPlaceItem;

    public GameObject allStones;

    public GameObject Crystal;

    public GameObject Storage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    // 特定のシーン名（例えば "Start"）の場合に SoundManager を破棄
    //    if (scene.name == "GameOver" || scene.name == "GameClear")
    //    {
    //        Destroy(gameObject);
    //        SceneManager.sceneLoaded -= OnSceneLoaded; // イベントを解除
    //    }
    //}

    //private void OnDestroy()
    //{
    //    // オブジェクトが破棄されるときにイベントを解除
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}
}

