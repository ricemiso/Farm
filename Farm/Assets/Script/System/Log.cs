// �쐬�ҁF���Α���

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
    public static Log Instance { get; set; }

    // Text UI�����蓖�Ă邽�߂̕ϐ�
    public GameObject logTextPrefab;  // �e���O���b�Z�[�W�̃v���n�u
    public Transform logContainer;    // ���O��z�u����e�I�u�W�F�N�g
    public GameObject logPanel;       // ���O�\���p�̃p�l��
    public float displayDuration = 5f; // ���O���\������鎞��
    public float fadeDuration = 2f;   // �t�F�[�h�A�E�g�ɂ����鎞��

    private Queue<string> logQueue = new Queue<string>();
    private bool isDisplaying = false;

    private class LogMessage
    {
        public GameObject logObject;   // ���O�\���p��UI�I�u�W�F�N�g
        public Text logText;           // ���ۂ̃e�L�X�g
        public float timer;            // �c��\������

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
        AddLogMessageToQueue(FarmName + " ���U������܂����B");
    }

    public void OnFarmDeath(string FarmName)
    {
        logQueue.Clear();
        AddLogMessageToQueue(FarmName + " ���j�󂳂�܂����B");
    }

    public void OnCreateEnemy(string FarmName)
    {
        logQueue.Clear();
        AddLogMessageToQueue(FarmName + "�����������Ă���B�P�����ꂻ����");
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

            // �v���n�u����V�������O�I�u�W�F�N�g�𐶐�
            GameObject newLogObject = Instantiate(logTextPrefab, logContainer);
            Text newLogText = newLogObject.GetComponent<Text>();

            // ���O���b�Z�[�W��ݒ�
            newLogText.text = message;

            // ���O���b�Z�[�W�����X�g�ɒǉ�
            logMessages.Add(new LogMessage(newLogObject, newLogText, displayDuration));

            // �p�l����\��
            logPanel.SetActive(true);

            // ���O�̕\�����Ԃ�ҋ@
            yield return new WaitForSeconds(displayDuration);

            // �t�F�[�h�A�E�g����
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

            // �\�����Ԃ��o�߂������O���폜
            if (logMessages.Count > 0)
            {
                LogMessage log = logMessages[0];
                Destroy(log.logObject);
                logMessages.RemoveAt(0);
            }

            // �p�l������ɂȂ������\���ɂ���
            if (logMessages.Count == 0)
            {
                logPanel.SetActive(false);
            }

            // ���̃��O�\���܂�1�b�ҋ@
            yield return new WaitForSeconds(1f);
        }

        isDisplaying = false;
    }

    public void TriggerPickupPop(string itemName)
    {
        AddLogMessageToQueue($"���S���܂���: {itemName}");
    }
}


