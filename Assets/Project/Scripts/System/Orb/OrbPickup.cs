using DarkPixelGD.Audio;
using DarkPixelGD.Juice;
using DarkPixelGD.Player;
using UnityEngine;

namespace DarkPixelGD.Systems.Orb
{
    public enum OrbEffect { UnlockDash, RestoreControls }

    public class OrbPickup : MonoBehaviour
    {
        [SerializeField] private OrbEffect _effect = OrbEffect.UnlockDash;

        [Header("Audio")]
        [SerializeField] private AudioEventChannelSO _audioChannel;
        [SerializeField] private SoundDataSO _orbPickupSoundSO;
        [Header("Juice Refrences")]
        [SerializeField] private JuiceEventChannelSO _juiceChannel;
        [SerializeField] private JuiceProfileSO _orbPickupJuiceProfile;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            if (_audioChannel != null && _orbPickupSoundSO != null) { _audioChannel.RaisePlaySound(_orbPickupSoundSO, transform.position); }
            if (_juiceChannel != null && _orbPickupJuiceProfile != null) _juiceChannel.RaiseJuice(_orbPickupJuiceProfile, transform.position);

            if (_effect == OrbEffect.UnlockDash)
            {
                if (other.TryGetComponent(out PlayerController controller))
                {
                    controller.SetDashLocked(false);
                    HintPopupUI.Instance.Show("Dash ability restored!");
                }
                    
                
            }
            else
            {
                if (other.TryGetComponent(out PlayerController controller))
                {
                    controller.SetControlsInverted(false);
                    HintPopupUI.Instance.Show("Dash ability restored!");
                }

            }

            gameObject.SetActive(false); // placeholder pickup; swap for VFX/pool later
        }
    }
}