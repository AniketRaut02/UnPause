using System;
using UnityEngine;

public class PauseService : MonoBehaviour
{
    public static PauseService Instance { get; private set; }
    public bool IsPaused { get; private set; }
    public event Action<bool> OnPauseChanged;

    private int _pauseRequests;
    private bool _isLocked;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public bool RequestPause()
    {
        if (_isLocked) return false; 
        _pauseRequests++;
        Refresh();
        return true;
    }
    public void ReleasePause() { _pauseRequests = Mathf.Max(0, _pauseRequests - 1); Refresh(); }

    // Used only by death/level-transition flows — guarantees a clean state regardless
    // of how many things thought they had a pause request open.
    public void ForceUnpause()
    {
        _pauseRequests = 0;
        Refresh();
    }

    private void Refresh()
    {
        bool shouldBePaused = _pauseRequests > 0;
        if (shouldBePaused == IsPaused) return;
        IsPaused = shouldBePaused;
        Time.timeScale = IsPaused ? 0f : 1f;
        OnPauseChanged?.Invoke(IsPaused);
    }

    public void LockUntilNextScene()
    {
        _isLocked = true;
        ForceUnpause();
    }

    public void UnlockForNewScene()
    {
        _isLocked = false;
        _pauseRequests = 0;
        Refresh();
    }

}