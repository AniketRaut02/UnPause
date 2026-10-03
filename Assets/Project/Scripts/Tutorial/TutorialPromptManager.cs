using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkPixelGD.Tutorial
{
    public class TutorialPromptManager : MonoBehaviour
    {
        public static TutorialPromptManager Instance { get; private set; }
        [SerializeField] private TutorialEventChannelSO _channel;

        public event Action<TutorialPromptDataSO> OnPromptShown;
        public event Action OnPromptClosed;
        public bool IsShowingPrompt { get; private set; }

        private readonly Queue<(TutorialPromptDataSO data, bool blocking)> _queue = new();
        private bool _currentIsBlocking;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Start() => _channel.OnShowRequested += HandleShowRequested;
        private void OnDisable() => _channel.OnShowRequested -= HandleShowRequested;

        private void HandleShowRequested(TutorialPromptDataSO data, bool blocking)
        {
            _queue.Enqueue((data, blocking));
            if (!IsShowingPrompt) ShowNext();
        }

        private void ShowNext()
        {
            if (_queue.Count == 0) { IsShowingPrompt = false; return; }
            var (data, blocking) = _queue.Dequeue();
            _currentIsBlocking = blocking;
            IsShowingPrompt = true;
            OnPromptShown?.Invoke(data);
            if (blocking) PauseService.Instance.RequestPause();
        }

        // Hooked to the Close button via TutorialPromptUI
        public void CloseCurrent()
        {
            if (_currentIsBlocking) PauseService.Instance.ReleasePause();
            OnPromptClosed?.Invoke();
            ShowNext();
        }
    }
}