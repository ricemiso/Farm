using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//担当者　越浦晃生

/// <summary>
/// インベントリスロットを管理するクラス。
/// </summary>
public class InventrySlot : MonoBehaviour
{
    /// <summary>
    /// アイテムの数量を表示するテキストコンポーネント。
    /// </summary>
    public Text amountTXT;

    /// <summary>
    /// スロット内のインベントリアイテム。
    /// </summary>
    public InventoryItem itemInSlot;

    /// <summary>
    /// クイックスロットかどうかを示すフラグ。
    /// </summary>
    public bool quickSlot;

    /// <summary>
    /// 非表示オブジェクトを設定するためのゲームオブジェクト。
    /// </summary>
    [SerializeField] private GameObject alphaobject;

    /// <summary>
    /// スタックの個数が0ならすかしたオブジェクトを出現させる
    /// スタック数があれば数字を出す
    /// </summary>
    private void Update()
    {
        InventoryItem item = CheckInventryItem();

        if (item != null)
        {
            itemInSlot = item;
            if (alphaobject != null)
            {
                alphaobject.SetActive(false);
            }
        }
        else
        {
            itemInSlot = null;
            if (alphaobject != null)
            {
                alphaobject.SetActive(true);
            }
        }

        if (itemInSlot != null && itemInSlot.amountInventry >= 2 && itemInSlot.amountInventry<= InventorySystem.Instance.stackLimit)
        {
            amountTXT.gameObject.SetActive(true);
            amountTXT.text = $"{itemInSlot.amountInventry}";
            amountTXT.transform.SetAsLastSibling();
        }
        else
        {
            amountTXT.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// スロット内のインベントリアイテムをチェックします。
    /// </summary>
    /// <returns>スロット内のインベントリアイテム</returns>
    public InventoryItem CheckInventryItem()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<InventoryItem>())
            {
                return child.GetComponent<InventoryItem>();
            }
        }
        return null;
    }

    /// <summary>
    /// スロット内のインベントリアイテムを設定する。
    /// </summary>
    public void SetItemInSlot()
    {
        itemInSlot = CheckInventryItem();
    }
}
