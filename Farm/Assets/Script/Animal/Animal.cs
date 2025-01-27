using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//担当者　越浦晃生

/// <summary>
/// AI全般に関わるクラス。レベルアップやダメージの処理、アイテムドロップを行う
/// </summary>
public class Animal : MonoBehaviour
{
    public string animalName;
    public bool playerISRange;
    
    public float currentHealth;
    public float maxHealth;

    public bool isCraftedMinion = false;
    public float damage;

    public int healthIncrease = 0;
    public int damageIncrease = 0;
    public int level;
    public bool canBeChopped;
    public bool canbeWatch;


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

    [SerializeField]public AnimalType thisAnimalType;


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

   /// <summary>
   /// 初期化
   /// </summary>
    private void Start()
    {
        unionForceStrength = 10;
        enemyForceStrength = 10;

        canbeWatch = false;

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

    /// <summary>
    /// 体力を更新する
    /// </summary>
    private void Update()
    {
        // UIに体力情報を表示
        if (canBeChopped ||canbeWatch)
        {
            GrobalState.Instance.resourceHelth = currentHealth;
            GrobalState.Instance.resourceMaxHelth = maxHealth;
        }
        canBeChopped = false;
        canbeWatch = false;

        if (isDead)
        {
            if(animator != null)
            {
                animator.enabled = false;
            }
           
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
                Lootable loot = gameObject.GetComponent<Lootable>();
                StartCoroutine(DelayedLoot(loot));

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
                    if(animator != null)
                    {
                        animator.ResetTrigger("Run");
                        animator.ResetTrigger("Fire");
                        animator.SetTrigger("Die"); // "Die"はAnimator内で設定したTrigger名

                        // 倒されたら全ての付き物を外す
                        gameObject.GetComponent<LongRange>().enabled = false;

                        if (gameObject.GetComponentInChildren<LongRangeAttackBox>())
                        {
                            Destroy(gameObject.GetComponentInChildren<LongRangeAttackBox>().gameObject);
                        }
                    }
                }

                else
                {
                    GetComponent<Rabbit>().enabled = false;
                    animator.SetTrigger("Die");
                    GetComponentInChildren<AttackBoxRabbit>().enabled = false;
                }


                if (thisAnimalType != AnimalType.Union)
                {
                    StartCoroutine(puddleDelay());
                }
                   
                isDead = true;
                GrobalState.Instance.isDeath = true;


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
    /// <param name="manacnt">使用するマナの数</param>
    public void LevelUp(int manacnt)
    {
       

        for (int i = 0; i < manacnt; i++)
        {
            if (level >= 3) return;

            //int HealthIncrease = healthIncrease * (level+1);
            //int DamageIncrease = damageIncrease * (level + 1);

            maxHealth += healthIncrease;
            currentHealth = maxHealth;
            damage += damageIncrease;

            CraftingSystem.Instance.islevelUp = false;

            // パーティクルを再生
            ParticleSystem partiSystem = levelupparticle;
            partiSystem.Play();

            // 現在のスケールを取得
            Vector3 currentScale = gameObject.transform.localScale;

            // スケールを1.2倍にする
            Vector3 newScale = currentScale * 1.2f;
            gameObject.transform.localScale = newScale;
            level++;
        }
    }

    /// <summary>
    /// 敵からのドロップを行うまでの遅延(アニメーションの時間によって変更)
    /// </summary>
    /// <param name="loot"></param>
    /// <returns></returns>
    private IEnumerator DelayedLoot(Lootable loot)
    {

        yield return new WaitForSeconds(1.5f);

        Loot(loot); // 遅延後にLootを呼び出す

    }

    /// <summary>
    /// ランダムな範囲にドロップアイテムを指定された個数をランダムにポップさせる
    /// </summary>
    /// <param name="lootable"></param>
    private void Loot(Lootable lootable)
    {
        if (lootable.wasLootCalulated == false)
        {
            List<LootRecieved> lootRecieveds = new List<LootRecieved>();

            foreach (LootPossibility loot in lootable.possibilities)
            {
                var lootAmount = UnityEngine.Random.Range(loot.amountMin, loot.amountMax + 1);
                if (lootAmount > 0)
                {
                    LootRecieved It = new LootRecieved();
                    It.item = loot.item;
                    It.amount = lootAmount;

                    lootRecieveds.Add(It);
                }
            }

            lootable.finalLoot = lootRecieveds;
            lootable.wasLootCalulated = true;
        }

        Vector3 lootSpawnPosition = lootable.gameObject.transform.position;
        float scatterRange = 2.0f; // 散らばりの範囲を設定

        foreach (LootRecieved lootRecieved in lootable.finalLoot)
        {
            for (int i = 0; i < lootRecieved.amount; i++)
            {
                // ランダムな位置を生成 (X-Zのみ散らばり、Yは固定)
                Vector3 randomOffset = new Vector3(
                    UnityEngine.Random.Range(-scatterRange, scatterRange), // X方向
                    1,                                         // Y方向はそのまま
                    UnityEngine.Random.Range(-scatterRange, scatterRange)  // Z方向
                );

                Vector3 spawnPosition = lootSpawnPosition + randomOffset;

                // アイテムをスポーン
                GameObject lootSpawn = Instantiate(
                    Resources.Load<GameObject>(lootRecieved.item.name + "_model"),
                    spawnPosition,
                    Quaternion.identity
                );

            }
        }

        //if (lootable.GetComponent<Animal>())
        //{
        //	lootable.GetComponent<Animal>().bloodPaddle.transform.SetParent(lootable.transform.parent);
        //}

        Destroy(lootable.gameObject);
    }

}

