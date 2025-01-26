// 作成者：立石大翔

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

    private Queue<string> logQueue = new Queue<string>();
    private bool isDisplaying = false;

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

        logPanel.SetActive(false);
    }

    public void OnFarmAttack(string FarmName)
    {
        logQueue.Clear();
        AddLogMessageToQueue(FarmName + " が攻撃されました。");
    }

    public void OnFarmDeath(string FarmName)
    {
        logQueue.Clear();
        AddLogMessageToQueue(FarmName + " が破壊されました。");
    }

    public void OnCreateEnemy(string FarmName)
    {
        logQueue.Clear();
        AddLogMessageToQueue(FarmName + "も活性化している。襲撃されそうだ");
    }



    private void AddLogMessageToQueue(string message)
    {
        logQueue.Enqueue(message);
      
        if (!isDisplaying)
        {
            StartCoroutine(ProcessLogQueue());
        }
    }

    private IEnumerator ProcessLogQueue()
    {
        isDisplaying = true;

        while (logQueue.Count > 0)
        {
            string message = logQueue.Dequeue();

            // プレハブから新しいログオブジェクトを生成
            GameObject newLogObject = Instantiate(logTextPrefab, logContainer);
            Text newLogText = newLogObject.GetComponent<Text>();

            // ログメッセージを設定
            newLogText.text = message;

            // ログメッセージをリストに追加
            logMessages.Add(new LogMessage(newLogObject, newLogText, displayDuration));

            // パネルを表示
            logPanel.SetActive(true);

            // ログの表示時間を待機
            yield return new WaitForSeconds(displayDuration);

            // フェードアウト処理
            float fadeTime = fadeDuration;
            while (fadeTime > 0)
            {
                fadeTime -= Time.deltaTime;
                float alpha = Mathf.Clamp01(fadeTime / fadeDuration);
                Color color = newLogText.color;
                color.a = alpha;
                newLogText.color = color;
                yield return null;
            }

            // 表示時間が経過したログを削除
            if (logMessages.Count > 0)
            {
                LogMessage log = logMessages[0];
                Destroy(log.logObject);
                logMessages.RemoveAt(0);
            }

            // パネルが空になったら非表示にする
            if (logMessages.Count == 0)
            {
                logPanel.SetActive(false);
            }

            // 次のログ表示まで1秒待機
            yield return new WaitForSeconds(1f);
        }

        isDisplaying = false;
    }

    public void TriggerPickupPop(string itemName)
    {
        AddLogMessageToQueue($"死亡しました: {itemName}");
    }
}


