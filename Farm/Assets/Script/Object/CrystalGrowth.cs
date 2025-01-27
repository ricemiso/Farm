using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

//担当者　佐藤　越浦晃生


/// <summary>
/// クリスタルの成長を管理するクラス。
/// </summary>
public class CrystalGrowth : MonoBehaviour
{
    /// <summary>
    /// 最大エネルギー。
    /// </summary>
    public float maxEnergy;

    /// <summary>
    /// 現在のエネルギー。
    /// </summary>
    public float currentEnergy;

    /// <summary>
    /// 回転角度。
    /// </summary>
    public Vector3 rotAngle;

    /// <summary>
    /// プレイヤーが範囲内にいるかどうかを示すフラグ。
    /// </summary>
    public bool playerRange;

    /// <summary>
    /// クリスタルの最大ヘルス。
    /// </summary>
    public float CrystalMaxHealth;

    /// <summary>
    /// クリスタルが監視可能かどうかを示すフラグ。
    /// </summary>
    public bool canBeWatch;

    /// <summary>
    /// クリスタルの現在のヘルス。
    /// </summary>
    public float CrystalHealth;

    /// <summary>
    /// クリスタルがチャージ可能かどうかを示すフラグ。
    /// </summary>
    public bool canBeCharge;

    /// <summary>
    /// チャージに消費するカロリー。
    /// </summary>
    public float caloriesSpendCarge;

    /// <summary>
    /// ロード画面のキャンバス。
    /// </summary>
    public Canvas loadScreen;

    /// <summary>
    /// 判定距離。
    /// </summary>
    [SerializeField] float dis = 10f;

    /// <summary>
    /// 破壊されたクリスタルのゲームオブジェクト。
    /// </summary>
    [SerializeField] GameObject breakCrystal;

    /// <summary>
    /// パーティクルシステム1。
    /// </summary>
    [SerializeField] ParticleSystem clearparth1;

    /// <summary>
    /// パーティクルシステム2。
    /// </summary>
    [SerializeField] ParticleSystem clearparth2;

    /// <summary>
    /// パーティクルシステム3。
    /// </summary>
    [SerializeField] ParticleSystem clearparth3;

    /// <summary>
    /// フェールキャンバス。
    /// </summary>
    [SerializeField] Canvas falseCanvas;

    /// <summary>
    /// ゲームクリアキャンバス。
    /// </summary>
    [SerializeField] Canvas gameClearCanvas;

    /// <summary>
    /// ゲームオーバーキャンバス。
    /// </summary>
    [SerializeField] Canvas gameOverCanvas;

    [SerializeField] GameObject CreateEnemyPlace;

	//WaveSysytemの参照
	public GameObject waveSystem;

	/// <summary>
	/// 初期設定を行います。
	/// </summary>
	void Start()
    {
        if(waveSystem != null)
            waveSystem.active = true;

		currentEnergy = 0;
        CrystalHealth = CrystalMaxHealth;
        rotAngle = Vector3.zero;
        canBeWatch = false;

        clearparth1.gameObject.SetActive(false);
        clearparth2.gameObject.SetActive(false);
        clearparth2.gameObject.SetActive(false);
        gameClearCanvas.gameObject.SetActive(false);
        falseCanvas.gameObject.SetActive(true);

        gameOverCanvas.sortingOrder = -2;
        gameClearCanvas.sortingOrder = -1;
    }

    /// <summary>
    /// 毎フレームの更新処理を行います。
    /// </summary>
    void Update()
    {
        if (PlayerState.Instance.currentHealth <= 0) return;

        //TODO:止める処理を入れる

        if (canBeWatch)
        {
            GrobalState.Instance.resourceHelth = CrystalHealth;
            GrobalState.Instance.resourceMaxHelth = CrystalMaxHealth;
        }

        canBeWatch = false;

        if (MenuManager.Instance.isCrystalMove)
        {
            rotAngle.y += 0.3f;
            transform.eulerAngles = rotAngle;
        }

        if (PlayerState.Instance.playerBody != null)
        {
            float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

            if (distance < dis)
            {
                playerRange = true;
            }
            else
            {
                playerRange = false;
            }
        }

        if (GrobalState.Instance.isTutorialEnd || GrobalState.Instance.isSkip)
        {
            // TODO:中央クリスタルにマナが溜まり切ったらゲームクリア
            if (PlayerState.Instance.currentHydrationPercent >= 100)
            {
				if (waveSystem != null)
					waveSystem.active = false;

				clearparth1.gameObject.SetActive(true);
                clearparth2.gameObject.SetActive(true);
                clearparth3.gameObject.SetActive(true);
                falseCanvas.gameObject.SetActive(false);

                if (clearparth1.isPlaying == false)
                {
                    clearparth1.Play();
                    clearparth2.Play();
                    clearparth3.Play();
                }

                PlayerState.Instance.playerBody.SetActive(false);
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                UnityEngine.Cursor.visible = true;
                gameOverCanvas.sortingOrder = -1;
                gameClearCanvas.sortingOrder = 2;
                gameClearCanvas.gameObject.SetActive(true);
				SoundManager.Instance.StopBGMSound();
				SoundManager.Instance.StopWalkSound();
                SoundManager.Instance.PlaySound(SoundManager.Instance.gameClearBGM);
                Transform enemyparent = CreateEnemyPlace.transform;
                // 親オブジェクトのすべての子オブジェクトを削除
                foreach (Transform child in enemyparent)
                {
                    Destroy(child.gameObject);
                }
            }
        }
        else
        {
            // TODO:中央クリスタルにマナが溜まり切ったらゲームクリア
            if (PlayerState.Instance.currentHydrationPercent >= 5)
            {
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                //Destroy(SoundManager.Instance.gameObject);
                // クリアシーン
                //TODO:チュートリアル終了変数
                GrobalState.Instance.isTutorialEnd = true;
                Destroy(gameObject.transform.parent.parent.gameObject);
                //SceneManager.LoadScene("MainScene");
                MainMenuSaveManager.Instance.StartLoadedGame("MainScene");
            }
        }
    }

    /// <summary>
    /// クリスタルにエネルギーを追加します。
    /// </summary>
    /// <param name="getEnergy">追加するエネルギー量</param>
    public void GetEnergy(float getEnergy)
    {
        PlayerState.Instance.currentHydrationPercent += getEnergy;
    }

    /// <summary>
    /// クリスタルがヒットされた時の処理を行います。
    /// </summary>
    /// <param name="damage">受けるダメージ量</param>
    public void GetHit(float damage)
    {

        Log.Instance.OnFarmAttack(gameObject.name,Color.black);

        SoundManager.Instance.PlaySound(SoundManager.Instance.CrystalAttack);

        CrystalHealth -= damage;
        GrobalState.Instance.resourceHelth = CrystalHealth;
        GrobalState.Instance.resourceMaxHelth = CrystalMaxHealth;

        PlayerState.Instance.currentCalories -= caloriesSpendCarge;

        if (CrystalHealth <= 0)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.Crystalbreak);
			SoundManager.Instance.StopBGMSound();
			SoundManager.Instance.StopWalkSound();
            SoundManager.Instance.PlaySound(SoundManager.Instance.gameOverBGM);
            CrystalIsDead();
        }
    }

    /// <summary>
    /// クリスタルが破壊された時の処理を行います。
    /// </summary>
    public void CrystalIsDead()
    {
        //Destroy(SoundManager.Instance.gameObject);
        UnityEngine.Cursor.lockState = CursorLockMode.None;

        // ゲームオーバーシーンに移動
        Vector3 newPosition = gameObject.transform.position; // 現在の位置を取得
        newPosition.y += 4; // y座標をオフセット（上方向に移動）
        GameObject Crystal = Instantiate(breakCrystal, newPosition, gameObject.transform.rotation);
        gameOverCanvas.sortingOrder = 2;
        gameClearCanvas.sortingOrder = -1;
        gameOverCanvas.gameObject.SetActive(true);

        Destroy(gameObject);
    }
}
