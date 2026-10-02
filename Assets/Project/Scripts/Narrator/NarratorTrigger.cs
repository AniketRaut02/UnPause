using DarkPixelGD.Audio;
using UnityEngine;

namespace DarkPixelGD.Narrator
{
    [RequireComponent(typeof(Collider2D))]
    public class NarratorTrigger : MonoBehaviour
    {
        [SerializeField] private DialogueDataSO _dialogueData;
        [SerializeField] private bool _playOnce = true;

        [Header("Audio")]
        [SerializeField] private AudioEventChannelSO _audioChannel;
        [SerializeField] private SoundDataSO _narratorTriggerSoundSO;

        private bool _hasPlayed;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (_playOnce && _hasPlayed) return;
            _hasPlayed = true;
            if (_audioChannel != null && _narratorTriggerSoundSO != null) { _audioChannel.RaisePlaySound(_narratorTriggerSoundSO, transform.position); }
            NarratorService.Instance.Play(_dialogueData);
        }
    }
}