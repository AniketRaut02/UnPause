using DarkPixelGD.Audio;
using DarkPixelGD.Juice;
using UnityEngine;

namespace DarkPixelGD.Systems.Combat
{
    public class HealthPickup : MonoBehaviour
    {
        [SerializeField] private int _healAmount = 20;

        [Header("Audio")]
        [SerializeField] private AudioEventChannelSO _audioChannel;
        [SerializeField] private SoundDataSO _healthPickupSoundSO;
        [Header("Juice Refrences")]
        [SerializeField] private JuiceEventChannelSO _juiceChannel;
        [SerializeField] private JuiceProfileSO _healthPickupJuiceProfile;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (other.TryGetComponent(out Health health))
            {
                health.Heal(_healAmount);
                if (_audioChannel != null && _healthPickupSoundSO != null) { _audioChannel.RaisePlaySound(_healthPickupSoundSO, transform.position); }
                if (_juiceChannel != null && _healthPickupJuiceProfile != null) _juiceChannel.RaiseJuice(_healthPickupJuiceProfile, transform.position);
                Destroy(gameObject);
            }
        }
    }
}