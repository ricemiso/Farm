using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//担当者　越浦晃生

/// <summary>
/// 拾えるオブジェクトを管理するクラス。
/// </summary>
public class InteractableObject : MonoBehaviour
{
    /// <summary>
    /// アイテムの名前。
    /// </summary>
    public string ItemName;

    /// <summary>
    /// インベントリに追加されるアイテムの名前。
    /// </summary>
    [SerializeField] string InventryName;

    /// <summary>
    /// プレイヤーが範囲内にいるかどうかを示すフラグ。
    /// </summary>
    public bool playerRange;

    /// <summary>
    /// 即座にピックアップするかどうかを示すフラグ。
    /// </summary>
    [SerializeField] bool FastPickup = false;

    /// <summary>
    /// 判定範囲の距離。
    /// </summary>
    [SerializeField] float ditectionRange = 10f;

    /// <summary>
    /// ゲームオブジェクトの名前に基づいてアイテム名を取得します。
    /// </summary>
    /// <param name="objectname">ゲームオブジェクト</param>
    /// <returns>アイテム名</returns>
    public string GetItemName(GameObject objectname)
    {
        switch (objectname.name)
        {
            case "Mana_model":
                ItemName = "マナ";
                break;
            case "Stone_model":
                ItemName = "石ころ";
                break;
            case "Log_model":
                ItemName = "丸太";
                break;
            case "Mana_model(Clone)":
                ItemName = "マナ";
                break;
            case "Stone_model(Clone)":
                ItemName = "石ころ";
                break;
            case "Log_model(Clone)":
                ItemName = "丸太";
                break;
        }

        return ItemName;
    }

    /// <summary>
    /// 毎フレームの更新処理を行います。
    /// </summary>
    private void Update()
    {
        // 距離をここで判定する
        if (PlayerState.Instance.playerBody != null && PlayerState.Instance.playerBody.gameObject != null)
        {
            float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

            if (distance < ditectionRange)
            {
                playerRange = true;
            }
            else
            {
                playerRange = false;
            }
        }

        if (playerRange)
        {
            // FastPickupがtrueか、選択したらクリックしたら
            if ((Input.GetKeyDown(KeyCode.Mouse0) && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectgameObject == gameObject)
                || FastPickup)
            {
                if (InventorySystem.Instance.CheckSlotAvailable(1))
                {
                    InventorySystem.Instance.AddToinventry(InventryName, true);
                    InventorySystem.Instance.itemsPickedup.Add(gameObject.name);

                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("inventry is full");
                }
            }
        }
    }
}
