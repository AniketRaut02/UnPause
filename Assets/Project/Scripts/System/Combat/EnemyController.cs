using UnityEngine;

namespace DarkPixelGD.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private float _moveSpeed = 3f;
        [SerializeField] private float _detectRange = 8f;

        private Rigidbody2D _rb;

        private void Awake() => _rb = GetComponent<Rigidbody2D>();

        private void FixedUpdate() // auto-freezes on pause, matching a normal (non-ignoring) hazard
        {
            if (_player == null) return;

            Vector2 toPlayer = (Vector2)_player.position - _rb.position;
            if (toPlayer.magnitude > _detectRange)
            {
                _rb.linearVelocity = Vector2.zero;
                return;
            }

            _rb.linearVelocity = toPlayer.normalized * _moveSpeed;
            float angle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle); // also serves as this enemy's aim direction
        }
    }
}