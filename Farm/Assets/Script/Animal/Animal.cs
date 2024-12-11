using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI�S�ʂɊւ��N���X�B���x���A�b�v��_���[�W�̏���
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

    [SerializeField] AnimalType thisAnimalType;


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

   
    private void Start()
    {
        unionForceStrength = 10;
        enemyForceStrength = 10;

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

    private void Update()
    {
        // UI�ɑ̗͏���\��
        if (canBeChopped)
        {
            GrobalState.Instance.resourceHelth = currentHealth;
            GrobalState.Instance.resourceMaxHelth = maxHealth;
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
                    if (animator != null)
                    {
                        animator.SetTrigger("Die"); // "Die"��Animator���Őݒ肵��Trigger��
                    }
                    else
                    {
                        Debug.LogWarning("Animator��������܂���");
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
    /// <param name="nowlevel">���݂̃��x��</param>
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

        // 1.6�{�̃X�P�[�����v�Z
        Vector3 newScale = currentScale * 1.5f;

        // �X�P�[�����X�V
        gameObject.transform.localScale = newScale;
    }
}
