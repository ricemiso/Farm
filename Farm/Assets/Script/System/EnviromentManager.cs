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
    //    // ����̃V�[�����i�Ⴆ�� "Start"�j�̏ꍇ�� SoundManager ��j��
    //    if (scene.name == "GameOver" || scene.name == "GameClear")
    //    {
    //        Destroy(gameObject);
    //        SceneManager.sceneLoaded -= OnSceneLoaded; // �C�x���g������
    //    }
    //}

    //private void OnDestroy()
    //{
    //    // �I�u�W�F�N�g���j�������Ƃ��ɃC�x���g������
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}
}

