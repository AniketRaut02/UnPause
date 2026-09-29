using UnityEngine;

namespace DarkPixelGD.Systems.Combat
{
    public class HealthPickup : MonoBehaviour
    {
        [SerializeField] private int _healAmount = 20;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (other.TryGetComponent(out Health health))
            {
                health.Heal(_healAmount);
                Destroy(gameObject);
            }
        }
    }
}