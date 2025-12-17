using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float timerCountdownMins = 3;

    private float time;

    public bool isFinished;

    private void Start()
    {
        time = timerCountdownMins * 60;
    }

    private void Update()
    {
        time -= Time.deltaTime;
        UpdateUI();
    }

    private void UpdateUI()
    {
        int min = (int)time / 60;
        int sec = (int)time % 60;
        text.text = $"{min.ToString("00")}:{sec.ToString("00")}";
    }
}
