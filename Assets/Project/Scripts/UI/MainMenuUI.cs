using DarkPixelGD.UI;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private string _storySceneName = "Story";
    [SerializeField] private string _aftermathStorySceneName = "Ending";


    public void OnPlayPressed() => SceneLoader.Instance.LoadScene(_storySceneName, showProgress: true, minimumDisplayTime: 1f);
    public void OnPlayAftermathPressed() => SceneLoader.Instance.LoadScene(_aftermathStorySceneName, showProgress: true, minimumDisplayTime: 1f);
    public void OnOptionsPressed() => _optionsPanel.SetActive(true);
    public void OnQuitPressed() => Application.Quit();
}