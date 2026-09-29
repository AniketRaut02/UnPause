using DarkPixelGD.Combat;
using DarkPixelGD.Systems.Combat;
using System.Collections;
using UnityEngine;

namespace DarkPixelGD.Enemy
{
    public class EnemyShooter : MonoBehaviour
    {
        [SerializeField] private Transform _muzzle;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private float _fireInterval = 1.5f;
        [SerializeField] private int _minDamage = 5;
        [SerializeField] private int _maxDamage = 15;

        [Header("Animator Refrences")]
        [SerializeField] private GunFireAnimator _gunAnimator;
        [SerializeField] private MuzzleFlash _muzzleFlash;

        private void Start() => StartCoroutine(FireLoop());

        private IEnumerator FireLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(_fireInterval); // scaled: pauses automatically
                Fire();
            }
        }

        private void Fire()
        {
            var bullet = Instantiate(_bulletPrefab, _muzzle.position, _muzzle.rotation);
            int damage = Random.Range(_minDamage, _maxDamage + 1);
            bullet.GetComponent<Projectile>().Initialize(damage, "Player");
            _gunAnimator.PlayFire();
            _muzzleFlash.Flash();
        }
    }
}