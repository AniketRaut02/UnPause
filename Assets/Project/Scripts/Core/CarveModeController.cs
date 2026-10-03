using DarkPixelGD.Input;
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
        SetCarveMode(!IsCarveModeActive);
    }

    private void SetCarveMode(bool active)
    {
        if (active == IsCarveModeActive) return;
        IsCarveModeActive = active;
        _overlayPanel.SetActive(active);
        if (active) PauseService.Instance.RequestPause();
        else PauseService.Instance.ReleasePause();
        OnCarveModeChanged?.Invoke(active);
    }

    // If a ForceUnpause happened (e.g. death mid-carve), resync our local flag
    // without double-releasing a request that's already gone.
    private void HandleGlobalPauseChanged(bool isPaused)
    {
        if (!isPaused && IsCarveModeActive)
        {
            IsCarveModeActive = false;
            OnCarveModeChanged?.Invoke(false);
        }
    }
}