using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DarkPixelGD.Tutorial
{
    public class TutorialPromptUI : MonoBehaviour
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private TMP_Text _titleLabel;
        [SerializeField] private TMP_Text _messageLabel;
        [SerializeField] private Image _iconImage; // optional

        private void Awake() => _root.SetActive(false);

        private void Start()
        {
            TutorialPromptManager.Instance.OnPromptShown += HandleShown;
            TutorialPromptManager.Instance.OnPromptClosed += HandleClosed;
        }

        private void OnDestroy()
        {
            if (TutorialPromptManager.Instance == null) return;
            TutorialPromptManager.Instance.OnPromptShown -= HandleShown;
            TutorialPromptManager.Instance.OnPromptClosed -= HandleClosed;
        }

        private void HandleShown(TutorialPromptDataSO data)
        {
            _root.SetActive(true);
            _titleLabel.text = data.title;
            _messageLabel.text = data.message;
            if (_iconImage != null) _iconImage.sprite = data.icon;
        }

        private void HandleClosed() => _root.SetActive(false);

        // Hook to the Close button's OnClick()
        public void OnCloseButtonPressed() => TutorialPromptManager.Instance.CloseCurrent();
    }
}