using UnityEngine;
using DarkPixelGD.Player;
using DarkPixelGD.Audio;

namespace DarkPixelGD.Systems.Key
{
    [RequireComponent(typeof(Collider2D))]
    public class KeyPickup : MonoBehaviour
    {
        [SerializeField] private AudioEventChannelSO _audioChannel;
        [SerializeField] private SoundDataSO _pickupSound;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (other.TryGetComponent(out PlayerController controller))
                controller.SetHasKey(true);
            HintPopupUI.Instance?.Show("Picked up portal key.");
            _audioChannel?.RaisePlaySound(_pickupSound, transform.position);
            gameObject.SetActive(false);
        }
    }
}