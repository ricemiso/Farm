using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �|�b�v�A�b�v�}�l�[�W���[�N���X�B
/// </summary>
/// 
[RequireComponent(typeof(Animator))]
public class PopupManager : MonoBehaviour
{
    /// <summary>
    /// �s�b�N�A�b�v�̃A���[�g�Q�[���I�u�W�F�N�g�B
    /// </summary>
    public GameObject pickupAlert;

    /// <summary>
    /// �s�b�N�A�b�v����\������e�L�X�g�B
    /// </summary>
    public Text pickupName = null;

    /// <summary>
    /// �s�b�N�A�b�v�̉摜��\������C���[�W�B
    /// </summary>
    public Image pickupImage = null;

    /// <summary>
    /// �|�b�v�A�b�v�҂��̃L���[�B
    /// </summary>
    private Queue<PickupRequest> pickupQueue = new Queue<PickupRequest>();

    /// <summary>
    /// ���������ǂ����̃t���O�B
    /// </summary>
    private bool isProcessing = false;

    /// <summary>
    /// �A�j���[�^�[�R���|�[�l���g�B
    /// </summary>
    public Animator Animator;

    /// <summary>
    /// �s�b�N�A�b�v���N�G�X�g��\���N���X�B
    /// </summary>
    private class PickupRequest
    {
        /// <summary>
        /// �A�C�e�����B
        /// </summary>
        public string itemName;

        /// <summary>
        /// �A�C�e���̃X�v���C�g�B
        /// </summary>
        public Sprite itemSprite;

        /// <summary>
        /// PickupRequest�N���X�̃R���X�g���N�^�B
        /// </summary>
        /// <param name="itemName">�A�C�e�����B</param>
        /// <param name="itemSprite">�A�C�e���̃X�v���C�g�B</param>
        public PickupRequest(string itemName, Sprite itemSprite)
        {
            this.itemName = itemName;
            this.itemSprite = itemSprite;
        }
    }

    /// <summary>
    /// PopupManager�̃C���X�^���X�B
    /// </summary>
    public static PopupManager Instance { get; private set; }

    /// <summary>
    /// �����ݒ���s���܂��B
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
        Animator = pickupAlert.GetComponent<Animator>();
    }

    /// <summary>
    /// �s�b�N�A�b�v�̃|�b�v�A�b�v���N�G�X�g��ǉ����܂��B
    /// </summary>
    /// <param name="itemName">�A�C�e�����B</param>
    /// <param name="itemSprite">�A�C�e���̃X�v���C�g�B</param>
    public void TriggerPickupPop(string itemName, Sprite itemSprite)
    {
        pickupQueue.Enqueue(new PickupRequest(itemName, itemSprite));

        if (!isProcessing)
        {
            StartCoroutine(ProcessPopupQueue());
        }
    }

    /// <summary>
    /// �L���[�����Ԃɏ�������R���[�`���B
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator ProcessPopupQueue()
    {
        isProcessing = true;

        while (pickupQueue.Count > 0)
        {
            PickupRequest request = pickupQueue.Dequeue();
            yield return StartCoroutine(ShowPickupPopup(request.itemName, request.itemSprite));
        }

        isProcessing = false;
    }

    /// <summary>
    /// ���ۂɃ|�b�v�A�b�v��\�����鏈���B
    /// </summary>
    /// <param name="itemName">�A�C�e�����B</param>
    /// <param name="itemSprite">�A�C�e���̃X�v���C�g�B</param>
    private IEnumerator ShowPickupPopup(string itemName, Sprite itemSprite)
    {
        pickupAlert.SetActive(true);

        Animator.SetTrigger("pop");

        string translatedName = InventorySystem.Instance.GetItemName(itemName);
        pickupName.text = translatedName;
        pickupImage.sprite = itemSprite;

        yield return new WaitForSeconds(3.0f);  // 3�b�ҋ@

        pickupAlert.SetActive(false);
    }
}
