using UnityEngine;

namespace DarkPixelGD.Enemy
{
    [RequireComponent(typeof(Collider2D))]
    public class BossActivationTrigger : MonoBehaviour
    {
        [Header("Reference to Boss")]
        [SerializeField] private BossController _bossController;
        [SerializeField] private bool _triggerOnce = true;

        private bool _hasTriggered;

        private void Awake()
        {
            if (_bossController == null)
            {
                Debug.LogWarning($"[{nameof(BossActivationTrigger)}] BossController not assigned on {gameObject.name}!", this);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (_triggerOnce && _hasTriggered) return;

            _hasTriggered = true;

            if (_bossController != null)
            {
                _bossController.ActivateBoss();
            }

            // Optional: Close doors or trigger arena lockdown here if desired

            if (_triggerOnce)
            {
                gameObject.SetActive(false); // Disable trigger after activation
            }
        }
    }
}