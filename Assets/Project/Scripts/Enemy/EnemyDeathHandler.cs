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

            if (_dropsHealthPickup && _healthPickupPrefab != null)
                Instantiate(_healthPickupPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject, 1f);
        }
    }
}