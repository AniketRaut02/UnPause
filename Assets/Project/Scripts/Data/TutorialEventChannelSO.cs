using System;
using UnityEngine;

namespace DarkPixelGD.Tutorial
{
    [CreateAssetMenu(fileName = "TutorialEventChannel", menuName = "Tutorial/TutorialEventChannelSO")]
    public class TutorialEventChannelSO : ScriptableObject
    {
        public event Action<TutorialPromptDataSO, bool> OnShowRequested;

        // blocking = true freezes the game via PauseService while this prompt is open.
        public void RaiseShow(TutorialPromptDataSO data, bool blocking = true) =>
            OnShowRequested?.Invoke(data, blocking);
    }
}