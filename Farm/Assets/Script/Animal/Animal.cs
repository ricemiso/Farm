using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI全般に関わるクラス。レベルアップやダメージの処理
/// </summary>

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


    [SerializeField] float unionForceStrength;
	[SerializeField] float enemyForceStrength;
	[SerializeField] AudioSource soundChannel;
    [SerializeField] AudioClip rabbithitAndScreem;
    [SerializeField] AudioClip rabitHitAndDie;

    /// <summary>
    /// 動物の種類を定義する列挙型
    /// </summary>
    public enum AnimalType
    {
        Rabbit,
        PlantEnemy,
        Union,
        Goblin,
        Wolf,
        Dragon
    }

    [SerializeField] AnimalType thisAnimalType;


    private Animator animator;
    private new Animation animation;
    public bool isDead;
    [SerializeField] ParticleSystem bloodparticle;
    [SerializeField] ParticleSystem levelupparticle;
    public GameObject bloodPaddle;

    /// <summary>
    /// 動物の名前を取得
    /// </summary>
    /// <returns>動物の名前</returns>
    public string GetAnimalName()
    {
        return animalName;
    }

   
    private void Start()
    {
        unionForceStrength = 10;
        enemyForceStrength = 10;

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

        if (thisAnimalType == AnimalType.Union || thisAnimalType == AnimalType.Goblin || thisAnimalType == AnimalType.Wolf)
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
        // UIに体力情報を表示
        if (canBeChopped)
        {
            GrobalState.Instance.resourceHelth = currentHealth;
            GrobalState.Instance.resourceMaxHelth = maxHealth;
        }

    }

    /// <summary>
    /// ノックバック
    /// </summary>
    public void Force()
    {
        float force;

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            Vector3 forceDirection = -transform.forward + transform.up * 0.001f;
            if (thisAnimalType == AnimalType.Union)
            {
				 force = unionForceStrength;
            }
            else
            {
				force = enemyForceStrength;
			}

            // 力を加える
            rb.AddForce(forceDirection * force, ForceMode.Impulse);
        }
        else
        {
            Debug.LogWarning("Rigidbody コンポーネントが見つかりません");
        }
    }

    /// <summary>
    /// ダメージを受ける処理
    /// </summary>
    /// <param name="damage">受けるダメージ量</param>
    public void TakeDamage(float damage)
    {
       // if (!canBeChopped) return;
       // if (!EquipSystem.Instance.SwingWait) return;

        //canBeChopped = true;
        if (isDead == false)
        {
            currentHealth -= damage;
            GrobalState.Instance.resourceHelth = currentHealth;
            GrobalState.Instance.resourceMaxHelth = maxHealth;
            Force();

            Rabbit ai = gameObject.GetComponent<Rabbit>();
            if (ai != null)
            {
                ai.IsChaseSupportUnit = true;
                ai.IsChasePlayer = true;
            }

            bloodparticle.Play();

            if (currentHealth <= 0)
            {
                PlayDyingSound();

                //Log.Instance.TriggerPickupPop(animalName);

                if (thisAnimalType == AnimalType.Union)
                {
                    animation.Play("Death");
                    Destroy(this.gameObject);
                    gameObject.SetActive(false);
                }

                // ゴブリン
                else if (thisAnimalType == AnimalType.Goblin)
                {
                    if (animator == null)
                    {
                        if (animation.GetClip("dead") != null)
                        {
                            GetComponent<Rabbit>().enabled = false;
                            //GetComponentInChildren<AttackBoxRabbit>().enabled = false;
                            animation.Play("dead");
                            
                        }

                    }
                }

                // ウルフ
                else if (thisAnimalType == AnimalType.Wolf)
                {
                    if (animator == null)
                    {
                        if (animation.GetClip("dead") != null)
                        {
                            GetComponent<Rabbit>().enabled = false;
                            //GetComponentInChildren<AttackBoxRabbit>().enabled = false;
                            animation.Play("dead");
                            //animation.enabled = false;
                        }

                    }
                }

                // ドラゴン
                else if (thisAnimalType == AnimalType.Dragon)
                {
                    if (animator != null)
                    {
                        animator.SetTrigger("Die"); // "Die"はAnimator内で設定したTrigger名
                    }
                    else
                    {
                        Debug.LogWarning("Animatorが見つかりません");
                    }
                }

                else
                {
                    GetComponent<Rabbit>().enabled = false;
                    animator.SetTrigger("Die");
                    GetComponentInChildren<AttackBoxRabbit>().enabled = false;
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

    /// <summary>
    /// 血を表示するまでの遅延処理
    /// </summary>
    IEnumerator puddleDelay()
    {
        yield return new WaitForSeconds(1);
        if (bloodPaddle != null)
        {
            bloodPaddle.SetActive(true);
            //yield return new WaitForSeconds(3);
            //Destroy(bloodPaddle);
            //StartCoroutine(puddleDelay2());
        }

    }


    /// <summary>
    /// 死亡時のサウンドを再生
    /// </summary>
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

    /// <summary>
    /// 被弾時のサウンドを再生
    /// </summary>
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

    /// <summary>
    /// プレイヤーが範囲内に入った際の処理
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerISRange = true;
        }
    }

    /// <summary>
    /// プレイヤーが範囲外に出た際の処理
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerISRange = false;
        }
    }

    /// <summary>
    /// ミニオンをレベルアップさせる
    /// </summary>
    /// <param name="nowlevel">現在のレベル</param>
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
