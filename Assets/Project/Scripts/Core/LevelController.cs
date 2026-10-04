using DarkPixelGD.Interaction;
using DarkPixelGD.Juice;
using DarkPixelGD.Player;
using DarkPixelGD.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace DarkPixelGD.Core
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private PortalTrigger portal;
        [SerializeField] private LevelConfig config;
        [SerializeField] private PlayerController player;
        [SerializeField] private Transform _spawnPoint;

        [Header("Explosion")]
        [SerializeField] private GameObject _explosionPrefab;
        [SerializeField] private float _explosionDelayBeforeFade = 0.5f;

        [Header("Juice Refrences")]
        [SerializeField] private JuiceEventChannelSO _juiceChannel;
        [SerializeField] private JuiceProfileSO _explosionJuiceProfile;
        private bool _hasUsedFreeDeath;
        private bool _hasEnded;

        private void Start()
        {
            if (config.startWithDashLocked)
                player.SetDashLocked(true);
            if (config.startWithInvertedControls)
                player.SetControlsInverted(true);
            player.SetHasKey(false);

            portal.OnLevelComplete += HandleLevelComplete;
            TimerService.Instance.OnTimerExpired += HandleTimerExpired;
            TimerService.Instance.StartTimer(config.timerDuration);
        }

        private void OnDestroy()
        {
            portal.OnLevelComplete -= HandleLevelComplete;
            if (TimerService.Instance != null)
                TimerService.Instance.OnTimerExpired -= HandleTimerExpired;
        }

        private void HandleLevelComplete()
        {
            if (_hasEnded) return;
            _hasEnded = true;

            PauseService.Instance.ForceUnpause();
            if (!string.IsNullOrEmpty(config.nextSceneName))
                SceneLoader.Instance.LoadScene(config.nextSceneName);
            else
                Debug.LogWarning("No next scene configured on this LevelConfig.");
        }

        private void HandleTimerExpired()
        {
            if (_hasEnded) return; // already transitioning out via the portal — ignore a late/racing death

            PauseService.Instance.ForceUnpause();

            if (config.firstDeathIsFree && !_hasUsedFreeDeath)
            {
                _hasUsedFreeDeath = true;
                HintPopupUI.Instance?.Show("Try pausing.");
                StartCoroutine(RespawnWithInvulnerability());
                return; // level continues, so _hasEnded intentionally stays false
            }

            _hasEnded = true;
            StartCoroutine(ExplodeAndReload());
        }
        private IEnumerator ExplodeAndReload()
        {
            if (_explosionPrefab != null)
                Instantiate(_explosionPrefab, player.transform.position, Quaternion.identity);

            _juiceChannel.RaiseJuice(_explosionJuiceProfile, player.transform.position);

            yield return new WaitForSecondsRealtime(_explosionDelayBeforeFade);
            SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().name);
        }
        private IEnumerator RespawnWithInvulnerability()
        {
            player.Teleport(_spawnPoint.position);
           // player.SetInvulnerable(true);
            TimerService.Instance.StartTimer(config.timerDuration);

            yield return new WaitForSecondsRealtime(1f); // unscaled: safe even if paused mid-window
            player.SetInvulnerable(false);
        }

    }
}