using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    //[SerializeField] private Canvas canvas;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;
    public bool isStack = false;


    private void Awake()
    {

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

    }


    public void OnBeginDrag(PointerEventData eventData)
    {



        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
        startPosition = transform.position;
        startParent = transform.parent;
        transform.SetParent(transform.root);
        itemBeingDragged = gameObject;

    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta /*/canvas.scaaleFactor*/;

    }



    public void OnEndDrag(PointerEventData eventData)
    {
        var termpItemReference = itemBeingDragged;
        itemBeingDragged = null;

        if (termpItemReference.transform.parent == termpItemReference.transform.root)
        {
            // アイテムのドロップ処理
            if (termpItemReference.name == "Mana(Clone)" || termpItemReference.name == "Stone(Clone)" || termpItemReference.name == "Stick(Clone)" || termpItemReference.name == "Log(Clone)")
            {
                termpItemReference.SetActive(false);
                AlertDialogManager dialogManager = FindObjectOfType<AlertDialogManager>();
                dialogManager.ShowDialog("ドロップしますか？", (responce) =>
                {
                    if (responce)
                    {
                        DropItemIntoTheWorld(termpItemReference);

                    }
                    else
                    {
                        CancelDragging(termpItemReference);
                    }
                });
            }
            else
            {
                CancelDragging(termpItemReference);
            }
        }

        if (termpItemReference.transform.parent == startParent)
        {
            CancelDragging(termpItemReference);
        }

        if (termpItemReference.transform.parent != termpItemReference.transform.root
            && termpItemReference.transform.parent != startParent)
        {
            if (termpItemReference.transform.parent.childCount > 2)
            {
                CancelDragging(termpItemReference);
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    isStack = true;
                    DivdeStack(termpItemReference);
                }
                else
                {
                    CraftingSystem.Instance.RefreshNeededItems();
                }
                isStack = false;
            }
        }

        termpItemReference.transform.SetAsFirstSibling();

        // リストの再計算とアイテムの更新
        InventorySystem.Instance.ReCalculeList();
        CraftingSystem.Instance.RefreshNeededItems();

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    // アイテムのスタック分割
    private void DivdeStack(GameObject termpItemReference)
    {
        InventoryItem item = termpItemReference.GetComponent<InventoryItem>();

        if (item.amountInventry > 1)
        {
            //TODO:スタックを一つだけ分割しているが変更する？
            item.amountInventry--;  // 元のスタックの数を減らす
            InventorySystem.Instance.AddToinventry(item.thisName, false);  // 新しいスタックを追加
            InventorySystem.Instance.ReCalculeList();  // スタック変更後にリストを再計算
            CraftingSystem.Instance.RefreshNeededItems();  // 必要なアイテムを更新

            InventorySystem.Instance.SetStackState(true);
        }
    }


    void CancelDragging(GameObject termpItemReference)
    {
        transform.position = startPosition;
        transform.SetParent(startParent);

        termpItemReference.SetActive(true);
    }


    private void DropItemIntoTheWorld(GameObject termpItemReference)
    {
        string cleanName = termpItemReference.name.Split(new string[] { "(Clone)" }, StringSplitOptions.None)[0];
        Debug.Log(cleanName);

        int stackCount = termpItemReference.GetComponent<InventoryItem>().amountInventry;

        for (int i = 0; i < stackCount; i++)
        {
            GameObject item = Instantiate(Resources.Load<GameObject>(cleanName + "_model"));
            item.transform.position = Vector3.zero;

            var dropSpawnPosition = PlayerState.Instance.playerBody.transform.Find("DropSpawn").transform.position;
            item.transform.localPosition = new Vector3(dropSpawnPosition.x, dropSpawnPosition.y, dropSpawnPosition.z);

            var itemsObject = FindObjectOfType<EnviromentManager>().gameObject.transform.Find("[ITEMS]");
            item.transform.SetParent(itemsObject.transform);
        }

        DestroyImmediate(termpItemReference.gameObject);
        InventorySystem.Instance.ReCalculeList();
        CraftingSystem.Instance.RefreshNeededItems();
    }
}