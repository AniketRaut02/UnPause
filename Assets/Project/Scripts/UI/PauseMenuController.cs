using DarkPixelGD.Narrator;
using DarkPixelGD.Tutorial;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public static PauseMenuController Instance { get; private set; }
    [SerializeField] private DarkPixelGD.Input.InputReader _inputReader;
    [SerializeField] private PauseMenuUI _menuUI;

    public bool IsMenuOpen { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void OnEnable() => _inputReader.OpenMenuEvent += ToggleMenu;
    private void OnDisable() => _inputReader.OpenMenuEvent -= ToggleMenu;

    public void ToggleMenu()
    {
        if (CarveModeController.Instance != null && CarveModeController.Instance.IsCarveModeActive) return;
        if (NarratorService.Instance != null && NarratorService.Instance.IsPlaying) return;
        if (TutorialPromptManager.Instance != null && TutorialPromptManager.Instance.IsShowingPrompt) return;
        SetMenuOpen(!IsMenuOpen);
    }

    public void SetMenuOpen(bool open)
    {
        if (open == IsMenuOpen) return;
        IsMenuOpen = open;
        if (open) PauseService.Instance.RequestPause();
        else PauseService.Instance.ReleasePause();
        _menuUI.SetVisible(open);
    }
}