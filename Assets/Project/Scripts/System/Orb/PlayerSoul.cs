using UnityEngine;
using System;

namespace DarkPixelGD.Player
{
    public class PlayerSoul : MonoBehaviour
    {
        [Header("Soul Settings")]
        [SerializeField] private float _maxSoul = 100f;
        private float _currentSoul;

        public float CurrentSoul => _currentSoul;
        public float MaxSoul => _maxSoul;

        // Event to notify UI or other systems when soul changes
        public event Action<float, float> OnSoulChanged;

        private void Awake()
        {
            _currentSoul = 0;
        }

        private void Start()
        {
            // Initial UI sync
            OnSoulChanged?.Invoke(_currentSoul, _maxSoul);
        }

        public void RestoreSoul(float amount)
        {
            _currentSoul = Mathf.Clamp(_currentSoul + amount, 0f, _maxSoul);
            OnSoulChanged?.Invoke(_currentSoul, _maxSoul);
            if(_currentSoul == _maxSoul)
            {
                HintPopupUI.Instance.Show("GF's Soul Restored!");
            }
        }
    }
}