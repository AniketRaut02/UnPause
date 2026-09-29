using System;
using UnityEngine;

namespace DarkPixelGD.Systems.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int _maxHealth = 100;
        private int _currentHealth;
        private bool _isInvulnerable;

        public event Action OnDeath;
        public event Action<int, int> OnHealthChanged;
        public event Action<int> OnDamageTaken;


        //Exposed for UI
        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _maxHealth;


        private void Awake() => _currentHealth = _maxHealth;


        public void SetInvulnerable(bool value) { _isInvulnerable = value; }

        public void TakeDamage(int amount)
        {
            if (_isInvulnerable || _currentHealth <= 0) return;

            _currentHealth = Mathf.Max(0, _currentHealth - amount);

            OnDamageTaken?.Invoke(amount);

            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

            if (_currentHealth <= 0) OnDeath?.Invoke();
        }

        public bool Kill()
        {
            if (_isInvulnerable || _currentHealth <= 0) return false;

            _currentHealth = 0;

            OnHealthChanged?.Invoke(0, _maxHealth);

            OnDeath?.Invoke();

            return true;
        }

        public void Heal(int amount)
        {
            if (_currentHealth <= 0) return; // no healing a corpse
            _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }
    }
}