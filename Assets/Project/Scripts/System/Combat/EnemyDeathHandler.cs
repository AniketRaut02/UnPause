using UnityEngine;

namespace DarkPixelGD.Enemy
{
    public class EnemyDeathHandler : MonoBehaviour
    {
        [SerializeField] private Systems.Combat.Health _health;

        private void OnEnable() => _health.OnDeath += HandleDeath;
        private void OnDisable() => _health.OnDeath -= HandleDeath;

        private void HandleDeath() => gameObject.SetActive(false); // swap for VFX/pooling later
    }
}