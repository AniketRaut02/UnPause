using DarkPixelGD.UI;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject _root;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private string _mainMenuSceneName = "MainMenu";

    public void SetVisible(bool visible) => _root.SetActive(visible);
    public void OnResumePressed() => PauseMenuController.Instance.SetMenuOpen(false);
    public void OnOptionsPressed() => _optionsPanel.SetActive(true);

    public void OnMainMenuPressed()
    {
        PauseMenuController.Instance.SetMenuOpen(false); // release the pause request cleanly before leaving the scene
        SceneLoader.Instance.LoadScene(_mainMenuSceneName);
    }
}