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
        [SerializeField] private int _damage = 10;

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
            bullet.GetComponent<Projectile>().Initialize(_damage, "Player");
        }
    }
}