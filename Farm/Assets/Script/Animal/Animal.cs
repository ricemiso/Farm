using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public string animalName;
    public bool playerISRange;

    [SerializeField] int currentHealth;
    [SerializeField] int maxHelth;


    [SerializeField] AudioSource soundChannel;
    [SerializeField] AudioClip rabbithitAndScreem;
    [SerializeField] AudioClip rabitHitAndDie;

    enum AnimalType
    {
        Rabbit,
        PlantEnemy
    }

    [SerializeField] AnimalType thisAnimalType;


    private Animator animator;
    public bool isDead;
    [SerializeField] ParticleSystem bloodparticle;
    public GameObject bloodPaddle;

    public string GetAnimalName()
    {
        return animalName;
    }

    private void Start()
    {
        currentHealth = maxHelth;
        animator = GetComponent<Animator>();
    }


    public void TakeDamage(int damage)
    {
        if (isDead == false)
        {
            currentHealth -= damage;

            bloodparticle.Play();

            if (currentHealth <= 0)
            {
                PlayDyingSound();


                animator.SetTrigger("Die");
                GetComponent<AI_Movement>().enabled = false;
                StartCoroutine(puddleDelay());
                isDead = true;

            }
            else
            {
                PlayHitSound();

            }
        }
       
    }
    IEnumerator puddleDelay()
    {
        yield return new WaitForSeconds(1);
        bloodPaddle.SetActive(true);
    }

    private void PlayDyingSound()
    {
        switch (thisAnimalType)
        {
            case AnimalType.Rabbit:
                soundChannel.PlayOneShot(rabitHitAndDie);
                break;
            default:
                break;
        }
       
    }

    private void PlayHitSound()
    {
        switch (thisAnimalType)
        {
            case AnimalType.Rabbit:
                soundChannel.PlayOneShot(rabbithitAndScreem);
                break;
            default:
                break;
        }
       
    }

    
    //TODO:‹——£‚É•Ï‚¦‚½‚Ù‚¤‚ª—Ç‚¢‚©‚à
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerISRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerISRange = false;
        }
    }
}
