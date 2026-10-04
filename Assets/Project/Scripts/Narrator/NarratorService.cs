using DarkPixelGD.Audio;
using DarkPixelGD.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkPixelGD.Narrator
{
    public class NarratorService : MonoBehaviour
    {
        public static NarratorService Instance { get; private set; }

        [SerializeField] private InputReader _inputReader; // used only to allow skipping a line early
        [SerializeField] private AudioSource _audioSource;

        [Header("Line Pop Sound")]
        [SerializeField] private AudioEventChannelSO _audioChannel; 
        [SerializeField] private SoundDataSO _linePopSound;

        public event Action<DialogueDataSO> OnDialogueStarted;
        public event Action<DialogueLine, int> OnLineChanged;
        public event Action OnDialogueEnded;

        public bool IsPlaying { get; private set; }
        private readonly Queue<DialogueDataSO> _queue = new();

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        public void Play(DialogueDataSO data)
        {
            _queue.Enqueue(data);
            if (!IsPlaying) StartCoroutine(ProcessQueue());
        }

        private IEnumerator ProcessQueue()
        {
            IsPlaying = true;
            PauseService.Instance.RequestPause();

            while (_queue.Count > 0)
                yield return PlayDialogue(_queue.Dequeue());
            PauseService.Instance.ReleasePause();

            IsPlaying = false;
        }

        private IEnumerator PlayDialogue(DialogueDataSO data)
        {
            OnDialogueStarted?.Invoke(data);

            for (int i = 0; i < data.lines.Length; i++)
            {
                DialogueLine line = data.lines[i];
                OnLineChanged?.Invoke(line, i);

                _audioChannel?.RaisePlaySound(_linePopSound, transform.position);

                if (line.voiceClip != null && _audioSource != null)
                    _audioSource.PlayOneShot(line.voiceClip);

                bool skip = false;
                void OnSkip() => skip = true;
                if (_inputReader != null) _inputReader.InteractEvent += OnSkip;

                float elapsed = 0f;
                while (elapsed < line.minDisplayDuration && !skip)
                {
                    elapsed += Time.deltaTime; // scaled: auto-freezes if the player pauses mid-line, same as your timer
                    yield return null;
                }

                if (_inputReader != null) _inputReader.InteractEvent -= OnSkip;
            }

            OnDialogueEnded?.Invoke();
        }
    }
}