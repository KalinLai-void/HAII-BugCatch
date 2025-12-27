using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float timerCountdownMins = 3;

    [SerializeField] private AudioSource countdown30AudioSource;
    [SerializeField] private AudioSource countdown10AudioSource;

    private float time;
    private bool isPlaying30, isPlaying10;

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
        else if (time <= 31 && !isPlaying30)
        {
            countdown30AudioSource.Play();
            isPlaying30 = true;
        }
        else if (time <= 11 && !isPlaying10)
        {
            countdown10AudioSource.Play();
            isPlaying10 = true;
        }

        UpdateUI();
    }

    public void PauseCountdownSFX()
    {
        countdown10AudioSource.Pause();
        countdown30AudioSource.Pause();
    }
    public void PlayCountdownSFX()
    {
        if (isPlaying10) countdown10AudioSource.Play();
        if (isPlaying30) countdown30AudioSource.Play();
    }

    private void UpdateUI()
    {
        int min = (int)time / 60;
        int sec = (int)time % 60;
        text.text = $"{min.ToString("00")}:{sec.ToString("00")}";
    }
}
