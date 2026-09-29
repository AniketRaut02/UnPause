using DarkPixelGD.Combat;
using DarkPixelGD.Systems.Combat;
using DarkPixelGD.VFX;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DarkPixelGD.Player
{
    public class PlayerShooting : MonoBehaviour
    {
        [SerializeField] private Transform _aimPivot;  
        [SerializeField] private Transform _muzzle;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private int _minDamage = 15;
        [SerializeField] private int _maxDamage = 30;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private SpriteRenderer _aimPivotRenderer;

        [Header("Shooting Properties")]
        [SerializeField] private float _fireCooldown = 0.15f;
        private float _nextFireTime;

        [Header("Animator Refrences")]
        [SerializeField] private GunFireAnimator _gunAnimator;
        [SerializeField] private MuzzleFlash _muzzleFlash;


        public Vector2 AimDirection { get; private set; }


        private void Update()
        {
            if (PauseService.Instance != null && PauseService.Instance.IsPaused) return;

            AimAtMouse();
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && Time.time >= _nextFireTime)
            {
                Shoot();
                _nextFireTime = Time.time + _fireCooldown;
            }
        }

        private void AimAtMouse()
        {
            Vector2 mouseWorld = _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 direction = mouseWorld - (Vector2)_aimPivot.position;
            AimDirection = direction.normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _aimPivot.rotation = Quaternion.Euler(0, 0, angle);

            bool aimingLeft = Mathf.Abs(angle) > 90f;
            _aimPivotRenderer.flipY = aimingLeft;
        }

        private void Shoot()
        {
            var bullet = Instantiate(_bulletPrefab, _muzzle.position, _muzzle.rotation);
            int damage = Random.Range(_minDamage, _maxDamage + 1); // max is exclusive on ints, hence +1
            bullet.GetComponent<Projectile>().Initialize(damage, "Enemy");
            _gunAnimator.PlayFire();
            _muzzleFlash.Flash();
        }


    }
}