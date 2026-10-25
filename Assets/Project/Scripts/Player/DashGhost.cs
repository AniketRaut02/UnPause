using System.Collections;
using UnityEngine;

namespace DarkPixelGD.Player
{
    public class DashGhost : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _fadeDuration = 0.3f;
        [SerializeField] private Color _ghostColor = new Color(0f, 0.8f, 1f, 0.6f); // Cyan/neon accent fit

        private void Awake()
        {
            if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Initialize(Sprite sprite, Vector3 position, Quaternion rotation, Vector3 scale, bool flipY)
        {
            transform.position = position;
            transform.rotation = rotation;
            transform.localScale = scale;

            if (_spriteRenderer != null)
            {
                _spriteRenderer.sprite = sprite;
                _spriteRenderer.flipY = flipY;
                _spriteRenderer.color = _ghostColor;
            }

            StartCoroutine(FadeAndDestroy());
        }

        private IEnumerator FadeAndDestroy()
        {
            float elapsed = 0f;
            Color startColor = _spriteRenderer.color;

            while (elapsed < _fadeDuration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(startColor.a, 0f, elapsed / _fadeDuration);
                _spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}