using UnityEngine;

namespace DarkPixelGD.Tutorial
{
    [CreateAssetMenu(fileName = "TutorialPrompt", menuName = "Tutorial/TutorialPromptDataSO")]
    public class TutorialPromptDataSO : ScriptableObject
    {
        public string title;
        [TextArea] public string message;
        public Sprite icon; // optional
    }
}