using DarkPixelGD.Audio;
using DarkPixelGD.Juice;
using DarkPixelGD.Player;
using UnityEngine;

namespace DarkPixelGD.Systems.Orb
{
    public enum OrbEffect { UnlockDash, RestoreControls, SoulRestore }

    public class OrbPickup : MonoBehaviour
    {
        [SerializeField] private OrbEffect _effect = OrbEffect.UnlockDash;

        [Header("Soul Settings")]
        [SerializeField] private float _soulRestoreAmount = 25f;

        [Header("Audio")]
        [SerializeField] private AudioEventChannelSO _audioChannel;
        [SerializeField] private SoundDataSO _orbPickupSoundSO;

        [Header("Juice References")]
        [SerializeField] private JuiceEventChannelSO _juiceChannel;
        [SerializeField] private JuiceProfileSO _orbPickupJuiceProfile;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            // Play audio & juice effects
            if (_audioChannel != null && _orbPickupSoundSO != null)
            {
                _audioChannel.RaisePlaySound(_orbPickupSoundSO, transform.position);
            }
            if (_juiceChannel != null && _orbPickupJuiceProfile != null)
            {
                _juiceChannel.RaiseJuice(_orbPickupJuiceProfile, transform.position);
            }

            // Handle specific orb effect
            if (_effect == OrbEffect.UnlockDash)
            {
                if (other.TryGetComponent(out PlayerController controller))
                {
                    controller.SetDashLocked(false);
                    HintPopupUI.Instance.Show("Dash ability restored!");
                }
            }
            else if (_effect == OrbEffect.SoulRestore)
            {
                if (other.TryGetComponent(out PlayerSoul playerSoul))
                {
                    playerSoul.RestoreSoul(_soulRestoreAmount);
                    HintPopupUI.Instance.Show($"Soul {playerSoul.CurrentSoul}% Restored");
                }
                else
                {
                    Debug.LogWarning("Player missing PlayerSoul component!");
                }
            }
            else if (_effect == OrbEffect.RestoreControls)
            {
                if (other.TryGetComponent(out PlayerController controller))
                {
                    controller.SetControlsInverted(false);
                    HintPopupUI.Instance.Show("Controls restored!");
                }
            }

            gameObject.SetActive(false); // Placeholder pickup; swap for VFX/pool later
        }
    }
}