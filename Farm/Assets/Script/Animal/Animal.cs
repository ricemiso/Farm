using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�S���ҁ@�z�Y�W��

/// <summary>
/// AI�S�ʂɊւ��N���X�B���x���A�b�v��_���[�W�̏����A�A�C�e���h���b�v���s��
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
    /// �����̎�ނ��`����񋓌^
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
    /// �����̖��O���擾
    /// </summary>
    /// <returns>�����̖��O</returns>
    public string GetAnimalName()
    {
        return animalName;
    }

   /// <summary>
   /// ������
   /// </summary>
    private void Start()
    {
        unionForceStrength = 10;
        enemyForceStrength = 10;

        canbeWatch = false;

        if (CraftingSystem.Instance.islevelUp)
        {
            // ���݂̃��x���Ɋ�Â��ăX�e�[�^�X�𔽉f
            for (int i = 0; i < GrobalState.Instance.level; i++)
            {
                LevelUp(level);  // ���x���ɉ����ČJ��Ԃ����x���A�b�v
            }

            GrobalState.Instance.level = 0;
        }
        else
        {
            // ������Ԃ�ݒ�
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
    /// �̗͂��X�V����
    /// </summary>
    private void Update()
    {
        // UI�ɑ̗͏���\��
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
    /// �m�b�N�o�b�N
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

            // �͂�������
            rb.AddForce(forceDirection * force, ForceMode.Impulse);
        }
        else
        {
            Debug.LogWarning("Rigidbody �R���|�[�l���g��������܂���");
        }
    }

    /// <summary>
    /// �_���[�W���󂯂鏈��
    /// </summary>
    /// <param name="damage">�󂯂�_���[�W��</param>
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

                // �S�u����
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

                // �E���t
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

                // �h���S��
                else if (thisAnimalType == AnimalType.Dragon)
                {
                    if(animator != null)
                    {
                        animator.ResetTrigger("Run");
                        animator.ResetTrigger("Fire");
                        animator.SetTrigger("Die"); // "Die"��Animator���Őݒ肵��Trigger��

                        // �|���ꂽ��S�Ă̕t�������O��
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
    /// ����\������܂ł̒x������
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
    /// ���S���̃T�E���h���Đ�
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
    /// ��e���̃T�E���h���Đ�
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
    /// �v���C���[���͈͓��ɓ������ۂ̏���
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerISRange = true;
        }
    }

    /// <summary>
    /// �v���C���[���͈͊O�ɏo���ۂ̏���
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerISRange = false;
        }
    }

    /// <summary>
    /// �~�j�I�������x���A�b�v������
    /// </summary>
    /// <param name="manacnt">�g�p����}�i�̐�</param>
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

            // �p�[�e�B�N�����Đ�
            ParticleSystem partiSystem = levelupparticle;
            partiSystem.Play();

            // ���݂̃X�P�[�����擾
            Vector3 currentScale = gameObject.transform.localScale;

            // �X�P�[����1.2�{�ɂ���
            Vector3 newScale = currentScale * 1.2f;
            gameObject.transform.localScale = newScale;
            level++;
        }
    }

    /// <summary>
    /// �G����̃h���b�v���s���܂ł̒x��(�A�j���[�V�����̎��Ԃɂ���ĕύX)
    /// </summary>
    /// <param name="loot"></param>
    /// <returns></returns>
    private IEnumerator DelayedLoot(Lootable loot)
    {

        yield return new WaitForSeconds(1.5f);

        Loot(loot); // �x�����Loot���Ăяo��

    }

    /// <summary>
    /// �����_���Ȕ͈͂Ƀh���b�v�A�C�e�����w�肳�ꂽ���������_���Ƀ|�b�v������
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
        float scatterRange = 2.0f; // �U��΂�͈̔͂�ݒ�

        foreach (LootRecieved lootRecieved in lootable.finalLoot)
        {
            for (int i = 0; i < lootRecieved.amount; i++)
            {
                // �����_���Ȉʒu�𐶐� (X-Z�̂ݎU��΂�AY�͌Œ�)
                Vector3 randomOffset = new Vector3(
                    UnityEngine.Random.Range(-scatterRange, scatterRange), // X����
                    1,                                         // Y�����͂��̂܂�
                    UnityEngine.Random.Range(-scatterRange, scatterRange)  // Z����
                );

                Vector3 spawnPosition = lootSpawnPosition + randomOffset;

                // �A�C�e�����X�|�[��
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

