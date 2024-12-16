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
    public AudioSource DamageSound;
    public AudioSource CrystalAttack;
    public AudioSource Crystalbreak;
    public AudioSource Stonebreak;


    //BGM
    public AudioSource startingZoneBGMMusic;
    public AudioSource gameClearBGM;
    public AudioSource gameOverBGM;
    public AudioSource EnemyCreateBGM;


    private List<AudioSource> allBGMAudioSources = new List<AudioSource>();
    private List<AudioSource> allWalkAudioSources = new List<AudioSource>();

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


    private void Start()
    {
        RegisterAudioSource(startingZoneBGMMusic);
        RegisterAudioSource(gameClearBGM);
        RegisterAudioSource(gameOverBGM);
        RegisterAudioSource(EnemyCreateBGM);
        WalkRegisterAudioSource(grassWalkSound);
        WalkRegisterAudioSource(gravelWalkSound);
        WalkRegisterAudioSource(foundationWalkSound);
        WalkRegisterAudioSource(FarmWalkSound);
    }

    public void RegisterAudioSource(AudioSource source)
    {
        if (!allBGMAudioSources.Contains(source))
        {
            allBGMAudioSources.Add(source);
        }
    }

    public void WalkRegisterAudioSource(AudioSource source)
    {
        if (!allWalkAudioSources.Contains(source))
        {
            allWalkAudioSources.Add(source);
        }
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

    public void StopSound(AudioSource soundToPlay)
    {
        if (soundToPlay.isPlaying)
        {
            soundToPlay.Stop();
        }
    }

    public void StopWalkSound()
    {
        foreach (AudioSource source in allWalkAudioSources)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
    }

    public void StopBGMSound()
    {
        foreach (AudioSource source in allBGMAudioSources)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
    }

    public void PlayIfNoOtherMusic(AudioSource soundToPlay)
    {
        // 現在再生中の音楽があるかを確認
        foreach (AudioSource source in allBGMAudioSources)
        {
            if (source.isPlaying)
            {
                return;
            }
        }

        soundToPlay.Play();
    }
}
