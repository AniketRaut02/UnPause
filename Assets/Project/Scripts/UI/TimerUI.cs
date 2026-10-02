using DarkPixelGD.Audio;
using DarkPixelGD.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DarkPixelGD.UI
{
    public class TimerUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _timeLabel;
        [SerializeField] private Image _icon;

        [Header("Low-Time Warning")]
        [SerializeField] private float _warningThreshold = 10f;
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _warningColor = Color.red;
        [SerializeField] private Animator _animator; // optional — for a pulse clip, see below

        [Header("Audio")]
        [SerializeField] private AudioEventChannelSO _audioChannel;
        [SerializeField] private SoundDataSO _timerTickSound;
        [SerializeField] private Transform playerTransform;

        private static readonly int _pulseHash = Animator.StringToHash("Pulse");
        private bool _warningActive;

        private void Start()
        {
            TimerService.Instance.OnTick += HandleTick;
        }

        private void OnDisable()
        {
            if (TimerService.Instance != null)
                TimerService.Instance.OnTick -= HandleTick;
        }

        private void HandleTick(float timeRemaining)
        {
            float clamped = Mathf.Max(0f, timeRemaining);
            int minutes = Mathf.FloorToInt(clamped / 60f);
            int seconds = Mathf.FloorToInt(clamped % 60f);
            _timeLabel.text = $"{minutes:00}:{seconds:00}";

            bool shouldWarn = clamped <= _warningThreshold;
            if (shouldWarn != _warningActive)
            {
                _warningActive = shouldWarn;
                Color target = shouldWarn ? _warningColor : _normalColor;
                _timeLabel.color = target;
                _icon.color = target;
                if (_audioChannel != null && _timerTickSound != null) { _audioChannel.RaisePlaySound(_timerTickSound, playerTransform.position); }
                if (shouldWarn && _animator != null)
                {
                    _animator.SetTrigger(_pulseHash); // one-shot punch/scale clip, same Animator-owns-motion pattern as your gun
                }
                  
            }
        }
    }
}