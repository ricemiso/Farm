using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//担当者　越浦晃生

/// <summary>
/// クリスタルが壊れた際の処理を管理するクラス。
/// </summary>
public class BreakCrystal : MonoBehaviour
{
    /// <summary>
    /// 初期設定を行います。
    /// </summary>
    void Start()
    {
        StartCoroutine(delaygameOver());
        PlayerState.Instance.playerBody.SetActive(false);
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
    }

    /// <summary>
    /// ゲームオーバーまでの遅延処理を行うコルーチン。
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator delaygameOver()
    {
        yield return new WaitForSeconds(10);
    }
}
