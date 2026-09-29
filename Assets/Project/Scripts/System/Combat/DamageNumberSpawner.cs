using UnityEngine;

namespace DarkPixelGD.Systems.Combat
{
    public class DamageNumberSpawner : MonoBehaviour
    {
        [SerializeField] private Health _health; 
        [SerializeField] private GameObject _damageNumberPrefab;
        [SerializeField] private Transform _spawnPoint; 
        [SerializeField] private Vector2 _randomOffsetRange = new Vector2(0.2f, 0.2f);

        private void Awake()
        {
            if (_health == null) _health = GetComponent<Health>();
        }

        private void OnEnable() => _health.OnDamageTaken += HandleDamageTaken;
        private void OnDisable() => _health.OnDamageTaken -= HandleDamageTaken;

        private void HandleDamageTaken(int amount)
        {
            Vector3 spawnPos = (_spawnPoint != null ? _spawnPoint.position : transform.position)
                + new Vector3(Random.Range(-_randomOffsetRange.x, _randomOffsetRange.x),
                              Random.Range(0f, _randomOffsetRange.y), 0f);

            var obj = Instantiate(_damageNumberPrefab, spawnPos, Quaternion.identity);
            obj.GetComponent<DamageNumberUI>().Show(amount);
        }
    }
}