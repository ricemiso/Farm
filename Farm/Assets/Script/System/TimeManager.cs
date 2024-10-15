using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; set; }

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

    public int dayInGame = 1;

    public Text dayUI;

    private void Start()
    {
        dayUI.text = $"{dayInGame}“ú–Ú";
    }

    public void TriggerNextDay()
    {
        dayInGame += 1;
        dayUI.text = $"{dayInGame}“ú–Ú";
    }
}
