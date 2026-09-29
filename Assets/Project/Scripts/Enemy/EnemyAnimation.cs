using UnityEngine;

namespace DarkPixelGD.Enemy
{
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private EnemyController _controller;
        private Animator _animator;

        private readonly int _animMoveX = Animator.StringToHash("MoveX");
        private readonly int _animMoveY = Animator.StringToHash("MoveY");
        private readonly int _animSpeed = Animator.StringToHash("Speed");
        private readonly int _animHurt = Animator.StringToHash("Hurt");

        private readonly int _animDeath = Animator.StringToHash("Death");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            if (_animator == null)
            {
                Debug.LogError($"[{nameof(EnemyAnimation)}] Animator not assigned on {gameObject.name}.", this);
                enabled = false;
                return;
            }
            if (_controller == null)
            {
                Debug.LogError($"[{nameof(EnemyAnimation)}] EnemyController not assigned on {gameObject.name}.", this);
                enabled = false;
                return;
            }
        }

        private void Start()
        {
            _controller.Health.OnDamageTaken += HandleDamageTaken;
            _controller.Health.OnDeath += HandleDeath;
        }

        private void Update()
        {
            _animator.SetFloat(_animMoveX, _controller.FacingDirection.x);
            _animator.SetFloat(_animMoveY, _controller.FacingDirection.y);
            _animator.SetFloat(_animSpeed, _controller.CurrentSpeed);
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

        private void HandleDeath()
        {
            _animator.SetTrigger(_animDeath);
        }
    }
}