using UnityEngine;
using DarkPixelGD.Player;
using DarkPixelGD.Audio;
using System;

namespace DarkPixelGD.Systems.Key
{
    [RequireComponent(typeof(Collider2D))]
    public class KeyPickup : MonoBehaviour
    {
        [SerializeField] private AudioEventChannelSO _audioChannel;
        [SerializeField] private SoundDataSO _pickupSound;
        [SerializeField]public bool isLastLevel;
        [SerializeField] private float shrunkDuration;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (other.TryGetComponent(out PlayerController controller))
                controller.SetHasKey(true);
            if (isLastLevel)
            {
                HintPopupUI.Instance?.Show("Picked up portal key.Timer shrunk down!!!");
                TimerService.Instance.StartTimer(shrunkDuration);
            }
            else
            {
                HintPopupUI.Instance?.Show("Picked up portal key.");
            }

            _audioChannel?.RaisePlaySound(_pickupSound, transform.position);
            gameObject.SetActive(false);
        }
    }
}