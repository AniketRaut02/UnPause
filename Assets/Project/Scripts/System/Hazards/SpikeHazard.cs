using UnityEngine;

namespace DarkPixelGD.Systems.Hazard
{
    public class SpikeHazard : HazardBase
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            ApplyHazardDamage(other.gameObject);
        }
    }
}