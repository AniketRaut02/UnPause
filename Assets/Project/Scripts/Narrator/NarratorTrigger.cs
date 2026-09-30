using UnityEngine;

namespace DarkPixelGD.Narrator
{
    [RequireComponent(typeof(Collider2D))]
    public class NarratorTrigger : MonoBehaviour
    {
        [SerializeField] private DialogueDataSO _dialogueData;
        [SerializeField] private bool _playOnce = true;

        private bool _hasPlayed;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (_playOnce && _hasPlayed) return;
            _hasPlayed = true;

            NarratorService.Instance.Play(_dialogueData);
        }
    }
}