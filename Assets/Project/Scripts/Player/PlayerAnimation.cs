using UnityEngine;

namespace DarkPixelGD.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : MonoBehaviour
    {
        [Header("Refrences")]
        [SerializeField] private PlayerController _controller;
        private Animator _animator;


        private readonly int _animMoveX = Animator.StringToHash("MoveX");
        private readonly int _animMoveY = Animator.StringToHash("MoveY");
        private readonly int _animSpeed = Animator.StringToHash("Speed");
        private readonly int _animDash = Animator.StringToHash("IsDashing");

        private readonly int _animHurt = Animator.StringToHash("Hurt");

        private readonly int _animDeath = Animator.StringToHash("Death");


        private void Awake()
        {
            _animator = GetComponent<Animator>();
            if (_animator == null)
            {
                Debug.LogError($"[{nameof(PlayerAnimation)}] Animator not assigned on {gameObject.name}.", this);
                enabled = false;
                return;
            }
            if (_controller == null)
            {
                Debug.LogError($"[{nameof(PlayerAnimation)}] PlayerController not assigned on {gameObject.name}.", this);
                enabled = false;
                return;
            }

        }

        private void Start()
        {
            if (_controller != null && _controller.Health != null)
            {
                _controller.Health.OnDamageTaken += HandleDamageTaken;
                _controller.Health.OnDeath += HandleDeath;
            }

        }
        void Update()
        {
            _animator.SetFloat(_animMoveX, _controller._facingDirection.x);
            _animator.SetFloat(_animMoveY, _controller._facingDirection.y);
            _animator.SetFloat(_animSpeed, _controller.CurrentSpeed);
            _animator.SetBool(_animDash, _controller._isDashing);
        }

        private void OnDestroy()
        {
            if (_controller != null && _controller.Health != null)
            {
                _controller.Health.OnDamageTaken -= HandleDamageTaken;
                _controller.Health.OnDeath -= HandleDeath;
            }
               
        }
        private void HandleDamageTaken(int amount)
        {
            _animator.SetTrigger(_animHurt);
        }

        private void HandleDeath() => _animator.SetTrigger(_animDeath);
    }
}