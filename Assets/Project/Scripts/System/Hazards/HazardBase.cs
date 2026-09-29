using DarkPixelGD.Player;
using System;
using UnityEngine;

namespace DarkPixelGD.Systems.Hazard
{
    public abstract class HazardBase : MonoBehaviour
    {
        [SerializeField] protected bool _ignoresPause = false; // unused until Day 5's L4 twist
        public event Action OnPlayerKilled;

/*        protected void KillPlayer(GameObject player)
        {
            if (player.TryGetComponent(out PlayerController controller) && controller.IsInvulnerable)
                return;

            OnPlayerKilled?.Invoke();
            TimerService.Instance?.ForceExpire();
        }*/
        protected void ApplyHazardDamage(GameObject target)
        {
            if (!target.TryGetComponent(out Combat.Health health)) return;
            bool died = health.Kill();
            if (died && target.CompareTag("Player"))
            {
                OnPlayerKilled?.Invoke();
                TimerService.Instance?.ForceExpire();
            }

        }
    }
}