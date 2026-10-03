using DarkPixelGD.Audio;
using DarkPixelGD.Juice;
using DarkPixelGD.Player;
using System;
using UnityEngine;


namespace DarkPixelGD.Interaction
{ 

    [RequireComponent(typeof(Collider2D))]
    public class PortalTrigger : MonoBehaviour
    {
        public event Action OnLevelComplete;
        [Header("Key Requirement")]
        [SerializeField] private bool _requiresKey = false;
        [SerializeField] private PlayerController _player;


        [Header("Audio")]
        [SerializeField] private AudioEventChannelSO _audioChannel;
        [SerializeField] private SoundDataSO _portalSoundSO;
        [SerializeField] private SoundDataSO _portalActivatedSoundSO;
        [SerializeField] private SoundDataSO _lockedSoundSO;

        [Header("Juice Refrences")]
        [SerializeField] private JuiceEventChannelSO _juiceChannel;
        [SerializeField] private JuiceProfileSO _portalActivatedJuiceProfile;


        private void Start()
        {
            if (_audioChannel != null && _portalSoundSO != null) { _audioChannel.RaisePlaySound(_portalSoundSO, transform.position); }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            if (_requiresKey && (_player == null || !_player.HasKey))
            {
                HintPopupUI.Instance?.Show("You need a key.");
                _audioChannel?.RaisePlaySound(_lockedSoundSO, transform.position);
                return;
            }

            if (_audioChannel != null && _portalActivatedSoundSO != null) { _audioChannel.RaisePlaySound(_portalActivatedSoundSO, transform.position); }
            if (_juiceChannel != null && _portalActivatedJuiceProfile != null) _juiceChannel.RaiseJuice(_portalActivatedJuiceProfile, transform.position);
            OnLevelComplete?.Invoke();
        }
    }
}