using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
    public static Log Instance { get; set; }

    // Text UIを割り当てるための変数
    public GameObject logTextPrefab;  // 各ログメッセージのプレハブ
    public Transform logContainer;    // ログを配置する親オブジェクト
    public GameObject logPanel;       // ログ表示用のパネル
    public float displayDuration = 5f; // ログが表示される時間
    public float fadeDuration = 2f;   // フェードアウトにかかる時間

    // ピックアップポップアップのキュー
    private Queue<string> pickupQueue = new Queue<string>();
    private bool isProcessing = false;

    // ログメッセージを保持するクラス
    private class LogMessage
    {
        public GameObject logObject;   // ログ表示用のUIオブジェクト
        public Text logText;           // 実際のテキスト
        public float timer;            // 残り表示時間

        public LogMessage(GameObject obj, Text text, float duration)
        {
            logObject = obj;
            logText = text;
            timer = duration;
        }
    }

    private List<LogMessage> logMessages = new List<LogMessage>();

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

        // パネルを初期状態で非表示にする
        logPanel.SetActive(false);
    }

    // 味方が死んだ時のメソッド
    public void OnAllyDeath(string allyName)
    {
        AddLogMessage("味方 " + allyName + " が死亡しました。");
    }

    // クリスタルが攻撃された時のメソッド
    public void OnCrystalAttack()
    {
        AddLogMessage(" クリスタルが攻撃されました。");
    }

    // 敵が出現した時のメソッド
    public void OnEnemySpawn(string enemyName)
    {
        AddLogMessage("敵 " + enemyName + " が出現しました。");
    }

    // 畑　攻撃された時のメソッド
    public void OnFarmAttack(string FarmName)
    {
        AddLogMessage(FarmName + " が攻撃されました。");
    }

    // 畑　死んだ時のメソッド
    public void OnFarmDeath(string FarmName)
    {
        AddLogMessage(FarmName + " が破壊されました。");
    }

    // ログを追加し、Text UIに表示するメソッド
    private void AddLogMessage(string message)
    {
        // プレハブから新しいログオブジェクトを生成
        GameObject newLogObject = Instantiate(logTextPrefab, logContainer);
        Text newLogText = newLogObject.GetComponent<Text>();

        // タイムスタンプを含めずにログメッセージを設定
        newLogText.text = message;

        // 新しいログメッセージをリストに追加
        logMessages.Add(new LogMessage(newLogObject, newLogText, displayDuration));

        // パネルを表示
        logPanel.SetActive(true);
    }

    private void Update()
    {
        // 各ログメッセージのタイマーを更新し、フェードアウトまたは削除する
        for (int i = logMessages.Count - 1; i >= 0; i--)
        {
            LogMessage log = logMessages[i];
            log.timer -= Time.deltaTime;

            if (log.timer <= fadeDuration)
            {
                // フェードアウト処理
                float alpha = log.timer / fadeDuration;
                Color color = log.logText.color;
                color.a = Mathf.Clamp01(alpha);
                log.logText.color = color;
            }

            if (log.timer <= 0)
            {
                // 表示時間を超えたらログを削除
                Destroy(log.logObject);
                logMessages.RemoveAt(i);
            }
        }

        // パネルが空になったら非表示にする
        if (logMessages.Count == 0)
        {
            logPanel.SetActive(false);
        }
    }

    // ピックアップポップアップをトリガーするメソッド
    public void TriggerPickupPop(string itemName)
    {
        pickupQueue.Enqueue(itemName);

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
            string itemName = pickupQueue.Dequeue();
            yield return StartCoroutine(ShowPickupPopup(itemName));
        }

        isProcessing = false;
    }

    // ピックアップポップアップを表示するメソッド
    private IEnumerator ShowPickupPopup(string itemName)
    {
        // 実際のポップアップ表示の実装をここに追加してください
        // 例えば、UIにアイテム名を表示するなど
        AddLogMessage($"死亡しました: {itemName}");

        // ポップアップが表示される時間を制御する
        yield return new WaitForSeconds(2f);
    }
}
