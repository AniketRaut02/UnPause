using Unity.Cinemachine; // adjust if your installed version uses a different namespace
using UnityEngine;

namespace DarkPixelGD.Juice
{
    public class CameraJuiceManager : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private JuiceEventChannelSO _channel;
        [SerializeField] private CinemachineImpulseSource _impulseSource;
        [SerializeField] private CinemachineBasicMultiChannelPerlin _perlin; // the Noise component on your active CinemachineCamera

        [Header("Explosion Dread Ramp")]
        [SerializeField] private float _explosionWarningThreshold = 5f;
        [SerializeField] private float _maxContinuousAmplitude = 2f;

        private void OnEnable() => _channel.OnJuiceRequested += HandleJuiceRequested;
        private void OnDisable() => _channel.OnJuiceRequested -= HandleJuiceRequested;
        private void Start() => TimerService.Instance.OnTick += HandleTimerTick;
        private void OnDestroy()
        {
            if (TimerService.Instance != null)
                TimerService.Instance.OnTick -= HandleTimerTick;
        }

        private void HandleJuiceRequested(JuiceProfileSO profile, Vector3 position)
        {
            _impulseSource.GenerateImpulse(profile.impulseVelocity * profile.shakeForce);

            if (profile.useLightFlash && LightFlashManager.Instance != null)
                LightFlashManager.Instance.RequestFlash(profile, position);
        }

        // Builds dread as the timer approaches zero; automatically resets to calm whenever
        // the timer resets above threshold (e.g. a first-death-free respawn) — no extra code
        // needed for that case, since HandleTimerTick just sees a higher value next tick.
        private void HandleTimerTick(float timeRemaining)
        {
            if (_perlin == null) return;

            if (timeRemaining > _explosionWarningThreshold) { _perlin.AmplitudeGain = 0f; return; }

            float t = 1f - Mathf.Clamp01(timeRemaining / _explosionWarningThreshold);
            _perlin.AmplitudeGain = t * t * _maxContinuousAmplitude; // squared curve: subtle at first, punchy near zero
        }
    }
}