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

        private bool _isHolding;
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
            float distance = toPlayer.magnitude;
            bool inRange = distance <= _data.detectRange;

            UpdateHoldState(distance, inRange);

            Vector2 moveInput = (inRange && !_isHolding) ? toPlayer.normalized : Vector2.zero;
            HandleMovement(moveInput); // zero input decelerates smoothly through the existing accel/decel path

            if (inRange && distance > 0.01f)
                _facingDirection = toPlayer.normalized; // face the player even while holding position

            AimAt(toPlayer.normalized);
        }

        private void UpdateHoldState(float distance, bool inRange)
        {
            if (!inRange) { _isHolding = false; return; }

            if (!_isHolding && distance <= _data.stoppingDistance)
                _isHolding = true;
            else if (_isHolding && distance > _data.stoppingDistance + _data.resumeBuffer)
                _isHolding = false;
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
            bool aimingLeft = Mathf.Abs(angle) > 90f;
            float scaleY = aimingLeft ? -1 : 1;
            _aimPivotRenderer.gameObject.transform.localScale =
               new Vector3(_aimPivotRenderer.gameObject.transform.localScale.x, scaleY, _aimPivotRenderer.gameObject.transform.localScale.z);
/*            if (_aimPivotRenderer != null)
                _aimPivotRenderer.flipY = Mathf.Abs(angle) > 90f;*/
        }


    }
}