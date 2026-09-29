using DarkPixelGD.Interaction;
using DarkPixelGD.Player;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


namespace DarkPixelGD.Core
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private PortalTrigger portal;
        [SerializeField] private LevelConfig config;
        [SerializeField] private PlayerController player;
        [SerializeField] private Transform _spawnPoint;

        private bool _hasUsedFreeDeath;

        private void Start()
        {
            if (config.startWithDashLocked)
                player.SetDashLocked(true);
            if (config.startWithInvertedControls)
                player.SetControlsInverted(true);

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
            PauseService.Instance.SetPaused(false); // safety: never load next scene mid-pause
            if (!string.IsNullOrEmpty(config.nextSceneName))
                SceneManager.LoadScene(config.nextSceneName);
            else
                Debug.LogWarning("No next scene configured on this LevelConfig.");
        }

        private void HandleTimerExpired()
        {
            PauseService.Instance.SetPaused(false);

            if (config.firstDeathIsFree && !_hasUsedFreeDeath)
            {
                _hasUsedFreeDeath = true;
                HintPopupUI.Instance?.Show("Try pausing.");
                StartCoroutine(RespawnWithInvulnerability());
                return;
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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