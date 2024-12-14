using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]

public class PopupManager : MonoBehaviour
{
    public GameObject pickupAlert;
    public Text pickupName = null;
    public Image pickupImage = null;

    private Queue<PickupRequest> pickupQueue = new Queue<PickupRequest>();  // �|�b�v�A�b�v�҂��̃L���[
    private bool isProcessing = false;  // ���������ǂ���

    public Animator Animator;

    private class PickupRequest
    {
        public string itemName;
        public Sprite itemSprite;

        public PickupRequest(string itemName, Sprite itemSprite)
        {
            this.itemName = itemName;
            this.itemSprite = itemSprite;
        }
    }

    public static PopupManager Instance { get; private set; }

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

    private void Start()
    {
        Animator = pickupAlert.GetComponent<Animator>();
    }

    // �|�b�v�A�b�v�̃��N�G�X�g��ǉ�
    public void TriggerPickupPop(string itemName, Sprite itemSprite)
    {
       
        pickupQueue.Enqueue(new PickupRequest(itemName, itemSprite));

        if (!isProcessing)
        {
            StartCoroutine(ProcessPopupQueue());
        }
    }

    // �L���[�����Ԃɏ�������R���[�`��
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

    // ���ۂɃ|�b�v�A�b�v��\�����鏈��
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
