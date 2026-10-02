using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DarkPixelGD.UI
{
    public class LoadingScreenUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeDuration = 0.4f;

        [Header("Progress (optional — only shown when a load explicitly requests it)")]
        [SerializeField] private GameObject _progressGroup; // parent of bar + label, inactive by default
        [SerializeField] private Slider _progressBar;
        [SerializeField] private TMP_Text _progressLabel;

        

        private void Awake()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
        }

        public IEnumerator FadeOut() // black screen appears
        {
            _canvasGroup.blocksRaycasts = true;
            yield return Fade(0.8f, 1f);
        }

        public IEnumerator FadeIn() // black screen clears
        {
            yield return Fade(1f, 0f);
            _canvasGroup.blocksRaycasts = false;
        }

        private IEnumerator Fade(float from, float to)
        {
            float elapsed = 0f;
            _canvasGroup.alpha = from;
            while (elapsed < _fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime; // unscaled: must complete even if timeScale is 0 at the moment of death
                _canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / _fadeDuration);
                yield return null;
            }
            _canvasGroup.alpha = to;
        }

        public void SetProgressVisible(bool visible) => _progressGroup.SetActive(visible);

        public void SetProgress(float value01)
        {
            if (_progressBar != null) _progressBar.value = value01;
            if (_progressLabel != null) _progressLabel.text = $"Loading... {Mathf.RoundToInt(value01 * 100)}%";
        }
    }
}