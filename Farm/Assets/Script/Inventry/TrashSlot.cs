using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//担当者　越浦晃生
//インベントリの廃止に伴い現在使用していない

/// <summary>
/// ゴミ箱スロットを管理するクラス。
/// </summary>
public class TrashSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// ゴミ箱アラートUI
    /// </summary>
    public GameObject trashAlertUI;

    /// <summary>
    /// テキストを変更するためのTextコンポーネント
    /// </summary>
    private Text textToModify;

    /// <summary>
    /// ゴミ箱が閉じているときのスプライト
    /// </summary>
    public Sprite trash_closed;

    /// <summary>
    /// ゴミ箱が開いているときのスプライト
    /// </summary>
    public Sprite trash_opened;

    /// <summary>
    /// イメージコンポーネント
    /// </summary>
    private Image imageComponent;

    /// <summary>
    /// "Yes"ボタン
    /// </summary>
    private Button YesBTN;

    /// <summary>
    /// "No"ボタン
    /// </summary>
    private Button NoBTN;

    /// <summary>
    /// ドラッグされたアイテムを取得するプロパティ
    /// </summary>
    private GameObject draggedItem
    {
        get
        {
            return DragDrop.itemBeingDragged;
        }
    }

    /// <summary>
    /// 削除されるアイテム
    /// </summary>
    private GameObject itemToBeDeleted;

    /// <summary>
    /// アイテム名を取得するプロパティ
    /// </summary>
    private string itemName
    {
        get
        {
            string name = itemToBeDeleted.name;
            string toRemove = "(Clone)";
            string result = name.Replace(toRemove, "");
            return result;
        }
    }

    /// <summary>
    /// 初期設定。
    /// </summary>
    private void Start()
    {
        imageComponent = transform.Find("background").GetComponent<Image>();
        textToModify = trashAlertUI.transform.Find("Text").GetComponent<Text>();
        YesBTN = trashAlertUI.transform.Find("yes").GetComponent<Button>();
        YesBTN.onClick.AddListener(delegate { DeleteItem(); });

        NoBTN = trashAlertUI.transform.Find("no").GetComponent<Button>();
        NoBTN.onClick.AddListener(delegate { CancelDeletion(); });
    }

    /// <summary>
    /// アイテムがドロップされた時の処理を行います。
    /// </summary>
    /// <param name="eventData">イベントデータ</param>
    public void OnDrop(PointerEventData eventData)
    {
        if (draggedItem.GetComponent<InventoryItem>().isTrashable == true)
        {
            itemToBeDeleted = draggedItem.gameObject;
            StartCoroutine(notifyBeforeDeletion());
        }
    }

    /// <summary>
    /// 削除前に通知を行うコルーチン。
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator notifyBeforeDeletion()
    {
        trashAlertUI.SetActive(true);
        textToModify.text = itemName + "を捨ててもよろしいですか?";
        yield return new WaitForSeconds(1f);
    }

    /// <summary>
    /// 削除をキャンセルします。
    /// </summary>
    private void CancelDeletion()
    {
        imageComponent.sprite = trash_closed;
        trashAlertUI.SetActive(false);
    }

    /// <summary>
    /// アイテムを削除します。
    /// </summary>
    private void DeleteItem()
    {
        imageComponent.sprite = trash_closed;
        DestroyImmediate(itemToBeDeleted.gameObject);
        InventorySystem.Instance.ReCalculeList();
        CraftingSystem.Instance.RefreshNeededItems();
        trashAlertUI.SetActive(false);
    }

    /// <summary>
    /// ポインタがゴミ箱スロットに入った時の処理を行います。
    /// </summary>
    /// <param name="eventData">イベントデータ</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (draggedItem != null && draggedItem.GetComponent<InventoryItem>().isTrashable == true)
        {
            imageComponent.sprite = trash_opened;
        }
    }

    /// <summary>
    /// ポインタがゴミ箱スロットから出た時の処理を行います。
    /// </summary>
    /// <param name="eventData">イベントデータ</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (draggedItem != null && draggedItem.GetComponent<InventoryItem>().isTrashable == true)
        {
            imageComponent.sprite = trash_closed;
        }
    }
}
