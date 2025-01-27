using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
    public static Log Instance { get; private set; }

    public GameObject logTextPrefab;  // ���O���b�Z�[�W�̃v���n�u
    public Transform logContainer;   // ���O�̐e�I�u�W�F�N�g
    public GameObject logPanel;      // ���O�p�̃p�l��
    public float displayDuration = 5f; // ���O�\������
    public float fadeDuration = 2f;   // �t�F�[�h�A�E�g����

    private Dictionary<string, bool> logStatus = new Dictionary<string, bool>(); // ���O�̃t�F�[�h�A�E�g���

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

        logPanel.SetActive(false); // ������Ԃł̓p�l�����\��

    }

    public void OnFarmAttack(string farmName, Color color)
    {
        string colorHex = ColorUtility.ToHtmlStringRGB(color);
        string coloredFarmName = $"<color=#{colorHex}>{farmName}</color>";
        AddLogMessage(coloredFarmName + " ���U������܂����B");
    }

    public void OnFarmDeath(string farmName, Color color)
    {
        string colorHex = ColorUtility.ToHtmlStringRGB(color);
        string coloredFarmName = $"<color=#{colorHex}>{farmName}</color>";
        AddLogMessage(coloredFarmName + " ���j�󂳂�܂����B");
    }

    public void OnCreateEnemy(string farmName, Color color)
    {
        string colorHex = ColorUtility.ToHtmlStringRGB(color);
        string coloredFarmName = $"<color=#{colorHex}>{farmName}</color>";
        AddLogMessage(coloredFarmName + " �����������Ă���B �P�����ꂻ����");
    }

    public void TriggerPickupPop(string itemName)
    {
        AddLogMessage($"���S���܂���: {itemName}");
    }

    private void AddLogMessage(string message)
    {
        // �t�F�[�h�A�E�g���܂��͕\�����̃��b�Z�[�W�̓X�L�b�v
        if (logStatus.ContainsKey(message) && !logStatus[message])
        {
            return;
        }

        // ���b�Z�[�W��Ԃ��u�\�����v�ɐݒ�
        logStatus[message] = false;

        // �v���n�u����V�������O�I�u�W�F�N�g�𐶐�
        GameObject newLogObject = Instantiate(logTextPrefab, logContainer);
        Text newLogText = newLogObject.GetComponent<Text>();

        // ���b�Z�[�W��ݒ�
        newLogText.text = message;

        // �p�l����\��
        logPanel.SetActive(true);

		// ���b�Z�[�W���ʂɏ���
		StartCoroutine(HandleLogLifetime(newLogObject, newLogText, message));
    }

    private IEnumerator HandleLogLifetime(GameObject logObject, Text logText, string message)
    {
        // �\�����Ԃ�ҋ@
        yield return new WaitForSeconds(displayDuration);

        // �t�F�[�h�A�E�g����
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

        // ���O���폜
        Destroy(logObject);
		logPanel.SetActive(false);

		// ���b�Z�[�W�̏�Ԃ��u�ĕ\���\�v�ɐݒ�
		logStatus[message] = true;

		// ���O���S�ď�������p�l�����\���ɂ���
		//childCount�̏����l��1�̂��߁A �P�ȉ��Ƃ���
		//if (logContainer.childCount <= 1)
  //      {
		//	logPanel.SetActive(false);
  //      }
    }

    /// <summary>
    /// ���O�̐F��ύX����֐�
    /// </summary>
    public Color ColorChange(GameObject obj)
    {
        if (obj.name.Contains("��"))
            return Color.blue;
        if (obj.name.Contains("��"))
            return Color.red;
        if (obj.name.Contains("��"))
            return Color.yellow;
        if (obj.name.Contains("��"))
            return Color.green;

        return Color.clear;
    }
}
