using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �v���C���[�̏�Ԃ��Ǘ�����N���X�B
/// </summary>
public class PlayerState : MonoBehaviour
{
    /// <summary>
    /// �v���C���[�̏�Ԃ̃C���X�^���X�B
    /// </summary>
    public static PlayerState Instance { get; set; }

    [SerializeField] Canvas falseCanvas;
    [SerializeField] Canvas gameOverCanvas;

    // Health
    /// <summary>
    /// ���݂̗̑́B
    /// </summary>
    public float currentHealth;

    /// <summary>
    /// �ő�̗́B
    /// </summary>
    public float maxHealth;

    // Calories
    /// <summary>
    /// ���݂̃J�����[�B
    /// </summary>
    public float currentCalories;

    /// <summary>
    /// �ő�J�����[�B
    /// </summary>
    public float maxCalories;

    float distanceTravelled;
    Vector3 lastPosition;

    /// <summary>
    /// �v���C���[�̑̂̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    public GameObject playerBody;

    // Hydration
    /// <summary>
    /// ���݂̐����p�[�Z���e�[�W�B
    /// </summary>
    public float currentHydrationPercent;

    /// <summary>
    /// �ő吅���p�[�Z���e�[�W�B
    /// </summary>
    public float maxHydrationPercent;

    /// <summary>
    /// �������A�N�e�B�u���ǂ����̃t���O�B
    /// </summary>
    public bool isHydrationActive;

    /// <summary>
    /// �v���C���[�̈ʒu�B
    /// </summary>
    public Vector3 isPosition;

    /// <summary>
    /// �v���C���[�̑��x�̊����B
    /// </summary>
    public float playerSpeedRate;

    /// <summary>
    /// ���̃p�l���̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    public GameObject bloodPannl;

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
    }

    /// <summary>
    /// �����ݒ���s���܂��B
    /// </summary>
    private void Start()
    {
        currentHealth = maxHealth;
        currentCalories = maxCalories;

        gameOverCanvas.gameObject.SetActive(false);
        gameOverCanvas.sortingOrder = -2;

        currentHydrationPercent = 0;

        // StartCoroutine(decreaseHydration());
    }

    /// <summary>
    /// ����������������R���[�`���B
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator decreaseHydration()
    {
        while (/*isHydrationActive*/true)
        {
            currentHydrationPercent -= 1;
            yield return new WaitForSeconds(10);
        }
    }

    /// <summary>
    /// ���t���[���̍X�V�������s���܂��B
    /// </summary>
    void Update()
    {
        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        if (distanceTravelled >= 50)
        {
            distanceTravelled = 0;
            currentCalories -= 1;
        }
    }

    /// <summary>
    /// �̗͂𑝌������鏈�����s���܂��B
    /// </summary>
    /// <param name="num">����������̗̗͂�</param>
    public void AddHealth(float num)
    {
        currentHealth += num;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth < 0)
        {
            currentHealth = 0;
            falseCanvas.gameObject.SetActive(false);
            gameOverCanvas.gameObject.SetActive(true);
            gameOverCanvas.sortingOrder = 1;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            playerBody.GetComponent<PlayerMovement>().enabled = false;
            playerBody.GetComponent<MouseMovement>().enabled = false;

            SoundManager.Instance.StopSound(SoundManager.Instance.startingZoneBGMMusic);
            SoundManager.Instance.StopWalkSound();
            SoundManager.Instance.PlaySound(SoundManager.Instance.gameOverBGM);
        }

        StartCoroutine(delayPanel());
    }

    /// <summary>
    /// �p�l���̒x���������s���R���[�`���B
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator delayPanel()
    {
        yield return new WaitForSeconds(0.5f);
        bloodPannl.SetActive(false);
    }

    /// <summary>
    /// �̗͂�ݒ肵�܂��B
    /// </summary>
    /// <param name="newHealth">�V�����̗�</param>
    public void setHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    /// <summary>
    /// �J�����[��ݒ肵�܂��B
    /// </summary>
    /// <param name="newCalories">�V�����J�����[</param>
    public void setCalories(float newCalories)
    {
        currentCalories = newCalories;
    }

    /// <summary>
    /// ������ݒ肵�܂��B
    /// </summary>
    /// <param name="newHydration">�V��������</param>
    public void setHydration(float newHydration)
    {
        currentHydrationPercent = newHydration;
    }

    /// <summary>
    /// �v���C���[�̈ʒu��ݒ肵�܂��B
    /// </summary>
    /// <param name="newPosition">�V�����ʒu</param>
    public void setPlayerPosition(Vector3 newPosition)
    {
        isPosition = newPosition;
    }

    /// <summary>
    /// �v���C���[�̈ʒu���擾���܂��B
    /// </summary>
    /// <returns>�v���C���[�̈ʒu</returns>
    public Vector3 getPlayerPosition()
    {
        return isPosition;
    }

    /// <summary>
    /// �v���C���[�̑��x�̊�����ݒ肵�܂��B
    /// </summary>
    /// <param name="newPlayerSpeed">�V�������x�̊���</param>
    public void setPlayerSpeedRate(float newPlayerSpeed)
    {
        playerSpeedRate = newPlayerSpeed;
    }

    /// <summary>
    /// �v���C���[�̑��x�̊������擾���܂��B
    /// </summary>
    /// <returns>�v���C���[�̑��x�̊���</returns>
    public float getPlayerSpeedRate()
    {
        return playerSpeedRate;
    }
}
