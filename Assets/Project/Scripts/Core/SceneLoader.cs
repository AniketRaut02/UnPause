using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DarkPixelGD.UI
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }
        [SerializeField] private LoadingScreenUI _loadingScreen;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        public void LoadScene(string sceneName, bool showProgress = false, float minimumDisplayTime = 0f)
        {
            StartCoroutine(LoadRoutine(sceneName, showProgress, minimumDisplayTime));
        }

        private IEnumerator LoadRoutine(string sceneName, bool showProgress, float minimumDisplayTime)
        {
            if (showProgress) _loadingScreen.SetProgressVisible(true);
            yield return _loadingScreen.FadeOut();

            float startTime = Time.unscaledTime;
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            op.allowSceneActivation = false;

            while (op.progress < 0.9f) // Unity caps progress at 0.9 until activation is allowed
            {
                if (showProgress) _loadingScreen.SetProgress(op.progress / 0.9f);
                yield return null;
            }
            if (showProgress) _loadingScreen.SetProgress(1f);

            while (Time.unscaledTime - startTime < minimumDisplayTime) yield return null;

            op.allowSceneActivation = true;
            while (!op.isDone) yield return null;

            PauseService.Instance.UnlockForNewScene();

            yield return _loadingScreen.FadeIn();
            if (showProgress) _loadingScreen.SetProgressVisible(false);
        }
    }
}