using DarkPixelGD.UI;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private string _firstLevelSceneName = "Tutorial";

    public void OnPlayPressed() => SceneLoader.Instance.LoadScene(_firstLevelSceneName, showProgress: true, minimumDisplayTime: 1f);
    public void OnOptionsPressed() => _optionsPanel.SetActive(true);
    public void OnQuitPressed() => Application.Quit();
}