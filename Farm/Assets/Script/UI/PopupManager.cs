using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//担当者　越浦晃生

/// <summary>
/// ポップアップマネージャークラス。
/// </summary>
/// 
[RequireComponent(typeof(Animator))]
public class PopupManager : MonoBehaviour
{
    /// <summary>
    /// ピックアップのアラートゲームオブジェクト。
    /// </summary>
    public GameObject pickupAlert;

    /// <summary>
    /// ピックアップ名を表示するテキスト。
    /// </summary>
    public Text pickupName = null;

    /// <summary>
    /// ピックアップの画像を表示するイメージ。
    /// </summary>
    public Image pickupImage = null;

    /// <summary>
    /// ポップアップ待ちのキュー。
    /// </summary>
    private Queue<PickupRequest> pickupQueue = new Queue<PickupRequest>();

    /// <summary>
    /// 処理中かどうかのフラグ。
    /// </summary>
    private bool isProcessing = false;

    /// <summary>
    /// アニメーターコンポーネント。
    /// </summary>
    public Animator Animator;

    /// <summary>
    /// ピックアップリクエストを表すクラス。
    /// </summary>
    private class PickupRequest
    {
        /// <summary>
        /// アイテム名。
        /// </summary>
        public string itemName;

        /// <summary>
        /// アイテムのスプライト。
        /// </summary>
        public Sprite itemSprite;

        /// <summary>
        /// PickupRequestクラスのコンストラクタ。
        /// </summary>
        /// <param name="itemName">アイテム名。</param>
        /// <param name="itemSprite">アイテムのスプライト。</param>
        public PickupRequest(string itemName, Sprite itemSprite)
        {
            this.itemName = itemName;
            this.itemSprite = itemSprite;
        }
    }

    /// <summary>
    /// PopupManagerのインスタンス。
    /// </summary>
    public static PopupManager Instance { get; private set; }

    /// <summary>
    /// 初期設定を行います。
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
    /// 初期設定を行います。
    /// </summary>
    private void Start()
    {
        Animator = pickupAlert.GetComponent<Animator>();
    }

    /// <summary>
    /// ピックアップのポップアップリクエストを追加します。
    /// </summary>
    /// <param name="itemName">アイテム名。</param>
    /// <param name="itemSprite">アイテムのスプライト。</param>
    public void TriggerPickupPop(string itemName, Sprite itemSprite)
    {
        pickupQueue.Enqueue(new PickupRequest(itemName, itemSprite));

        if (!isProcessing)
        {
            StartCoroutine(ProcessPopupQueue());
        }
    }

    /// <summary>
    /// キューを順番に処理するコルーチン。
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
    /// 実際にポップアップを表示する処理。
    /// </summary>
    /// <param name="itemName">アイテム名。</param>
    /// <param name="itemSprite">アイテムのスプライト。</param>
    private IEnumerator ShowPickupPopup(string itemName, Sprite itemSprite)
    {
        pickupAlert.SetActive(true);

        Animator.SetTrigger("pop");

        string translatedName = InventorySystem.Instance.GetItemName(itemName);
        pickupName.text = translatedName;
        pickupImage.sprite = itemSprite;

        yield return new WaitForSeconds(3.0f);  // 3秒待機

        pickupAlert.SetActive(false);
    }
}
