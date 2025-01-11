using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//担当者　越浦晃生

/// <summary>
/// ストレージを管理するクラス。
/// </summary>
public class StorageManager : MonoBehaviour
{
    /// <summary>
    /// StorageManagerのインスタンス。
    /// </summary>
    public static StorageManager Instance { get; set; }

    /// <summary>
    /// 小さな収納ボックスのUI。
    /// </summary>
    [SerializeField] GameObject storageBoxSmallUI;

    /// <summary>
    /// 選択された収納ボックス。
    /// </summary>
    [SerializeField] StrageBox selectedStorage;

    /// <summary>
    /// 収納UIが開かれているかどうかを示すフラグ。
    /// </summary>
    public bool storageUIOpen;

    /// <summary>
    /// シングルトンパターンを適用し、インスタンスを初期化します。
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
    /// 収納ボックスを開きます。
    /// </summary>
    /// <param name="storage">開く収納ボックス</param>
    public void OpenBox(StrageBox storage)
    {
        SetSelectedStorage(storage);
        PopulateStorage(GetRelevantUI(selectedStorage));

        GetRelevantUI(selectedStorage).SetActive(true);
        storageUIOpen = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SelectionManager.Instance.DisableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
    }

    /// <summary>
    /// 収納UIにアイテムを配置します。
    /// </summary>
    /// <param name="storageUI">収納UI</param>
    private void PopulateStorage(GameObject storageUI)
    {
        // UIの全てのスロットを取得
        List<GameObject> uiSlots = new List<GameObject>();

        foreach (Transform child in storageUI.transform)
        {
            uiSlots.Add(child.gameObject);
        }

        // プレハブをインスタンス化し、それぞれのGameObjectの子として設定
        foreach (string name in selectedStorage.items)
        {
            foreach (GameObject slot in uiSlots)
            {
                if (slot.transform.childCount < 1)
                {
                    var resource = Resources.Load<GameObject>(name);
                    if (resource == null)
                    {
                        Debug.Log($"Failed to load resource: {name}");
                        continue;  // 次のアイテムへ
                    }

                    var itemToAdd = Instantiate(resource, slot.transform.position, slot.transform.rotation);
                    itemToAdd.name = name;
                    itemToAdd.transform.SetParent(slot.transform);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 収納ボックスを閉じます。
    /// </summary>
    public void CloseBox()
    {
        RecalculateStorage(GetRelevantUI(selectedStorage));

        GetRelevantUI(selectedStorage).SetActive(false);
        storageUIOpen = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SelectionManager.Instance.EnableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
    }

    /// <summary>
    /// 収納データを再計算します。
    /// </summary>
    /// <param name="storageUI">収納UI</param>
    private void RecalculateStorage(GameObject storageUI)
    {
        List<GameObject> uiList = new List<GameObject>();

        foreach (Transform child in storageUI.transform)
        {
            uiList.Add(child.gameObject);
        }

        selectedStorage.items.Clear();
        List<GameObject> toBeDeleted = new List<GameObject>();

        foreach (GameObject slot in uiList)
        {
            if (slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name;
                string str2 = "(Clone)";
                string result = name.Replace(str2, "");

                selectedStorage.items.Add(result);
                toBeDeleted.Add(slot.transform.GetChild(0).gameObject);
            }
        }

        foreach (GameObject obj in toBeDeleted)
        {
            Destroy(obj);
        }
    }

    /// <summary>
    /// 選択された収納を設定します。
    /// </summary>
    /// <param name="storage">選択された収納</param>
    public void SetSelectedStorage(StrageBox storage)
    {
        selectedStorage = storage;
    }

    /// <summary>
    /// 関連するUIを取得します。
    /// </summary>
    /// <param name="storage">収納</param>
    /// <returns>関連するUIのGameObject</returns>
    private GameObject GetRelevantUI(StrageBox storage)
    {
        // 他のタイプのスイッチを作成
        return storageBoxSmallUI;
    }
}
