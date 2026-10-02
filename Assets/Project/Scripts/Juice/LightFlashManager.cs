using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal; // requires URP's 2D Renderer — flag if your project isn't on it

namespace DarkPixelGD.Juice
{
    public class LightFlashManager : MonoBehaviour
    {
        public static LightFlashManager Instance { get; private set; }
        [SerializeField] private GameObject _lightPrefab; // a GameObject with a Light2D (Point) component
        [SerializeField] private int _poolSize = 8;

        private readonly Queue<Light2D> _pool = new();

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;

            for (int i = 0; i < _poolSize; i++)
            {
                var light = Instantiate(_lightPrefab, transform).GetComponent<Light2D>();
                light.gameObject.SetActive(false);
                _pool.Enqueue(light);
            }
        }

        public void RequestFlash(JuiceProfileSO profile, Vector3 position)
        {

            Light2D light = _pool.Count > 0 ? _pool.Dequeue()
                : Instantiate(_lightPrefab, transform).GetComponent<Light2D>();

            light.transform.position = position;
            light.color = profile.lightColor;
            light.intensity = profile.lightIntensity;
            light.pointLightOuterRadius = profile.lightRadius;
            //light.lightCookieSprite = profile.lightSprite;
            light.gameObject.SetActive(true);

            StartCoroutine(DisableAfter(light, profile.lightDuration));
        }

        private IEnumerator DisableAfter(Light2D light, float duration)
        {
            yield return new WaitForSeconds(duration); // scaled: freezes correctly if triggered right at a pause
            light.gameObject.SetActive(false);
            _pool.Enqueue(light);
        }
    }
}