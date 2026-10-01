using System;
using UnityEngine;

namespace DarkPixelGD.Audio
{
    [CreateAssetMenu(fileName = "AudioEventChannel", menuName = "Sound/AudioEventChannelSO")]
    public class AudioEventChannelSO : ScriptableObject
    {
        public event Action<SoundDataSO, Vector3> OnPlayRequested;
        public event Action<SoundDataSO, Vector3> OnStopRequested; // for stoppable loops, e.g. laser hum

        public void RaisePlaySound(SoundDataSO sound, Vector3 position)
        {
            if (sound == null) return;
            OnPlayRequested?.Invoke(sound, position);
        }

        public void RaiseStopSound(SoundDataSO sound, Vector3 position) => OnStopRequested?.Invoke(sound, position);
    }
}