using System;
using UnityEngine;

namespace DarkPixelGD.Systems.Hazard
{
    public abstract class HazardBase : MonoBehaviour
    {
        [SerializeField] protected bool _ignoresPause = false; // unused until Day 5's L4 twist
        public event Action OnPlayerKilled;

        protected void KillPlayer(GameObject player)
        {
            OnPlayerKilled?.Invoke();
            TimerService.Instance?.ForceExpire(); // reuses the existing explode/restart flow
        }
    }
}