using DarkPixelGD.Audio;
using DarkPixelGD.Juice;
using System;
using UnityEngine;


namespace DarkPixelGD.Interaction
{ 

    [RequireComponent(typeof(Collider2D))]
    public class PortalTrigger : MonoBehaviour
    {
        public event Action OnLevelComplete;

        [Header("Audio")]
        [SerializeField] private AudioEventChannelSO _audioChannel;
        [SerializeField] private SoundDataSO _portalSoundSO;
        [SerializeField] private SoundDataSO _portalActivatedSoundSO;

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
            //Debug.Log("Portal Activated");
            if (_audioChannel != null && _portalActivatedSoundSO != null) { _audioChannel.RaisePlaySound(_portalActivatedSoundSO, transform.position); }
            if (_juiceChannel != null && _portalActivatedJuiceProfile != null) _juiceChannel.RaiseJuice(_portalActivatedJuiceProfile, transform.position);
            OnLevelComplete?.Invoke();
        }
    }
}