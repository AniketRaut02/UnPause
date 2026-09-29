using UnityEngine;

namespace DarkPixelGD.Systems.Combat
{
    public class PlayerDeathHandler : MonoBehaviour
    {
        [SerializeField] private Health _health;

        private void OnEnable() => _health.OnDeath += HandleDeath;
        private void OnDisable() => _health.OnDeath -= HandleDeath;

        private void HandleDeath()
        {
            TimerService.Instance?.ForceExpire();
        }
    }
}