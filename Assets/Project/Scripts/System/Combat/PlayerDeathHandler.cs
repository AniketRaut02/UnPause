using DarkPixelGD.Player;
using UnityEngine;

namespace DarkPixelGD.Systems.Combat
{
    public class PlayerDeathHandler : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private Player.PlayerController _controller;
        [SerializeField] private Player.PlayerShooting _shooting; 
        [SerializeField] private Rigidbody2D _rb;

        private void OnEnable() => _health.OnDeath += HandleDeath;
        private void OnDisable() => _health.OnDeath -= HandleDeath;

        private void Awake()
        {
            _health = GetComponent<Health>();
            _controller = GetComponent<PlayerController>();
            _shooting = GetComponent<PlayerShooting>();
            _rb = GetComponent<Rigidbody2D>();
        }
        private void HandleDeath()
        {
            if (_rb != null) _rb.linearVelocity = Vector2.zero;
            if (_controller != null) _controller.enabled = false;
            if (_shooting != null) _shooting.enabled = false;

            TimerService.Instance?.ForceExpire();
        }
    }
}