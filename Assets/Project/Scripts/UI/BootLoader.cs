using DarkPixelGD.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour
{
    [SerializeField] private LoadingScreenUI _loadingScreen;
    [SerializeField] private string _mainMenuSceneName = "MainMenu";
    [SerializeField] private float _minimumDisplayTime = 1.5f; // avoids an instant flash on fast loads

    private void Start() => StartCoroutine(BootRoutine());

    private IEnumerator BootRoutine()
    {
        yield return _loadingScreen.FadeOut(); // covers the screen immediately on launch

        float startTime = Time.unscaledTime;
        AsyncOperation op = SceneManager.LoadSceneAsync(_mainMenuSceneName);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f) yield return null; // Unity caps progress at 0.9 until activation is allowed
        while (Time.unscaledTime - startTime < _minimumDisplayTime) yield return null;

        op.allowSceneActivation = true;
        while (!op.isDone) yield return null;

        yield return _loadingScreen.FadeIn();
    }
}