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

    // �s�b�N�A�b�v�|�b�v�A�b�v�̃L���[
    private Queue<string> pickupQueue = new Queue<string>();
    private bool isProcessing = false;

    // ���O���b�Z�[�W��ێ�����N���X
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

        // �p�l����������ԂŔ�\���ɂ���
        logPanel.SetActive(false);
    }

    // ���������񂾎��̃��\�b�h
    public void OnAllyDeath(string allyName)
    {
        AddLogMessage("���� " + allyName + " �����S���܂����B");
    }

    // �N���X�^�����U�����ꂽ���̃��\�b�h
    public void OnCrystalAttack()
    {
        AddLogMessage(" �N���X�^�����U������܂����B");
    }

    // �G���o���������̃��\�b�h
    public void OnEnemySpawn(string enemyName)
    {
        AddLogMessage("�G " + enemyName + " ���o�����܂����B");
    }

    // ���@�U�����ꂽ���̃��\�b�h
    public void OnFarmAttack(string FarmName)
    {
        AddLogMessage(FarmName + " ���U������܂����B");
    }

    // ���@���񂾎��̃��\�b�h
    public void OnFarmDeath(string FarmName)
    {
        AddLogMessage(FarmName + " ���j�󂳂�܂����B");
    }

    // ���O��ǉ����AText UI�ɕ\�����郁�\�b�h
    private void AddLogMessage(string message)
    {
        // �v���n�u����V�������O�I�u�W�F�N�g�𐶐�
        GameObject newLogObject = Instantiate(logTextPrefab, logContainer);
        Text newLogText = newLogObject.GetComponent<Text>();

        // �^�C���X�^���v���܂߂��Ƀ��O���b�Z�[�W��ݒ�
        newLogText.text = message;

        // �V�������O���b�Z�[�W�����X�g�ɒǉ�
        logMessages.Add(new LogMessage(newLogObject, newLogText, displayDuration));

        // �p�l����\��
        logPanel.SetActive(true);
    }

    private void Update()
    {
        // �e���O���b�Z�[�W�̃^�C�}�[���X�V���A�t�F�[�h�A�E�g�܂��͍폜����
        for (int i = logMessages.Count - 1; i >= 0; i--)
        {
            LogMessage log = logMessages[i];
            log.timer -= Time.deltaTime;

            if (log.timer <= fadeDuration)
            {
                // �t�F�[�h�A�E�g����
                float alpha = log.timer / fadeDuration;
                Color color = log.logText.color;
                color.a = Mathf.Clamp01(alpha);
                log.logText.color = color;
            }

            if (log.timer <= 0)
            {
                // �\�����Ԃ𒴂����烍�O���폜
                Destroy(log.logObject);
                logMessages.RemoveAt(i);
            }
        }

        // �p�l������ɂȂ������\���ɂ���
        if (logMessages.Count == 0)
        {
            logPanel.SetActive(false);
        }
    }

    // �s�b�N�A�b�v�|�b�v�A�b�v���g���K�[���郁�\�b�h
    public void TriggerPickupPop(string itemName)
    {
        pickupQueue.Enqueue(itemName);

        if (!isProcessing)
        {
            StartCoroutine(ProcessPopupQueue());
        }
    }

    // �L���[�����Ԃɏ�������R���[�`��
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

    // �s�b�N�A�b�v�|�b�v�A�b�v��\�����郁�\�b�h
    private IEnumerator ShowPickupPopup(string itemName)
    {
        // ���ۂ̃|�b�v�A�b�v�\���̎����������ɒǉ����Ă�������
        // �Ⴆ�΁AUI�ɃA�C�e������\������Ȃ�
        AddLogMessage($"���S���܂���: {itemName}");

        // �|�b�v�A�b�v���\������鎞�Ԃ𐧌䂷��
        yield return new WaitForSeconds(2f);
    }
}
