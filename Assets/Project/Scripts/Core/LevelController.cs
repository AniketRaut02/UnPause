using DarkPixelGD.Interaction;
using DarkPixelGD.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] private PortalTrigger portal;
    [SerializeField] private LevelConfig config;
    [SerializeField] private PlayerController player;

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
        Debug.Log("Level Complete!");
        Time.timeScale = 0f; // placeholder until scene-transition flow (Day 6)
    }

    private void HandleTimerExpired()
    {
        if (config.firstDeathIsFree && !_hasUsedFreeDeath)
        {
            _hasUsedFreeDeath = true;
            Debug.Log("First death forgiven — respawning in place. (Hint UI lands Day 5.)");
            TimerService.Instance.StartTimer(config.timerDuration);
            return;
        }

        Debug.Log("Level exploded — restarting");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}