using DarkPixelGD.Audio;
using DarkPixelGD.Juice;
using DarkPixelGD.Systems.Combat;
using DarkPixelGD.Systems.Manipulation;
using UnityEngine;

namespace DarkPixelGD.Systems.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 15f;
        [SerializeField] private float _lifetime = 3f;

        [Header("Audio")]
        [SerializeField] private AudioEventChannelSO _audioChannel;
        [SerializeField] private SoundDataSO _hitSound;

        [Header("Juice Refrences")]
        [SerializeField] private JuiceEventChannelSO _juiceChannel;
        [SerializeField] private JuiceProfileSO _damageJuiceProfile;


        private int _damage;
        private string _targetTag; // "Player" or "Enemy" — only this tag takes damage

        public void Initialize(int damage, string targetTag)
        {
            _damage = damage;
            _targetTag = targetTag;
            Destroy(gameObject, _lifetime);
        }

        private void FixedUpdate() // freezes automatically at timeScale = 0
        {
            transform.position += transform.right * _speed * Time.fixedDeltaTime;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(_targetTag) && other.TryGetComponent(out Health health))
            {
                health.TakeDamage(_damage);
                if (_audioChannel != null && _hitSound != null) { _audioChannel.RaisePlaySound(_hitSound, transform.position); }
                if (_juiceChannel!=null&&_damageJuiceProfile!=null) _juiceChannel.RaiseJuice(_damageJuiceProfile, transform.position);
                Destroy(gameObject);
            }
            else if (other.GetComponent<MovableObject>() != null)
            {
                Destroy(gameObject); // blocked by manipulable walls, same physics-first principle as your lasers
            }
        }
    }
}