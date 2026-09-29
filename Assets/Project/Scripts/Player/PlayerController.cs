using DarkPixelGD.Systems.Combat;
using DarkPixelGD.Input;
using UnityEngine;


namespace DarkPixelGD.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Dependencies")]
        public InputReader _input;
        public PlayerDataSO _data;

        [SerializeField] private Health _health;
        [SerializeField] private PlayerShooting _shooting;
        // Movement
        private Rigidbody2D _rb;
        private Vector2 _moveInput;
        private Vector2 _currentVelocity;

        // Dash Ability
        public bool _isDashing;
        private int _dashEndFixedFrame;   // fixed-tick based
        private int _lastDashFixedFrame = int.MinValue;
        private bool _dashLocked;
        private bool _controlsInverted;

        public Vector2 _facingDirection = Vector2.right; // 2D default facing

        public float CurrentSpeed = 0f;

        public bool IsInvulnerable { get; private set; }



        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

            if (_input == null)
            {
                Debug.LogError($"[{nameof(PlayerController)}] InputReader not assigned on {gameObject.name}.", this);
                enabled = false;
                return;
            }
            if (_data == null)
            {
                Debug.LogError($"[{nameof(PlayerController)}] PlayerDataSO not assigned on {gameObject.name}.", this);
                enabled = false;
                return;
            }

        }

        private void OnEnable()
        {
            _input.DashEvent += HandleDashInput;
        }

        private void OnDisable()
        {
            _input.DashEvent -= HandleDashInput;
        }

        private void Update()
        {
            if (!_isDashing)
            {
                Vector2 rawInput = _input.MoveInput;
                if (_controlsInverted) rawInput = -rawInput;

                _moveInput = rawInput.normalized;
                
            }

            if (_shooting != null && _shooting.AimDirection.sqrMagnitude > 0.01f)
                _facingDirection = _shooting.AimDirection;
            else if (_moveInput.sqrMagnitude > 0.01f)
                _facingDirection = _moveInput;
        }

        private void FixedUpdate()
        {
            if (_isDashing)
            {
                if (Time.frameCount >= _dashEndFixedFrame)
                {
                    EndDash();
                }
                return;
            }

            HandleMovement();

            CurrentSpeed = _currentVelocity.magnitude;
        }

        private void HandleMovement()
        {
            Vector2 targetVelocity = _moveInput * _data.maxSpeed;
            float accelerationRate = (_moveInput.magnitude > 0.01f) ? _data.accelaration : _data.deceleration;
            _currentVelocity = Vector2.MoveTowards(_currentVelocity, targetVelocity, accelerationRate * Time.fixedDeltaTime);
            _rb.linearVelocity = _currentVelocity;
        }

        private void HandleDashInput()
        {
            if (_isDashing || !_data.canDash||_dashLocked) return;
            int cooldownFrames = Mathf.CeilToInt(_data.dashCooldown / Time.fixedDeltaTime);
            if (Time.frameCount < _lastDashFixedFrame + cooldownFrames) return;

            StartDash();
        }

        private void StartDash()
        {
            _isDashing = true;
            _lastDashFixedFrame = Time.frameCount;

            int durationFrames = Mathf.CeilToInt(_data.dashDuration / Time.fixedDeltaTime);
            _dashEndFixedFrame = Time.frameCount + durationFrames;

            Vector2 dashDirection = _moveInput.sqrMagnitude > 0.01f ? _moveInput : _facingDirection;
            _rb.linearVelocity = dashDirection * _data.dashBurstSpeed;
        }

        private void EndDash()
        {
            _isDashing = false;

            _currentVelocity = _rb.linearVelocity;
        }

        public void SetDashLocked(bool locked) 
        {
            _dashLocked = locked;
        }
        public void SetControlsInverted(bool inverted)
        {
             _controlsInverted = inverted;
        }

        public void SetInvulnerable(bool value)
        {

            _health.SetInvulnerable(value);
        }
        public void Teleport(Vector3 position)
        {
            _rb.linearVelocity = Vector2.zero;
            transform.position = position;
        }
    }
}