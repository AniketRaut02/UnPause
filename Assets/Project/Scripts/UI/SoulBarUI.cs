using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DarkPixelGD.Player;

namespace DarkPixelGD.UI
{
    public class SoulBarUI : MonoBehaviour
    {
        [SerializeField] private PlayerSoul _playerSoul;    // auto-fetched if left unassigned
        [SerializeField] private Image _fillImage;
        [SerializeField] private GameObject _root;
        [SerializeField] private float _animateDuration = 0.25f;
        [SerializeField] private bool _hideWhenFull = false;

        private Coroutine _animateRoutine;

        private void Awake()
        {
            if (_playerSoul == null)
            {
                // Try looking in parents first, then fallback to finding by tag if in world/canvas space
                _playerSoul = GetComponentInParent<PlayerSoul>();
                if (_playerSoul == null)
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    if (player != null) _playerSoul = player.GetComponent<PlayerSoul>();
                }
            }
        }

        private void OnEnable()
        {
            if (_playerSoul != null)
            {
                _playerSoul.OnSoulChanged += HandleSoulChanged;
            }
        }

        private void OnDisable()
        {
            if (_playerSoul != null)
            {
                _playerSoul.OnSoulChanged -= HandleSoulChanged;
            }
        }

        private void Start()
        {
            if (_playerSoul != null)
            {
                // Snap instantly to the correct starting value, independent of event subscription order
                SetFillInstant(_playerSoul.CurrentSoul / _playerSoul.MaxSoul);
                UpdateVisibility(_playerSoul.CurrentSoul, _playerSoul.MaxSoul);
            }
            else
            {
                Debug.LogWarning("SoulBarUI: PlayerSoul component could not be found!", this);
            }
        }

        private void HandleSoulChanged(float current, float max)
        {
            UpdateVisibility(current, max);
            if (_animateRoutine != null) StopCoroutine(_animateRoutine);
            _animateRoutine = StartCoroutine(AnimateFill(current / max));
        }

        private void UpdateVisibility(float current, float max)
        {
            if (_root == null || !_hideWhenFull) return;
            // Using Mathf.Approximately to safely compare floats
            _root.SetActive(!Mathf.Approximately(current, max));
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