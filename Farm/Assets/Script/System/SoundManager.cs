using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//担当者　越浦晃生

/// <summary>
/// サウンドを管理するクラス。
/// </summary>
public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// SoundManagerのインスタンス。
    /// </summary>
    public static SoundManager Instance { get; set; }

    // 効果音
    /// <summary>
    /// アイテムをドロップする音。
    /// </summary>
    public AudioSource dropItemSound;

    /// <summary>
    /// クラフト音。
    /// </summary>
    public AudioSource craftingSound;

    /// <summary>
    /// ツールを振る音。
    /// </summary>
    public AudioSource toolSwingSound;

    /// <summary>
    /// チョップ音。
    /// </summary>
    public AudioSource chopSound;

    /// <summary>
    /// アイテムを拾う音。
    /// </summary>
    public AudioSource PickUpItemSound;

    /// <summary>
    /// 草の上を歩く音。
    /// </summary>
    public AudioSource grassWalkSound;

    /// <summary>
    /// 木が倒れる音。
    /// </summary>
    public AudioSource treeFallSound;

    /// <summary>
    /// アイテムを置く音。
    /// </summary>
    public AudioSource PutSeSound;

    /// <summary>
    /// 砂利の上を歩く音。
    /// </summary>
    public AudioSource gravelWalkSound;

    /// <summary>
    /// 基礎の上を歩く音。
    /// </summary>
    public AudioSource foundationWalkSound;

    /// <summary>
    /// ウサギの鳴き声。
    /// </summary>
    public AudioSource rabbitCrySound;

    /// <summary>
    /// 農場の上を歩く音。
    /// </summary>
    public AudioSource FarmWalkSound;

    /// <summary>
    /// 食べる音。
    /// </summary>
    public AudioSource EatSound;

    /// <summary>
    /// ダメージを受けた音。
    /// </summary>
    public AudioSource DamageSound;

    /// <summary>
    /// クリスタル攻撃の音。
    /// </summary>
    public AudioSource CrystalAttack;

    /// <summary>
    /// クリスタルが壊れる音。
    /// </summary>
    public AudioSource Crystalbreak;

    /// <summary>
    /// 石が壊れる音。
    /// </summary>
    public AudioSource Stonebreak;

    // BGM
    /// <summary>
    /// スタートゾーンのBGM。
    /// </summary>
    public AudioSource startingZoneBGMMusic;

    /// <summary>
    /// ゲームクリアのBGM。
    /// </summary>
    public AudioSource gameClearBGM;

    /// <summary>
    /// ゲームオーバーのBGM。
    /// </summary>
    public AudioSource gameOverBGM;

    /// <summary>
    /// 敵生成のBGM。
    /// </summary>
    public AudioSource EnemyCreateBGM;

    public AudioSource TutorialBGM;

    /// <summary>
    /// 全てのBGMオーディオソースのリスト。
    /// </summary>
    private List<AudioSource> allBGMAudioSources = new List<AudioSource>();

    /// <summary>
    /// 全ての歩行音オーディオソースのリスト。
    /// </summary>
    private List<AudioSource> allWalkAudioSources = new List<AudioSource>();

    /// <summary>
    /// シングルトンパターンを適用し、インスタンスを初期化します。
    /// </summary>
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

    /// <summary>
    /// 初期設定を行います。
    /// </summary>
    private void Start()
    {
        RegisterAudioSource(startingZoneBGMMusic);
        RegisterAudioSource(gameClearBGM);
        RegisterAudioSource(gameOverBGM);
        RegisterAudioSource(EnemyCreateBGM);
        RegisterAudioSource(TutorialBGM);
        WalkRegisterAudioSource(grassWalkSound);
        WalkRegisterAudioSource(gravelWalkSound);
        WalkRegisterAudioSource(foundationWalkSound);
        WalkRegisterAudioSource(FarmWalkSound);

    }

    /// <summary>
    /// オーディオソースを登録します。
    /// </summary>
    /// <param name="source">オーディオソース</param>
    public void RegisterAudioSource(AudioSource source)
    {
        if (!allBGMAudioSources.Contains(source))
        {
            allBGMAudioSources.Add(source);
        }
    }

    /// <summary>
    /// 歩行音のオーディオソースを登録します。
    /// </summary>
    /// <param name="source">オーディオソース</param>
    public void WalkRegisterAudioSource(AudioSource source)
    {
        if (!allWalkAudioSources.Contains(source))
        {
            allWalkAudioSources.Add(source);
        }
    }

    /// <summary>
    /// シーンがロードされたときの処理。
    /// </summary>
    /// <param name="scene">シーン</param>
    /// <param name="mode">ロードモード</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameOver" || scene.name == "GameClear")
        {
            Destroy(gameObject);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }else if(scene.name == "MainScene")
        {
            StopSound(TutorialBGM);
            PlaySound(startingZoneBGMMusic);
        }
    }

    /// <summary>
    /// オブジェクトが破棄されるときにイベントを解除。
    /// </summary>
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// サウンドを再生します。
    /// </summary>
    /// <param name="soundToPlay">再生するサウンド</param>
    public void PlaySound(AudioSource soundToPlay)
    {
        if (!soundToPlay.isPlaying)
        {
            soundToPlay.Play();
        }
    }

    /// <summary>
    /// サウンドを停止します。
    /// </summary>
    /// <param name="soundToPlay">停止するサウンド</param>
    public void StopSound(AudioSource soundToPlay)
    {
        if (soundToPlay.isPlaying)
        {
            soundToPlay.Stop();
        }
    }

    /// <summary>
    /// 全ての歩行音を停止します。
    /// </summary>
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

    /// <summary>
    /// 全てのBGMを停止します。
    /// </summary>
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

    /// <summary>
    /// 他の音楽が再生されていない場合に音楽を再生します。
    /// </summary>
    /// <param name="soundToPlay">再生する音楽</param>
    public void PlayIfNoOtherMusic(AudioSource soundToPlay)
    {
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
