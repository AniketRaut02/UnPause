using DarkPixelGD.Audio;
using UnityEngine;

namespace DarkPixelGD.VFX
{
    [RequireComponent(typeof(Animator))]
    public class ExplosionEffect : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] private AudioEventChannelSO _audioChannel;
        [SerializeField] private SoundDataSO _orbPickupSoundSO;

        private void Start()
        {
            if(_audioChannel!=null && _orbPickupSoundSO!=null)
            {
                _audioChannel.RaisePlaySound(_orbPickupSoundSO, transform.position);
            }
        }
        public void DestroySelf() => Destroy(gameObject);
    }
}