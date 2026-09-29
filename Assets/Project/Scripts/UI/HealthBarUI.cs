using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DarkPixelGD.Systems.Combat
{
    public class HealthBarUI : MonoBehaviour
    {
        [SerializeField] private Health _health;          // auto-fetched if left unassigned
        [SerializeField] private Image _fillImage;         
        [SerializeField] private GameObject _root;         
        [SerializeField] private float _animateDuration = 0.25f;
        [SerializeField] private bool _hideWhenFull = false;

        private Coroutine _animateRoutine;

        private void Awake()
        {
            if (_health == null) _health = GetComponentInParent<Health>();
        }

        private void OnEnable()
        {
            _health.OnHealthChanged += HandleHealthChanged;
            _health.OnDeath += HandleDeath;
        }

        private void OnDisable()
        {
            _health.OnHealthChanged -= HandleHealthChanged;
            _health.OnDeath -= HandleDeath;
        }

        private void Start()
        {
            // Snap instantly to the correct starting value, independent of event subscription order
            SetFillInstant((float)_health.CurrentHealth / _health.MaxHealth);
            UpdateVisibility(_health.CurrentHealth, _health.MaxHealth);
        }

        private void HandleHealthChanged(int current, int max)
        {
            UpdateVisibility(current, max);
            if (_animateRoutine != null) StopCoroutine(_animateRoutine);
            _animateRoutine = StartCoroutine(AnimateFill((float)current / max));
        }

        private void HandleDeath()
        {
            if (_root != null) _root.SetActive(false);
        }

        private void UpdateVisibility(int current, int max)
        {
            if (_root == null || !_hideWhenFull) return;
            _root.SetActive(current < max);
        }

        private IEnumerator AnimateFill(float targetFill)
        {
            float startFill = _fillImage.fillAmount;
            float elapsed = 0f;

            while (elapsed < _animateDuration)
            {
                elapsed += Time.deltaTime; // scaled: bar animation freezes automatically on pause
                _fillImage.fillAmount = Mathf.Lerp(startFill, targetFill, elapsed / _animateDuration);
                yield return null;
            }
            _fillImage.fillAmount = targetFill;
        }

        private void SetFillInstant(float value)
        {
            if (_fillImage != null) _fillImage.fillAmount = value;
        }
    }
}