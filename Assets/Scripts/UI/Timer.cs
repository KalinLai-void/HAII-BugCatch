using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float timerCountdownMins = 3;

    private float time;

    public bool isFinished = false;

    private void Start()
    {
        time = timerCountdownMins * 60;
    }

    private void Update()
    {
        if (GameManager.instance.isPaused) return;
        if (isFinished) { UIManager.instance.TurnOnUI(UIinGame.EndUI); };

        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = 0;
            isFinished = true; 
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        int min = (int)time / 60;
        int sec = (int)time % 60;
        text.text = $"{min.ToString("00")}:{sec.ToString("00")}";
    }
}
