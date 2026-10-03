using DarkPixelGD.Audio;
using DarkPixelGD.Input;
using DarkPixelGD.UI;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DarkPixelGD.Story
{
    public class StorySequenceController : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private StoryDataSO _storyData;

        [Header("UI")]
        [SerializeField] private TMP_Text _label;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _skipButton; // optional — jumps straight to next scene

        [Header("Typing")]
        [SerializeField] private float _charsPerSecond = 35f;

        [Header("Audio")]
        [SerializeField] private InputReader _inputReader;
        [SerializeField] private AudioEventChannelSO _audioChannel;
        [SerializeField] private SoundDataSO _typeSound;       // short blip, loop = false, spatialBlend = 0
        [SerializeField] private SoundDataSO _backgroundMusic; // loop = true, spatialBlend = 0

        [Header("Scene Flow")]
        [SerializeField] private string _nextSceneName = "Tutorial";

        private int _currentPageIndex;
        private bool _isTyping;
        private bool _skipRequested;
        private Coroutine _typingRoutine;

        private void Start()
        { // Player map (and ESC/menu-open with it) is off for this whole scene

            _continueButton.onClick.AddListener(OnContinuePressed);
            if (_skipButton != null) _skipButton.onClick.AddListener(OnSkipAllPressed);

            if (_backgroundMusic != null)
                _audioChannel.RaisePlaySound(_backgroundMusic, Vector3.zero);

            ShowPage(0);
        }

        private void ShowPage(int index)
        {
            _currentPageIndex = index;
            _typingRoutine = StartCoroutine(TypePage(_storyData.pages[index]));
        }

        private IEnumerator TypePage(StoryPage page)
        {
            _isTyping = true;
            _label.text = "";
            float delay = 1f / _charsPerSecond;

            foreach (char c in page.text)
            {
                _label.text += c;
                if (!char.IsWhiteSpace(c))
                    _audioChannel?.RaisePlaySound(_typeSound, Vector3.zero);

                float elapsed = 0f;
                while (elapsed < delay)
                {
                    if (_skipRequested)
                    {
                        _skipRequested = false;
                        CompleteTypingInstantly(page);
                        yield break;
                    }
                    elapsed += Time.deltaTime;
                    yield return null;
                }
            }

            _isTyping = false;
            yield return HoldBeforeAdvance(page.holdDurationAfterTyping);
        }

        private void CompleteTypingInstantly(StoryPage page)
        {
            _label.text = page.text;
            _isTyping = false;
            StartCoroutine(HoldBeforeAdvance(page.holdDurationAfterTyping));
        }

        private IEnumerator HoldBeforeAdvance(float holdDuration)
        {
            yield return new WaitForSeconds(holdDuration); // no special-case needed — timeScale is always 1 here
        }
            

        private void OnContinuePressed()
        {
            if (_isTyping) { _skipRequested = true; return; } // first click: finish current page instantly

            int next = _currentPageIndex + 1;
            if (next < _storyData.pages.Length) ShowPage(next);
            else EndStory();
        }

        private void OnSkipAllPressed() => EndStory();

        private void EndStory()
        {
            if (_typingRoutine != null) StopCoroutine(_typingRoutine);

            // Important: your pooled background loop lives on the persistent AudioManager and will
            // keep playing straight into Tutorial's own gameplay audio unless explicitly stopped here.
            if (_backgroundMusic != null)
                _audioChannel.RaiseStopSound(_backgroundMusic, Vector3.zero);

            SceneLoader.Instance.LoadScene(_nextSceneName);
        }
    }
}