using UnityEngine;

namespace DarkPixelGD.Story
{
    [System.Serializable]
    public class StoryPage
    {
        [TextArea(3, 8)] public string text;
        public float holdDurationAfterTyping = 1.5f; // minimum read time, even if Continue is spam-clicked
    }

    [CreateAssetMenu(fileName = "StoryData", menuName = "Story/StoryDataSO")]
    public class StoryDataSO : ScriptableObject
    {
        public StoryPage[] pages;
    }
}