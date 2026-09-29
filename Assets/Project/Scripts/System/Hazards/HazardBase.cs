using DarkPixelGD.Player;
using DarkPixelGD.Systems.Combat;
using System;
using UnityEngine;

namespace DarkPixelGD.Systems.Hazard
{
    public abstract class HazardBase : MonoBehaviour
    {
        [SerializeField] protected bool _ignoresPause = false;

        public event Action<GameObject> OnEntityKilled;

        protected void ApplyHazardDamage(GameObject target)
        {
            if (!target.TryGetComponent(out Health health)) return;
            if (health.Kill())
                OnEntityKilled?.Invoke(target);
        }
    }
}