using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
    public static Log Instance { get; private set; }

    public GameObject logTextPrefab;  // ログメッセージのプレハブ
    public Transform logContainer;   // ログの親オブジェクト
    public GameObject logPanel;      // ログ用のパネル
    public float displayDuration = 5f; // ログ表示時間
    public float fadeDuration = 2f;   // フェードアウト時間

    private Dictionary<string, bool> logStatus = new Dictionary<string, bool>(); // ログのフェードアウト状態

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

        logPanel.SetActive(false); // 初期状態ではパネルを非表示

    }

    public void OnFarmAttack(string farmName, Color color)
    {
        string colorHex = ColorUtility.ToHtmlStringRGB(color);
        string coloredFarmName = $"<color=#{colorHex}>{farmName}</color>";
        AddLogMessage(coloredFarmName + " が攻撃されました。");
    }

    public void OnFarmDeath(string farmName, Color color)
    {
        string colorHex = ColorUtility.ToHtmlStringRGB(color);
        string coloredFarmName = $"<color=#{colorHex}>{farmName}</color>";
        AddLogMessage(coloredFarmName + " が破壊されました。");
    }

    public void OnCreateEnemy(string farmName, Color color)
    {
        string colorHex = ColorUtility.ToHtmlStringRGB(color);
        string coloredFarmName = $"<color=#{colorHex}>{farmName}</color>";
        AddLogMessage(coloredFarmName + " も活性化している。 襲撃されそうだ");
    }

    public void TriggerPickupPop(string itemName)
    {
        AddLogMessage($"死亡しました: {itemName}");
    }

    private void AddLogMessage(string message)
    {
        // フェードアウト中または表示中のメッセージはスキップ
        if (logStatus.ContainsKey(message) && !logStatus[message])
        {
            return;
        }

        // メッセージ状態を「表示中」に設定
        logStatus[message] = false;

        // プレハブから新しいログオブジェクトを生成
        GameObject newLogObject = Instantiate(logTextPrefab, logContainer);
        Text newLogText = newLogObject.GetComponent<Text>();

        // メッセージを設定
        newLogText.text = message;

        // パネルを表示
        logPanel.SetActive(true);

		// メッセージを個別に処理
		StartCoroutine(HandleLogLifetime(newLogObject, newLogText, message));
    }

    private IEnumerator HandleLogLifetime(GameObject logObject, Text logText, string message)
    {
        // 表示時間を待機
        yield return new WaitForSeconds(displayDuration);

        // フェードアウト処理
        float fadeTime = fadeDuration;
        while (fadeTime > 0)
        {
            fadeTime -= Time.deltaTime;
            float alpha = Mathf.Clamp01(fadeTime / fadeDuration);
            Color color = logText.color;
            color.a = alpha;
            logText.color = color;
            yield return null;
        }

        // ログを削除
        Destroy(logObject);
		logPanel.SetActive(false);

		// メッセージの状態を「再表示可能」に設定
		logStatus[message] = true;

		// ログが全て消えたらパネルを非表示にする
		//childCountの初期値が1のため、 １以下とする
		//if (logContainer.childCount <= 1)
  //      {
		//	logPanel.SetActive(false);
  //      }
    }

    /// <summary>
    /// ログの色を変更する関数
    /// </summary>
    public Color ColorChange(GameObject obj)
    {
        if (obj.name.Contains("青"))
            return Color.blue;
        if (obj.name.Contains("赤"))
            return Color.red;
        if (obj.name.Contains("黄"))
            return Color.yellow;
        if (obj.name.Contains("緑"))
            return Color.green;

        return Color.clear;
    }
}
