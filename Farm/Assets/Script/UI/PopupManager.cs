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

    private Queue<PickupRequest> pickupQueue = new Queue<PickupRequest>();  // ポップアップ待ちのキュー
    private bool isProcessing = false;  // 処理中かどうか

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

    // ポップアップのリクエストを追加
    public void TriggerPickupPop(string itemName, Sprite itemSprite)
    {
       
        pickupQueue.Enqueue(new PickupRequest(itemName, itemSprite));

        if (!isProcessing)
        {
            StartCoroutine(ProcessPopupQueue());
        }
    }

    // キューを順番に処理するコルーチン
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

    // 実際にポップアップを表示する処理
    private IEnumerator ShowPickupPopup(string itemName, Sprite itemSprite)
    {
       

        pickupAlert.SetActive(true);

        Animator.SetTrigger("pop");

        string translatedName = TranslateItemName(itemName);
        pickupName.text = translatedName;
        pickupImage.sprite = itemSprite;

        yield return new WaitForSeconds(3.0f);  // 3秒待機

        pickupAlert.SetActive(false);
    }

    // アイテム名を翻訳するヘルパーメソッド
    private string TranslateItemName(string itemName)
    {
        switch (itemName)
        {
            case "Axe":
                return "斧";
            case "Plank":
                return "木の板";
            case "Foundation":
                return "木の床";
            case "Wall":
                return "木の壁";
            case "Log":
                return "丸太";
            case "Stone":
                return "石";
            case "Stick":
                return "木の棒";
            default:
                return itemName;
        }
    }
}
