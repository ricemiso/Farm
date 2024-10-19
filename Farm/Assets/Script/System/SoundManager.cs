using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
   public static SoundManager Instance { get; set; }

    //Œø‰Ê‰¹
    public AudioSource dropItemSound;
    public AudioSource craftingSound;
    public AudioSource toolSwingSound;
    public AudioSource chopSound;
    public AudioSource PickUpItemSound;
    public AudioSource grassWalkSound;
    public AudioSource treeFallSound;
    public AudioSource PutSeSound;
    public AudioSource gravelWalkSound;

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
    }


    public void PlaySound(AudioSource soundToPlay)
    {
        if (!soundToPlay.isPlaying)
        {
            soundToPlay.Play();
        }
    }

}
