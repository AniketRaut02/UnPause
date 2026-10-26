using DarkPixelGD.Audio;
using UnityEngine;

public class FinalOrbEffects : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioEventChannelSO _audioChannel;
    [SerializeField] private SoundDataSO _soulRegainedSound;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            if (_audioChannel != null && _soulRegainedSound != null) { _audioChannel.RaisePlaySound(_soulRegainedSound, transform.position); }
        }
    }
}
