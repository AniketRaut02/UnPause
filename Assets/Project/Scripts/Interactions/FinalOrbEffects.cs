using DarkPixelGD.Audio;
using DarkPixelGD.Juice;
using UnityEngine;

public class FinalOrbEffects : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioEventChannelSO _audioChannel;
    [SerializeField] private SoundDataSO _soulRegainedSound;

    [Header("Juice Refrences")]
    [SerializeField] private JuiceEventChannelSO _juiceChannel;
    [SerializeField] private JuiceProfileSO _soulJuiceProfile;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_audioChannel != null && _soulRegainedSound != null) { _audioChannel.RaisePlaySound(_soulRegainedSound, transform.position); }
            if (_juiceChannel != null && _soulJuiceProfile != null) { _juiceChannel.RaiseJuice(_soulJuiceProfile, transform.position); }
        }
    }
}
