using DarkPixelGD.Combat;
using DarkPixelGD.Systems.Combat;
using UnityEngine;

namespace DarkPixelGD.Enemy
{
    public class BossKilled : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private string _message = "Boss defeated!";

        private void Awake() { if (_health == null) _health = GetComponent<Health>(); }
        private void OnEnable() => _health.OnDeath += HandleDeath;
        private void OnDisable() => _health.OnDeath -= HandleDeath;

        private void HandleDeath() => HintPopupUI.Instance?.Show(_message);
    }
}