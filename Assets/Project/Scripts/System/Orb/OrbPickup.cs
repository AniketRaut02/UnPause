using UnityEngine;
using DarkPixelGD.Player;

namespace DarkPixelGD.Systems.Orb
{
    public enum OrbEffect { UnlockDash, RestoreControls }

    public class OrbPickup : MonoBehaviour
    {
        [SerializeField] private OrbEffect _effect = OrbEffect.UnlockDash;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            if (_effect == OrbEffect.UnlockDash)
            {
                if (other.TryGetComponent(out PlayerController controller))
                    controller.SetDashLocked(false);
            }
            else
            {
                if (other.TryGetComponent(out PlayerController controller))
                    controller.SetControlsInverted(false);
            }

            gameObject.SetActive(false); // placeholder pickup; swap for VFX/pool later
        }
    }
}