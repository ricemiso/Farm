using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
   public static SoundManager Instance { get; set; }

    //効果音
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

    //public AudioSource WateringSound;
    //public AudioSource WateringCanSound;

    //BGM
    public AudioSource startingZoneBGMMusic;

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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        if (scene.name == "GameOver"|| scene.name == "GameClear")
        {
            Destroy(gameObject);
            SceneManager.sceneLoaded -= OnSceneLoaded; 
        }
    }

    private void OnDestroy()
    {
        // オブジェクトが破棄されるときにイベントを解除
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void PlaySound(AudioSource soundToPlay)
    {
        if (!soundToPlay.isPlaying)
        {
            soundToPlay.Play();
        }
    }

}
