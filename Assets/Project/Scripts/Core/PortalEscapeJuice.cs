using System.Collections;
using UnityEngine;
using DarkPixelGD.Interaction;
using DarkPixelGD.Juice;

namespace DarkPixelGD.Systems.Cinematics
{
    [RequireComponent(typeof(PortalTrigger))]
    public class PortalEscapeJuice : MonoBehaviour
    {
        [Header("Timing Delay")]
        [SerializeField] private float _explosionDelay = 0.4f; // Time in seconds after portal entry before explosion triggers

        [Header("Escape Explosion Sequence")]
        [SerializeField] private GameObject _backgroundExplosionPrefab;
        [SerializeField] private Transform[] _explosionSpawnPoints;

        [Header("Juice References")]
        [SerializeField] private JuiceEventChannelSO _juiceChannel;
        [SerializeField] private JuiceProfileSO _portalEscapeJuiceProfile;

        private PortalTrigger _portalTrigger;

        private void Awake()
        {
            _portalTrigger = GetComponent<PortalTrigger>();
        }

        private void OnEnable()
        {
            if (_portalTrigger != null)
            {
                _portalTrigger.OnLevelComplete += HandleLevelComplete;
            }
        }

        private void OnDisable()
        {
            if (_portalTrigger != null)
            {
                _portalTrigger.OnLevelComplete -= HandleLevelComplete;
            }
        }

        private void HandleLevelComplete()
        {
            // Start a coroutine to delay the explosion slightly after escape
            StartCoroutine(DelayedExplosionRoutine());
        }

        private IEnumerator DelayedExplosionRoutine()
        {
            // Wait for the player to slip through the portal first
            yield return new WaitForSeconds(_explosionDelay);

            // 1. Raise screen juice effects
            if (_juiceChannel != null && _portalEscapeJuiceProfile != null)
            {
                _juiceChannel.RaiseJuice(_portalEscapeJuiceProfile, transform.position);
            }

            // 2. Spawn background explosions
            if (_backgroundExplosionPrefab != null)
            {
                if (_explosionSpawnPoints != null && _explosionSpawnPoints.Length > 0)
                {
                    foreach (var point in _explosionSpawnPoints)
                    {
                        if (point != null)
                        {
                            Instantiate(_backgroundExplosionPrefab, point.position, Quaternion.identity);
                        }
                    }
                }
                else
                {
                    Instantiate(_backgroundExplosionPrefab, transform.position, Quaternion.identity);
                }
            }
        }
    }
}