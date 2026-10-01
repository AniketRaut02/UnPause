using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkPixelGD.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioEventChannelSO _channel;
        [SerializeField] private int _poolSize = 16;

        private readonly Queue<AudioSource> _pool = new();
        private readonly Dictionary<SoundDataSO, AudioSource> _activeLoops = new();

        private void Awake()
        {
            for (int i = 0; i < _poolSize; i++)
            {
                var go = new GameObject($"PooledSource_{i}");
                go.transform.SetParent(transform);
                var source = go.AddComponent<AudioSource>();
                source.playOnAwake = false;
                _pool.Enqueue(source);
            }
        }

        private void OnEnable()
        {
            _channel.OnPlayRequested += HandlePlayRequested;
            _channel.OnStopRequested += HandleStopRequested;
        }

        private void OnDisable()
        {
            _channel.OnPlayRequested -= HandlePlayRequested;
            _channel.OnStopRequested -= HandleStopRequested;
        }

        // Singleton-ordering-safe, same convention as your other services
        private void Start() => PauseService.Instance.OnPauseChanged += HandleGamePaused;
        private void OnDestroy()
        {
            if (PauseService.Instance != null)
                PauseService.Instance.OnPauseChanged -= HandleGamePaused;
        }

        private void HandlePlayRequested(SoundDataSO sound, Vector3 position)
        {
            AudioClip clip = sound.GetClip();
            if (clip == null) return;

            AudioSource source = GetSource();
            source.transform.position = position;
            source.clip = clip;
            source.outputAudioMixerGroup = sound.mixerGroup;
            source.volume = sound.GetVolume();
            source.pitch = sound.GetPitch();
            source.spatialBlend = sound.spatialBlend;
            source.loop = sound.loop;
            source.Play();

            if (sound.loop)
                _activeLoops[sound] = source; // held until explicitly stopped, not auto-returned to pool
            else
                StartCoroutine(ReturnToPoolWhenDone(source, clip.length / Mathf.Max(0.01f, source.pitch)));
        }

        private void HandleStopRequested(SoundDataSO sound, Vector3 position)
        {
            if (_activeLoops.TryGetValue(sound, out var source))
            {
                _activeLoops.Remove(sound);
                ReturnSource(source);
            }
        }

        // Unity's AudioSource does NOT respect Time.timeScale by default — a looping laser hum
        // will keep playing at normal speed right through a carve-mode or menu pause unless
        // explicitly paused here. This is the one non-obvious gotcha in this whole system.
        private void HandleGamePaused(bool isPaused)
        {
            foreach (var source in _activeLoops.Values)
            {
                if (isPaused) source.Pause();
                else source.UnPause();
            }
        }

        private IEnumerator ReturnToPoolWhenDone(AudioSource source, float delay)
        {
            yield return new WaitForSeconds(delay);
            ReturnSource(source);
        }

        private AudioSource GetSource()
        {
            if (_pool.Count > 0) return _pool.Dequeue();

            // Pool exhausted — grow rather than steal. A jam-scale scene won't hit this often;
            // if it does regularly, raise _poolSize rather than adding priority/stealing logic.
            var go = new GameObject("PooledSource_Overflow");
            go.transform.SetParent(transform);
            return go.AddComponent<AudioSource>();
        }

        private void ReturnSource(AudioSource source)
        {
            source.Stop();
            source.clip = null;
            _pool.Enqueue(source);
        }
    }
}