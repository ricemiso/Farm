using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �T�E���h���Ǘ�����N���X�B
/// </summary>
public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// SoundManager�̃C���X�^���X�B
    /// </summary>
    public static SoundManager Instance { get; set; }

    // ���ʉ�
    /// <summary>
    /// �A�C�e�����h���b�v���鉹�B
    /// </summary>
    public AudioSource dropItemSound;

    /// <summary>
    /// �N���t�g���B
    /// </summary>
    public AudioSource craftingSound;

    /// <summary>
    /// �c�[����U�鉹�B
    /// </summary>
    public AudioSource toolSwingSound;

    /// <summary>
    /// �`���b�v���B
    /// </summary>
    public AudioSource chopSound;

    /// <summary>
    /// �A�C�e�����E�����B
    /// </summary>
    public AudioSource PickUpItemSound;

    /// <summary>
    /// ���̏��������B
    /// </summary>
    public AudioSource grassWalkSound;

    /// <summary>
    /// �؂��|��鉹�B
    /// </summary>
    public AudioSource treeFallSound;

    /// <summary>
    /// �A�C�e����u�����B
    /// </summary>
    public AudioSource PutSeSound;

    /// <summary>
    /// �����̏��������B
    /// </summary>
    public AudioSource gravelWalkSound;

    /// <summary>
    /// ��b�̏��������B
    /// </summary>
    public AudioSource foundationWalkSound;

    /// <summary>
    /// �E�T�M�̖����B
    /// </summary>
    public AudioSource rabbitCrySound;

    /// <summary>
    /// �_��̏��������B
    /// </summary>
    public AudioSource FarmWalkSound;

    /// <summary>
    /// �H�ׂ鉹�B
    /// </summary>
    public AudioSource EatSound;

    /// <summary>
    /// �_���[�W���󂯂����B
    /// </summary>
    public AudioSource DamageSound;

    /// <summary>
    /// �N���X�^���U���̉��B
    /// </summary>
    public AudioSource CrystalAttack;

    /// <summary>
    /// �N���X�^�������鉹�B
    /// </summary>
    public AudioSource Crystalbreak;

    /// <summary>
    /// �΂����鉹�B
    /// </summary>
    public AudioSource Stonebreak;

    // BGM
    /// <summary>
    /// �X�^�[�g�]�[����BGM�B
    /// </summary>
    public AudioSource startingZoneBGMMusic;

    /// <summary>
    /// �Q�[���N���A��BGM�B
    /// </summary>
    public AudioSource gameClearBGM;

    /// <summary>
    /// �Q�[���I�[�o�[��BGM�B
    /// </summary>
    public AudioSource gameOverBGM;

    /// <summary>
    /// �G������BGM�B
    /// </summary>
    public AudioSource EnemyCreateBGM;

    public AudioSource TutorialBGM;

    /// <summary>
    /// �S�Ă�BGM�I�[�f�B�I�\�[�X�̃��X�g�B
    /// </summary>
    private List<AudioSource> allBGMAudioSources = new List<AudioSource>();

    /// <summary>
    /// �S�Ă̕��s���I�[�f�B�I�\�[�X�̃��X�g�B
    /// </summary>
    private List<AudioSource> allWalkAudioSources = new List<AudioSource>();

    /// <summary>
    /// �V���O���g���p�^�[����K�p���A�C���X�^���X�����������܂��B
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
    /// �����ݒ���s���܂��B
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
    /// �I�[�f�B�I�\�[�X��o�^���܂��B
    /// </summary>
    /// <param name="source">�I�[�f�B�I�\�[�X</param>
    public void RegisterAudioSource(AudioSource source)
    {
        if (!allBGMAudioSources.Contains(source))
        {
            allBGMAudioSources.Add(source);
        }
    }

    /// <summary>
    /// ���s���̃I�[�f�B�I�\�[�X��o�^���܂��B
    /// </summary>
    /// <param name="source">�I�[�f�B�I�\�[�X</param>
    public void WalkRegisterAudioSource(AudioSource source)
    {
        if (!allWalkAudioSources.Contains(source))
        {
            allWalkAudioSources.Add(source);
        }
    }

    /// <summary>
    /// �V�[�������[�h���ꂽ�Ƃ��̏����B
    /// </summary>
    /// <param name="scene">�V�[��</param>
    /// <param name="mode">���[�h���[�h</param>
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
    /// �I�u�W�F�N�g���j�������Ƃ��ɃC�x���g�������B
    /// </summary>
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// �T�E���h���Đ����܂��B
    /// </summary>
    /// <param name="soundToPlay">�Đ�����T�E���h</param>
    public void PlaySound(AudioSource soundToPlay)
    {
        if (!soundToPlay.isPlaying)
        {
            soundToPlay.Play();
        }
    }

    /// <summary>
    /// �T�E���h���~���܂��B
    /// </summary>
    /// <param name="soundToPlay">��~����T�E���h</param>
    public void StopSound(AudioSource soundToPlay)
    {
        if (soundToPlay.isPlaying)
        {
            soundToPlay.Stop();
        }
    }

    /// <summary>
    /// �S�Ă̕��s�����~���܂��B
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
    /// �S�Ă�BGM���~���܂��B
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
    /// ���̉��y���Đ�����Ă��Ȃ��ꍇ�ɉ��y���Đ����܂��B
    /// </summary>
    /// <param name="soundToPlay">�Đ����鉹�y</param>
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
