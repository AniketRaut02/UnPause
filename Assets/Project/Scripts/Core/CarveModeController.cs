using DarkPixelGD.Input;
using DarkPixelGD.Narrator;
using DarkPixelGD.Tutorial;
using System;
using UnityEngine;

public class CarveModeController : MonoBehaviour
{
    public static CarveModeController Instance { get; private set; }
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GameObject _overlayPanel;

    public bool IsCarveModeActive { get; private set; }
    public event Action<bool> OnCarveModeChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void OnEnable() => _inputReader.PauseEvent += ToggleCarveMode;
    private void OnDisable() => _inputReader.PauseEvent -= ToggleCarveMode;
    private void Start() => PauseService.Instance.OnPauseChanged += HandleGlobalPauseChanged;

    public void ToggleCarveMode()
    {
        if (PauseMenuController.Instance != null && PauseMenuController.Instance.IsMenuOpen) return;
        if (TutorialPromptManager.Instance != null && TutorialPromptManager.Instance.IsShowingPrompt) return;
        if (NarratorService.Instance != null && NarratorService.Instance.IsPlaying) return;
        SetCarveMode(!IsCarveModeActive);
    }

    private void SetCarveMode(bool active)
    {
        if (active == IsCarveModeActive) return;

        if (active)
        {
            if (!PauseService.Instance.RequestPause()) return; 
            IsCarveModeActive = true;
            _overlayPanel.SetActive(true);
        }
        else
        {
            IsCarveModeActive = false;
            _overlayPanel.SetActive(false);
            PauseService.Instance.ReleasePause();
        }
        OnCarveModeChanged?.Invoke(IsCarveModeActive);
    }


    private void HandleGlobalPauseChanged(bool isPaused)
    {
        if (!isPaused && IsCarveModeActive)
        {
            IsCarveModeActive = false;
            OnCarveModeChanged?.Invoke(false);
        }
    }
}