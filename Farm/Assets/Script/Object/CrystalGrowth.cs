using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

//�S���ҁ@�����@�z�Y�W��


/// <summary>
/// �N���X�^���̐������Ǘ�����N���X�B
/// </summary>
public class CrystalGrowth : MonoBehaviour
{
    /// <summary>
    /// �ő�G�l���M�[�B
    /// </summary>
    public float maxEnergy;

    /// <summary>
    /// ���݂̃G�l���M�[�B
    /// </summary>
    public float currentEnergy;

    /// <summary>
    /// ��]�p�x�B
    /// </summary>
    public Vector3 rotAngle;

    /// <summary>
    /// �v���C���[���͈͓��ɂ��邩�ǂ����������t���O�B
    /// </summary>
    public bool playerRange;

    /// <summary>
    /// �N���X�^���̍ő�w���X�B
    /// </summary>
    public float CrystalMaxHealth;

    /// <summary>
    /// �N���X�^�����Ď��\���ǂ����������t���O�B
    /// </summary>
    public bool canBeWatch;

    /// <summary>
    /// �N���X�^���̌��݂̃w���X�B
    /// </summary>
    public float CrystalHealth;

    /// <summary>
    /// �N���X�^�����`���[�W�\���ǂ����������t���O�B
    /// </summary>
    public bool canBeCharge;

    /// <summary>
    /// �`���[�W�ɏ����J�����[�B
    /// </summary>
    public float caloriesSpendCarge;

    /// <summary>
    /// ���[�h��ʂ̃L�����o�X�B
    /// </summary>
    public Canvas loadScreen;

    /// <summary>
    /// ���苗���B
    /// </summary>
    [SerializeField] float dis = 10f;

    /// <summary>
    /// �j�󂳂ꂽ�N���X�^���̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    [SerializeField] GameObject breakCrystal;

    /// <summary>
    /// �p�[�e�B�N���V�X�e��1�B
    /// </summary>
    [SerializeField] ParticleSystem clearparth1;

    /// <summary>
    /// �p�[�e�B�N���V�X�e��2�B
    /// </summary>
    [SerializeField] ParticleSystem clearparth2;

    /// <summary>
    /// �p�[�e�B�N���V�X�e��3�B
    /// </summary>
    [SerializeField] ParticleSystem clearparth3;

    /// <summary>
    /// �t�F�[���L�����o�X�B
    /// </summary>
    [SerializeField] Canvas falseCanvas;

    /// <summary>
    /// �Q�[���N���A�L�����o�X�B
    /// </summary>
    [SerializeField] Canvas gameClearCanvas;

    /// <summary>
    /// �Q�[���I�[�o�[�L�����o�X�B
    /// </summary>
    [SerializeField] Canvas gameOverCanvas;

    [SerializeField] GameObject CreateEnemyPlace;

	//WaveSysytem�̎Q��
	public GameObject waveSystem;

	/// <summary>
	/// �����ݒ���s���܂��B
	/// </summary>
	void Start()
    {
        if(waveSystem != null)
            waveSystem.active = true;

		currentEnergy = 0;
        CrystalHealth = CrystalMaxHealth;
        rotAngle = Vector3.zero;
        canBeWatch = false;

        clearparth1.gameObject.SetActive(false);
        clearparth2.gameObject.SetActive(false);
        clearparth2.gameObject.SetActive(false);
        gameClearCanvas.gameObject.SetActive(false);
        falseCanvas.gameObject.SetActive(true);

        gameOverCanvas.sortingOrder = -2;
        gameClearCanvas.sortingOrder = -1;
    }

    /// <summary>
    /// ���t���[���̍X�V�������s���܂��B
    /// </summary>
    void Update()
    {
        if (PlayerState.Instance.currentHealth <= 0) return;

        //TODO:�~�߂鏈��������

        if (canBeWatch)
        {
            GrobalState.Instance.resourceHelth = CrystalHealth;
            GrobalState.Instance.resourceMaxHelth = CrystalMaxHealth;
        }

        canBeWatch = false;

        if (MenuManager.Instance.isCrystalMove)
        {
            rotAngle.y += 0.3f;
            transform.eulerAngles = rotAngle;
        }

        if (PlayerState.Instance.playerBody != null)
        {
            float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

            if (distance < dis)
            {
                playerRange = true;
            }
            else
            {
                playerRange = false;
            }
        }

        if (GrobalState.Instance.isTutorialEnd || GrobalState.Instance.isSkip)
        {
            // TODO:�����N���X�^���Ƀ}�i�����܂�؂�����Q�[���N���A
            if (PlayerState.Instance.currentHydrationPercent >= 100)
            {
				if (waveSystem != null)
					waveSystem.active = false;

				clearparth1.gameObject.SetActive(true);
                clearparth2.gameObject.SetActive(true);
                clearparth3.gameObject.SetActive(true);
                falseCanvas.gameObject.SetActive(false);

                if (clearparth1.isPlaying == false)
                {
                    clearparth1.Play();
                    clearparth2.Play();
                    clearparth3.Play();
                }

                PlayerState.Instance.playerBody.SetActive(false);
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                UnityEngine.Cursor.visible = true;
                gameOverCanvas.sortingOrder = -1;
                gameClearCanvas.sortingOrder = 2;
                gameClearCanvas.gameObject.SetActive(true);
				SoundManager.Instance.StopBGMSound();
				SoundManager.Instance.StopWalkSound();
                SoundManager.Instance.PlaySound(SoundManager.Instance.gameClearBGM);
                Transform enemyparent = CreateEnemyPlace.transform;
                // �e�I�u�W�F�N�g�̂��ׂĂ̎q�I�u�W�F�N�g���폜
                foreach (Transform child in enemyparent)
                {
                    Destroy(child.gameObject);
                }
            }
        }
        else
        {
            // TODO:�����N���X�^���Ƀ}�i�����܂�؂�����Q�[���N���A
            if (PlayerState.Instance.currentHydrationPercent >= 5)
            {
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                //Destroy(SoundManager.Instance.gameObject);
                // �N���A�V�[��
                //TODO:�`���[�g���A���I���ϐ�
                GrobalState.Instance.isTutorialEnd = true;
                Destroy(gameObject.transform.parent.parent.gameObject);
                //SceneManager.LoadScene("MainScene");
                MainMenuSaveManager.Instance.StartLoadedGame("MainScene");
            }
        }
    }

    /// <summary>
    /// �N���X�^���ɃG�l���M�[��ǉ����܂��B
    /// </summary>
    /// <param name="getEnergy">�ǉ�����G�l���M�[��</param>
    public void GetEnergy(float getEnergy)
    {
        PlayerState.Instance.currentHydrationPercent += getEnergy;
    }

    /// <summary>
    /// �N���X�^�����q�b�g���ꂽ���̏������s���܂��B
    /// </summary>
    /// <param name="damage">�󂯂�_���[�W��</param>
    public void GetHit(float damage)
    {

        Log.Instance.OnFarmAttack(gameObject.name,Color.black);

        SoundManager.Instance.PlaySound(SoundManager.Instance.CrystalAttack);

        CrystalHealth -= damage;
        GrobalState.Instance.resourceHelth = CrystalHealth;
        GrobalState.Instance.resourceMaxHelth = CrystalMaxHealth;

        PlayerState.Instance.currentCalories -= caloriesSpendCarge;

        if (CrystalHealth <= 0)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.Crystalbreak);
			SoundManager.Instance.StopBGMSound();
			SoundManager.Instance.StopWalkSound();
            SoundManager.Instance.PlaySound(SoundManager.Instance.gameOverBGM);
            CrystalIsDead();
        }
    }

    /// <summary>
    /// �N���X�^�����j�󂳂ꂽ���̏������s���܂��B
    /// </summary>
    public void CrystalIsDead()
    {
        //Destroy(SoundManager.Instance.gameObject);
        UnityEngine.Cursor.lockState = CursorLockMode.None;

        // �Q�[���I�[�o�[�V�[���Ɉړ�
        Vector3 newPosition = gameObject.transform.position; // ���݂̈ʒu���擾
        newPosition.y += 4; // y���W���I�t�Z�b�g�i������Ɉړ��j
        GameObject Crystal = Instantiate(breakCrystal, newPosition, gameObject.transform.rotation);
        gameOverCanvas.sortingOrder = 2;
        gameClearCanvas.sortingOrder = -1;
        gameOverCanvas.gameObject.SetActive(true);

        Destroy(gameObject);
    }
}
