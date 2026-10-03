using DarkPixelGD.Audio;
using UnityEngine;

namespace DarkPixelGD.Tutorial
{
    [RequireComponent(typeof(Collider2D))]
    public class TutorialPromptTrigger : MonoBehaviour
    {
        [SerializeField] private TutorialEventChannelSO _channel;
        [SerializeField] private TutorialPromptDataSO _promptData;
        [SerializeField] private bool _playOnce = true;
        [SerializeField] private bool _blocking = true;

        [Header("Audio")]
        [SerializeField] private AudioEventChannelSO _audioChannel;
        [SerializeField] private SoundDataSO _tutorialTriggerSoundSO;
        private bool _hasPlayed;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (_playOnce && _hasPlayed) return;
            _hasPlayed = true;

            if (_audioChannel != null && _tutorialTriggerSoundSO != null) { _audioChannel.RaisePlaySound(_tutorialTriggerSoundSO, transform.position); }
            _channel.RaiseShow(_promptData, _blocking);
        }
    }
}