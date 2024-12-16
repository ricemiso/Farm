using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    //���ʉ�
    public AudioSource dropItemSound;
    public AudioSource craftingSound;
    public AudioSource toolSwingSound;
    public AudioSource chopSound;
    public AudioSource PickUpItemSound;
    public AudioSource grassWalkSound;
    public AudioSource treeFallSound;
    public AudioSource PutSeSound;
    public AudioSource gravelWalkSound;
    public AudioSource foundationWalkSound;
    public AudioSource rabbitCrySound;
    public AudioSource FarmWalkSound;
    public AudioSource EatSound;
    public AudioSource DamageSound;
    public AudioSource CrystalAttack;
    public AudioSource Crystalbreak;
    public AudioSource Stonebreak;


    //BGM
    public AudioSource startingZoneBGMMusic;
    public AudioSource gameClearBGM;
    public AudioSource gameOverBGM;
    public AudioSource EnemyCreateBGM;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (scene.name == "GameOver" || scene.name == "GameClear")
        {
            Destroy(gameObject);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnDestroy()
    {
        // �I�u�W�F�N�g���j�������Ƃ��ɃC�x���g������
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void PlaySound(AudioSource soundToPlay)
    {
        if (!soundToPlay.isPlaying)
        {
            soundToPlay.Play();
        }
    }

    public void StopSound(AudioSource soundToPlay)
    {
        if (soundToPlay.isPlaying)
        {
            soundToPlay.Stop();
        }
    }
}
