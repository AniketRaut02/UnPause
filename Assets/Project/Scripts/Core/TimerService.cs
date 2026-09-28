using System;
using UnityEngine;

public class TimerService : MonoBehaviour
{
    public static TimerService Instance { get; private set; }
    public event Action OnTimerExpired;
    public event Action<float> OnTick;

    private float timeRemaining;
    private bool running;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void StartTimer(float duration)
    {
        timeRemaining = duration;
        running = true;
    }

    public void StopTimer() => running = false;

    private void Update()
    {
        if (!running) return;
        // Time.timeScale is 0 while paused, so Time.deltaTime naturally freezes here — no pause-check needed
        timeRemaining -= Time.deltaTime;
        OnTick?.Invoke(timeRemaining);
        if (timeRemaining <= 0f)
        {
            running = false;
            OnTimerExpired?.Invoke();
        }
    }

    public void ForceExpire()
    {
        if (!running) return;
        running = false;
        OnTimerExpired?.Invoke();
    }
}