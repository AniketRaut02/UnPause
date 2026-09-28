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

        // Update is called once per frame
        void Update()
        {
            _animator.SetFloat(_animMoveX, _controller._facingDirection.x);
            _animator.SetFloat(_animMoveY, _controller._facingDirection.y);
            _animator.SetFloat(_animSpeed, _controller.CurrentSpeed);
            _animator.SetBool(_animDash, _controller._isDashing);
        }
    }
}