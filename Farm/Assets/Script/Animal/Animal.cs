using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public string animalName;
    public bool playerISRange;

    public float currentHealth;
    public float maxHealth;

    public bool isCraftedMinion = false;
    public int damage;

    public int healthIncrease = 20;
    public int damageIncrease = 5;
    public int level;
    public bool canBeChopped;



    [SerializeField] AudioSource soundChannel;
    [SerializeField] AudioClip rabbithitAndScreem;
    [SerializeField] AudioClip rabitHitAndDie;

    public enum AnimalType
    {
        Rabbit,
        PlantEnemy,
        Union
    }

    [SerializeField] AnimalType thisAnimalType;


    private Animator animator;
    private new Animation animation;
    public bool isDead;
    [SerializeField] ParticleSystem bloodparticle;
    [SerializeField] ParticleSystem levelupparticle;
    public GameObject bloodPaddle;

    public string GetAnimalName()
    {
        return animalName;
    }

    private void Start()
    {

        if (CraftingSystem.Instance.islevelUp)
        {
            // 現在のレベルに基づいてステータスを反映
            for (int i = 0; i < GrobalState.Instance.level; i++)
            {
                LevelUp(level);  // レベルに応じて繰り返しレベルアップ
            }

            GrobalState.Instance.level = 0;
        }
        else
        {
            // 初期状態を設定
            currentHealth = maxHealth;
            level = 0;
            if(damage == 0)
            {
                damage = 10;
            }

        }

        if (thisAnimalType == AnimalType.Union)
        {
            animation = GetComponent<Animation>();
        }
        else
        {
            animator = GetComponent<Animator>();
        }

    }

    private void Update()
    {
        if (canBeChopped)
        {
            GrobalState.Instance.resourceHelth = currentHealth;
            GrobalState.Instance.resourceMaxHelth = maxHealth;
        }
        
        
    }



    public void TakeDamage(float damage)
    {
        canBeChopped = true;
        if (isDead == false)
        {
            currentHealth -= damage;
            

            bloodparticle.Play();

            if (currentHealth <= 0)
            {
                PlayDyingSound();

                //Log.Instance.TriggerPickupPop(animalName);


                if (thisAnimalType == AnimalType.Union)
                {
                    animation.Play("Death");
                    DestroyImmediate(gameObject);
                }
                else
                {
					GetComponent<Rabbit>().enabled = false;
					animator.SetTrigger("Die");
                }

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
        if(bloodPaddle != null)
        {
            bloodPaddle.SetActive(true);
        }
        
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


    public void LevelUp(int nowlevel)
    {


        nowlevel += 1;
        level = nowlevel;

        int healthIncrease = 20 * nowlevel;
        int damageIncrease = 5 * nowlevel;


        maxHealth += healthIncrease;
        currentHealth = maxHealth;
        damage += damageIncrease;

        CraftingSystem.Instance.islevelUp = false;

        ParticleSystem partiSystem = levelupparticle;
		partiSystem.Play();

        Vector3 currentScale = gameObject.transform.localScale;

        // 1.6倍のスケールを計算
        Vector3 newScale = currentScale * 1.5f;

        // スケールを更新
        gameObject.transform.localScale = newScale;
    }
}
