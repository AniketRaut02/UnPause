using DarkPixelGD.Audio;
using DarkPixelGD.Juice;
using DarkPixelGD.Player;
using DarkPixelGD.Systems.Combat;
using UnityEngine;

namespace DarkPixelGD.Enemy
{
    public class EnemyDeathHandler : MonoBehaviour
    {
        [Header("Refrences")]
        [SerializeField] private Health _health;
        [SerializeField] private Enemy.EnemyController _controller;
        [SerializeField] private Enemy.EnemyShooter _shooting;
        [SerializeField] private Rigidbody2D _rb;

        [Header("Health Pickup Drop")]
        [SerializeField] private bool _dropsHealthPickup = false;
        [SerializeField] private GameObject _healthPickupPrefab;

        [Header("Audio")]
        [SerializeField] private AudioEventChannelSO _audioChannel;
        [SerializeField] private SoundDataSO _deathSoundSO;

        [Header("Juice Refrences")]
        [SerializeField] private JuiceEventChannelSO _juiceChannel;
        [SerializeField] private JuiceProfileSO _deathJuiceProfile;

        private void OnEnable() => _health.OnDeath += HandleDeath;
        private void OnDisable() => _health.OnDeath -= HandleDeath;

        private void Awake()
        {
            _health = GetComponent<Health>();
            _controller = GetComponent<EnemyController>();
            _shooting = GetComponent<EnemyShooter>();
            _rb = GetComponent<Rigidbody2D>();
        }
        private void HandleDeath()
        {
            if (_rb != null) _rb.linearVelocity = Vector2.zero;
            if (_controller != null) _controller.enabled = false;
            if (_shooting != null) _shooting.enabled = false;
            if (_audioChannel != null && _deathSoundSO != null) { _audioChannel.RaisePlaySound(_deathSoundSO, transform.position); }
            if (_juiceChannel != null && _deathJuiceProfile != null) _juiceChannel.RaiseJuice(_deathJuiceProfile,transform.position);

            if (_dropsHealthPickup && _healthPickupPrefab != null)
                Instantiate(_healthPickupPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject, 1f);
        }
    }
}