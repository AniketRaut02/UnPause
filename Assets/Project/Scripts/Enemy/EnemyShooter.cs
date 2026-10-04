using DarkPixelGD.Audio;
using DarkPixelGD.Combat;
using DarkPixelGD.Juice;
using DarkPixelGD.Systems.Combat;
using System.Collections;
using UnityEngine;

namespace DarkPixelGD.Enemy
{
    public class EnemyShooter : MonoBehaviour
    {
        [SerializeField] private Transform _muzzle;
        [SerializeField] private Transform _shootJuicePoint;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _player;
        [SerializeField] private float _shootRange = 6f;
        [SerializeField] private float _fireInterval = 1.5f;
        [SerializeField] private int _minDamage = 5;
        [SerializeField] private int _maxDamage = 15;

        [Header("Animator Refrences")]
        [SerializeField] private GunFireAnimator _gunAnimator;
        [SerializeField] private MuzzleFlash _muzzleFlash;

        [Header("Audio")]
        [SerializeField] private AudioEventChannelSO _audioChannel;
        [SerializeField] private SoundDataSO _fireSound;

        [Header("Juice Refrences")]
        [SerializeField] private JuiceEventChannelSO _juiceChannel;
        [SerializeField] private JuiceProfileSO _fireJuiceProfile;

        private void Start() => StartCoroutine(FireLoop());

        private IEnumerator FireLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(_fireInterval); // scaled: pauses automatically
                if(IsPlayerInRange())Fire();
            }
        }

        private bool IsPlayerInRange()
        {
            if (_player == null) return false;
            float sqrDist = (_player.position - transform.position).sqrMagnitude;
            return sqrDist <= _shootRange * _shootRange; // sqrMagnitude avoids a sqrt per check
        }
        private void Fire()
        {
            var bullet = Instantiate(_bulletPrefab, _muzzle.position, _muzzle.rotation);
            int damage = Random.Range(_minDamage, _maxDamage + 1);
            bullet.GetComponent<Projectile>().Initialize(damage, "Player");
            _gunAnimator.PlayFire();
            _muzzleFlash.Flash();
            if(_audioChannel!=null && _fireSound!=null)_audioChannel.RaisePlaySound(_fireSound, transform.position);
            if (_juiceChannel != null && _fireJuiceProfile != null) _juiceChannel.RaiseJuice(_fireJuiceProfile,_shootJuicePoint.position);
        }
    }
}