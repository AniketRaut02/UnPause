using DarkPixelGD.Systems.Combat;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace DarkPixelGD.Player
{
    public class PlayerShooting : MonoBehaviour
    {
        [SerializeField] private Transform _aimPivot;  
        [SerializeField] private Transform _muzzle;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private int _damage = 25;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private SpriteRenderer _aimPivotRenderer;
        public Vector2 AimDirection { get; private set; }


        private void Update()
        {
            if (PauseService.Instance != null && PauseService.Instance.IsPaused) return;

            AimAtMouse();
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
                Shoot();
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
            bullet.GetComponent<Projectile>().Initialize(_damage, "Enemy");
        }
       

    }
}