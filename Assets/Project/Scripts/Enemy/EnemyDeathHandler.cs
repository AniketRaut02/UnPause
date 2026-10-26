using DarkPixelGD.Audio;
using DarkPixelGD.Juice;
using DarkPixelGD.Player;
using DarkPixelGD.Systems.Combat;
using DarkPixelGD.Systems.Key;
using UnityEngine;

namespace DarkPixelGD.Enemy
{
    public class EnemyDeathHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Health _health;
        [SerializeField] private EnemyController _controller;
        [SerializeField] private EnemyShooter _shooting;
        [SerializeField] private Rigidbody2D _rb;

        [Header("Item Drop On Death")]
        [SerializeField] private bool _dropsItem = false;
        [SerializeField] private GameObject _itemPrefab; // Drag your Key or Health pickup here

        [Header("Audio")]
        [SerializeField] private AudioEventChannelSO _audioChannel;
        [SerializeField] private SoundDataSO _deathSoundSO;

        [Header("Juice References")]
        [SerializeField] private JuiceEventChannelSO _juiceChannel;
        [SerializeField] private JuiceProfileSO _deathJuiceProfile;

        private void OnEnable() => _health.OnDeath += HandleDeath;
        private void OnDisable() => _health.OnDeath -= HandleDeath;

        private void Awake()
        {
            if (_health == null) _health = GetComponent<Health>();
            if (_controller == null) _controller = GetComponent<EnemyController>();
            if (_shooting == null) _shooting = GetComponent<EnemyShooter>();
            if (_rb == null) _rb = GetComponent<Rigidbody2D>();
        }

        private void HandleDeath()
        {
            if (_rb != null) _rb.linearVelocity = Vector2.zero;
            if (_controller != null) _controller.enabled = false;
            if (_shooting != null) _shooting.enabled = false;

            if (_audioChannel != null && _deathSoundSO != null)
                _audioChannel.RaisePlaySound(_deathSoundSO, transform.position);

            if (_juiceChannel != null && _deathJuiceProfile != null)
                _juiceChannel.RaiseJuice(_deathJuiceProfile, transform.position);

            if (_dropsItem && _itemPrefab != null)
            {
                GameObject pickup = Instantiate(_itemPrefab, transform.position, Quaternion.identity);
                
            }
                

            Destroy(gameObject, 1f);
        }
    }
}