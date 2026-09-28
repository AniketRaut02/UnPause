using DarkPixelGD.Input;
using System;
using UnityEngine;

public class PauseService : MonoBehaviour
{
    public static PauseService Instance { get; private set; }
    public bool IsPaused { get; private set; }
    public event Action<bool> OnPauseChanged;

    [SerializeField] private InputReader inputReader;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void OnEnable() => inputReader.PauseEvent += TogglePause;
    private void OnDisable() => inputReader.PauseEvent -= TogglePause;

    public void TogglePause() => SetPaused(!IsPaused);

    public void SetPaused(bool paused)
    {
        IsPaused = paused;
        Time.timeScale = paused ? 0f : 1f;
        OnPauseChanged?.Invoke(paused);
    }
}