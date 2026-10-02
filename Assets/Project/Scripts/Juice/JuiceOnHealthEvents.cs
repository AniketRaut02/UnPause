using DarkPixelGD.Combat;
using DarkPixelGD.Systems.Combat;
using UnityEngine;

namespace DarkPixelGD.Juice
{
    public class JuiceOnHealthEvents : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private JuiceEventChannelSO _channel;
        [SerializeField] private JuiceProfileSO _damageProfile;
        [SerializeField] private JuiceProfileSO _deathProfile;

        private void Awake() { if (_health == null) _health = GetComponent<Health>(); }
        private void OnEnable() { _health.OnDamageTaken += HandleDamage; _health.OnDeath += HandleDeath; }
        private void OnDisable() { _health.OnDamageTaken -= HandleDamage; _health.OnDeath -= HandleDeath; }

        private void HandleDamage(int amount) => _channel.RaiseJuice(_damageProfile, transform.position);
        private void HandleDeath() => _channel.RaiseJuice(_deathProfile, transform.position);
    }
}