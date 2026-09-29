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

        public void LoadScene(string sceneName) => StartCoroutine(LoadRoutine(sceneName));

        private IEnumerator LoadRoutine(string sceneName)
        {
            yield return _loadingScreen.FadeOut();

            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            while (!op.isDone) yield return null;

            yield return _loadingScreen.FadeIn();
        }
    }
}