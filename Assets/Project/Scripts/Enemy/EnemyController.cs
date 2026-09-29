using DarkPixelGD.Systems.Combat;
using UnityEngine;

namespace DarkPixelGD.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Transform _player;
        [SerializeField] private Health _health;
        [SerializeField] private EnemyDataSO _data;
        [Header("Aiming")]
        [SerializeField] private Transform _aimPivot;
        [SerializeField] private SpriteRenderer _aimPivotRenderer;


        private Rigidbody2D _rb;
        private Vector2 _currentVelocity;
        private Vector2 _facingDirection = Vector2.right;

        public Vector2 FacingDirection => _facingDirection;
        public float CurrentSpeed => _currentVelocity.magnitude;

        public Health Health => _health;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

            if (_data == null)
            {
                Debug.LogError($"[{nameof(EnemyController)}] EnemyDataSO not assigned on {gameObject.name}.", this);
                enabled = false;
            }
        }

        private void FixedUpdate()
        {
            if (_player == null) return;

            Vector2 toPlayer = (Vector2)_player.position - _rb.position;
            bool inRange = toPlayer.magnitude <= _data.detectRange;

            Vector2 moveInput = inRange ? toPlayer.normalized : Vector2.zero;
            HandleMovement(moveInput);

            if (moveInput.sqrMagnitude > 0.01f)
                _facingDirection = moveInput;

            AimAt(toPlayer.normalized); 
        }

        private void HandleMovement(Vector2 moveInput)
        {
            Vector2 targetVelocity = moveInput * _data.maxSpeed;
            float accelerationRate = (moveInput.magnitude > 0.01f) ? _data.acceleration : _data.deceleration;
            _currentVelocity = Vector2.MoveTowards(_currentVelocity, targetVelocity, accelerationRate * Time.fixedDeltaTime);
            _rb.linearVelocity = _currentVelocity;
        }
        private void AimAt(Vector2 direction)
        {
            if (direction.sqrMagnitude < 0.0001f || _aimPivot == null) return;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _aimPivot.rotation = Quaternion.Euler(0, 0, angle);

            if (_aimPivotRenderer != null)
                _aimPivotRenderer.flipY = Mathf.Abs(angle) > 90f;
        }


    }
}