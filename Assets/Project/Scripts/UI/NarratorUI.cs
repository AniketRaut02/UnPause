using TMPro;
using UnityEngine;

namespace DarkPixelGD.Narrator
{
    public class NarratorUI : MonoBehaviour
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private TMP_Text _speakerLabel;
        [SerializeField] private TMP_Text _lineLabel;

        private void Start()
        {
            NarratorService.Instance.OnDialogueStarted += HandleStarted;
            NarratorService.Instance.OnLineChanged += HandleLineChanged;
            NarratorService.Instance.OnDialogueEnded += HandleEnded;
        }

        private void OnDestroy()
        {
            if (NarratorService.Instance == null) return;
            NarratorService.Instance.OnDialogueStarted -= HandleStarted;
            NarratorService.Instance.OnLineChanged -= HandleLineChanged;
            NarratorService.Instance.OnDialogueEnded -= HandleEnded;
        }

        private void HandleStarted(DialogueDataSO data)
        {
            _root.SetActive(true);
            _speakerLabel.text = data.speakerName;
        }

        private void HandleLineChanged(DialogueLine line, int index) => _lineLabel.text = line.text;

        private void HandleEnded() => _root.SetActive(false);
    }
}