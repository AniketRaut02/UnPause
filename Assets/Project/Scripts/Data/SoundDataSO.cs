using UnityEngine;
using UnityEngine.Audio;

namespace DarkPixelGD.Audio
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "Sound/SoundDataSO")]
    public class SoundDataSO : ScriptableObject
    {
        [Header("Clips (one picked at random per play — single entry = no variation)")]
        public AudioClip[] clips;

        [Header("Output")]
        public AudioMixerGroup mixerGroup;

        [Header("Playback")]
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0f, 0.5f)] public float volumeVariance = 0.05f;
        public float pitchMin = 0.95f;
        public float pitchMax = 1.05f;
        [Range(0f, 1f)] public float spatialBlend = 1f; // 0 = UI/global, 1 = fully positional
        public bool loop = false;

        public AudioClip GetClip() => clips.Length == 0 ? null : clips[Random.Range(0, clips.Length)];
        public float GetVolume() => Mathf.Clamp01(volume + Random.Range(-volumeVariance, volumeVariance));
        public float GetPitch() => Random.Range(pitchMin, pitchMax);
    }
}