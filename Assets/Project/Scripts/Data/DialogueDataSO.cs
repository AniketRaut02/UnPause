using UnityEngine;

namespace DarkPixelGD.Narrator
{
    [System.Serializable]
    public class DialogueLine
    {
        [TextArea] public string text;
        public AudioClip voiceClip;
        public float minDisplayDuration = 2f; // fallback timing if no clip, or if not skipped
    }

    [CreateAssetMenu(fileName = "DialogueData", menuName = "Narrator/DialogueDataSO")]
    public class DialogueDataSO : ScriptableObject
    {
        public string speakerName;
        public DialogueLine[] lines;
    }
}