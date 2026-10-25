using System.Collections;
using UnityEngine;
using DarkPixelGD.Narrator;
using DarkPixelGD.UI;

namespace DarkPixelGD.Core
{
    public class IntroDialogueSequenceLoader : MonoBehaviour
    {
        [Header("Dialogue Sequence")]
        [SerializeField] private DialogueDataSO[] _dialogueSequence;
        [SerializeField] private string _nextSceneName;

        [Header("Settings")]
        [SerializeField] private float _delayBeforeStart = 0.5f;
        [SerializeField] private float _delayBetweenDialogues = 0.3f; // Brief pause between consecutive dialogue blocks

        private void Start()
        {
            if (_dialogueSequence == null || _dialogueSequence.Length == 0)
            {
                Debug.LogWarning($"[{nameof(IntroDialogueSequenceLoader)}] No dialogue sequence assigned! Loading next scene immediately.");
                LoadNextScene();
                return;
            }

            StartCoroutine(PlaySequenceRoutine());
        }

        private IEnumerator PlaySequenceRoutine()
        {
            if (_delayBeforeStart > 0f)
            {
                yield return new WaitForSecondsRealtime(_delayBeforeStart);
            }

            // Loop through each DialogueDataSO in order
            for (int i = 0; i < _dialogueSequence.Length; i++)
            {
                DialogueDataSO currentDialogue = _dialogueSequence[i];
                if (currentDialogue == null) continue;

                bool dialogueFinished = false;

                // Local callback handler for when *this specific* dialogue ends
                void OnDialogueEndedHandler()
                {
                    NarratorService.Instance.OnDialogueEnded -= OnDialogueEndedHandler;
                    dialogueFinished = true;
                }

                // Subscribe and play
                NarratorService.Instance.OnDialogueEnded += OnDialogueEndedHandler;
                NarratorService.Instance.Play(currentDialogue);

                // Wait until the current dialogue sequence finishes playing
                while (!dialogueFinished)
                {
                    yield return null;
                }

                // Optional tiny breathing room between different dialogue assets
                if (i < _dialogueSequence.Length - 1 && _delayBetweenDialogues > 0f)
                {
                    yield return new WaitForSecondsRealtime(_delayBetweenDialogues);
                }
            }

            // Once all dialogues in the array are done, load the next scene
            LoadNextScene();
        }

        private void LoadNextScene()
        {
            if (!string.IsNullOrEmpty(_nextSceneName))
            {
                if (SceneLoader.Instance != null)
                {
                    SceneLoader.Instance.LoadScene(_nextSceneName);
                }
                else
                {
                    Debug.LogError($"[{nameof(IntroDialogueSequenceLoader)}] SceneLoader instance not found in the scene!");
                }
            }
            else
            {
                Debug.LogWarning($"[{nameof(IntroDialogueSequenceLoader)}] Next scene name is not configured!");
            }
        }
    }
}