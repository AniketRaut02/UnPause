using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace DarkPixelGD.Systems.Lighting
{
    [RequireComponent(typeof(Light2D))]
    public class Light2DFlicker : MonoBehaviour
    {
        [Header("Falloff Flicker Settings")]
        [Tooltip("Minimum and maximum time interval between falloff changes.")]
        [SerializeField] private Vector2 _flickerFrequencyRange = new Vector2(0.05f, 0.2f);

        [Tooltip("Minimum and maximum Falloff Strength values during flicker.")]
        [SerializeField] private Vector2 _falloffStrengthRange = new Vector2(0.2f, 0.8f);

        private Light2D _light2D;
        private float _timer;
        private float _nextFlickerTime;

        private void Awake()
        {
            _light2D = GetComponent<Light2D>();
            ResetTimer();
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= _nextFlickerTime)
            {
                ApplyRandomFalloffFlicker();
                ResetTimer();
            }
        }

        private void ApplyRandomFalloffFlicker()
        {
            float randomFalloff = Random.Range(_falloffStrengthRange.x, _falloffStrengthRange.y);
            _light2D.falloffIntensity = randomFalloff;
        }

        private void ResetTimer()
        {
            _timer = 0f;
            _nextFlickerTime = Random.Range(_flickerFrequencyRange.x, _flickerFrequencyRange.y);
        }
    }
}