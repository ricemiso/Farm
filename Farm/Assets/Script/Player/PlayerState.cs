using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//担当者　越浦晃生

/// <summary>
/// プレイヤーの状態を管理するクラス。
/// </summary>
public class PlayerState : MonoBehaviour
{
    /// <summary>
    /// プレイヤーの状態のインスタンス。
    /// </summary>
    public static PlayerState Instance { get; set; }

    [SerializeField] Canvas falseCanvas;
    [SerializeField] Canvas gameOverCanvas;

    // Health
    /// <summary>
    /// 現在の体力。
    /// </summary>
    public float currentHealth;

    /// <summary>
    /// 最大体力。
    /// </summary>
    public float maxHealth;

    // Calories
    /// <summary>
    /// 現在のカロリー。
    /// </summary>
    public float currentCalories;

    /// <summary>
    /// 最大カロリー。
    /// </summary>
    public float maxCalories;

    float distanceTravelled;
    Vector3 lastPosition;

    /// <summary>
    /// プレイヤーの体のゲームオブジェクト。
    /// </summary>
    public GameObject playerBody;

    // Hydration
    /// <summary>
    /// 現在の水分パーセンテージ。
    /// </summary>
    public float currentHydrationPercent;

    /// <summary>
    /// 最大水分パーセンテージ。
    /// </summary>
    public float maxHydrationPercent;

    /// <summary>
    /// 水分がアクティブかどうかのフラグ。
    /// </summary>
    public bool isHydrationActive;

    /// <summary>
    /// プレイヤーの位置。
    /// </summary>
    public Vector3 isPosition;

    /// <summary>
    /// プレイヤーの速度の割合。
    /// </summary>
    public float playerSpeedRate;

    /// <summary>
    /// 血のパネルのゲームオブジェクト。
    /// </summary>
    public GameObject bloodPannl;

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
    /// 初期設定を行います。
    /// </summary>
    private void Start()
    {
        currentHealth = maxHealth;
        currentCalories = maxCalories;

        gameOverCanvas.gameObject.SetActive(false);
        gameOverCanvas.sortingOrder = -2;

        currentHydrationPercent = 0;

        // StartCoroutine(decreaseHydration());
    }

    /// <summary>
    /// 水分を減少させるコルーチン。
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator decreaseHydration()
    {
        while (/*isHydrationActive*/true)
        {
            currentHydrationPercent -= 1;
            yield return new WaitForSeconds(10);
        }
    }

    /// <summary>
    /// 毎フレームの更新処理を行います。
    /// </summary>
    void Update()
    {
        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        if (distanceTravelled >= 50)
        {
            distanceTravelled = 0;
            currentCalories -= 1;
        }
    }

    /// <summary>
    /// 体力を増減させる処理を行います。
    /// </summary>
    /// <param name="num">増減させる体力の量</param>
    public void AddHealth(float num)
    {
        currentHealth += num;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth < 0)
        {
            currentHealth = 0;
            falseCanvas.gameObject.SetActive(false);
            gameOverCanvas.gameObject.SetActive(true);
            gameOverCanvas.sortingOrder = 1;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            playerBody.GetComponent<PlayerMovement>().enabled = false;
            playerBody.GetComponent<MouseMovement>().enabled = false;

            SoundManager.Instance.StopSound(SoundManager.Instance.startingZoneBGMMusic);
            SoundManager.Instance.StopWalkSound();
            SoundManager.Instance.PlaySound(SoundManager.Instance.gameOverBGM);
        }

        StartCoroutine(delayPanel());
    }

    /// <summary>
    /// パネルの遅延処理を行うコルーチン。
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator delayPanel()
    {
        yield return new WaitForSeconds(0.5f);
        bloodPannl.SetActive(false);
    }

    /// <summary>
    /// 体力を設定します。
    /// </summary>
    /// <param name="newHealth">新しい体力</param>
    public void setHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    /// <summary>
    /// カロリーを設定します。
    /// </summary>
    /// <param name="newCalories">新しいカロリー</param>
    public void setCalories(float newCalories)
    {
        currentCalories = newCalories;
    }

    /// <summary>
    /// 水分を設定します。
    /// </summary>
    /// <param name="newHydration">新しい水分</param>
    public void setHydration(float newHydration)
    {
        currentHydrationPercent = newHydration;
    }

    /// <summary>
    /// プレイヤーの位置を設定します。
    /// </summary>
    /// <param name="newPosition">新しい位置</param>
    public void setPlayerPosition(Vector3 newPosition)
    {
        isPosition = newPosition;
    }

    /// <summary>
    /// プレイヤーの位置を取得します。
    /// </summary>
    /// <returns>プレイヤーの位置</returns>
    public Vector3 getPlayerPosition()
    {
        return isPosition;
    }

    /// <summary>
    /// プレイヤーの速度の割合を設定します。
    /// </summary>
    /// <param name="newPlayerSpeed">新しい速度の割合</param>
    public void setPlayerSpeedRate(float newPlayerSpeed)
    {
        playerSpeedRate = newPlayerSpeed;
    }

    /// <summary>
    /// プレイヤーの速度の割合を取得します。
    /// </summary>
    /// <returns>プレイヤーの速度の割合</returns>
    public float getPlayerSpeedRate()
    {
        return playerSpeedRate;
    }
}
