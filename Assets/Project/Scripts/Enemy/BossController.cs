using System.Collections;
using UnityEngine;
using DarkPixelGD.Combat;
using DarkPixelGD.Systems.Combat;

namespace DarkPixelGD.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BossController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Transform _player;
        [SerializeField] private Health _health;
        [SerializeField] private EnemyDataSO _data;
        [SerializeField] private EnemyShooter _standardShooter;

        [Header("Aiming References")]
        [SerializeField] private Transform _aimPivot;
        [SerializeField] private SpriteRenderer _aimPivotRenderer;

        [Header("Boss Special Attacks")]
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _muzzle;
        [SerializeField] private float _specialAttackInterval = 4.0f;

        [Header("Attack A: Spread Attack Settings")]
        [SerializeField] private int _bulletSpreadCount = 5;
        [SerializeField] private float _spreadAngle = 15f;

        [Header("Attack B: Radial Ring Attack Settings")]
        [SerializeField] private int _ringBulletCount = 12;

        [Header("Attack C: Linear Bullet Stream Settings")]
        [SerializeField] private int _streamBulletCount = 8;
        [SerializeField] private float _streamFireRate = 0.08f;

        private Rigidbody2D _rb;
        private Vector2 _currentVelocity;
        private Vector2 _facingDirection = Vector2.right;

        public Vector2 FacingDirection => _facingDirection;
        public float CurrentSpeed => _currentVelocity.magnitude;
        public Health Health => _health;

        private bool _isActivated = false; // Boss stays dormant until triggered
        private bool _isPerformingSpecial;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

            if (_health == null) _health = GetComponent<Health>();
            if (_standardShooter == null) _standardShooter = GetComponent<EnemyShooter>();

            // Ensure standard shooter is disabled until the boss activates
            if (_standardShooter != null) _standardShooter.enabled = false;

            if (_player == null)
            {
                GameObject p = GameObject.FindGameObjectWithTag("Player");
                if (p != null) _player = p.transform;
            }
        }

        // Called externally when the player steps into the arena trigger zone
        public void ActivateBoss()
        {
            if (_isActivated) return;
            _isActivated = true;

            // Enable standard shooter and kick off pattern routine
            if (_standardShooter != null) _standardShooter.enabled = true;
            StartCoroutine(BossPatternRoutine());

            //HintPopupUI.Instance?.Show("Boss Encountered!");
        }

        private void FixedUpdate()
        {
            // Do not move or track player if not yet activated
            if (!_isActivated || _player == null) return;

            Vector2 toPlayer = (Vector2)_player.position - _rb.position;
            float distance = toPlayer.magnitude;

            if (_isPerformingSpecial) return;

            // 1. Standard Movement
            bool inRange = distance <= _data.detectRange;
            bool isHolding = inRange && distance <= _data.stoppingDistance;

            Vector2 moveInput = (inRange && !isHolding) ? toPlayer.normalized : Vector2.zero;

            Vector2 targetVelocity = moveInput * _data.maxSpeed;
            float accelerationRate = (moveInput.magnitude > 0.01f) ? _data.acceleration : _data.deceleration;
            _currentVelocity = Vector2.MoveTowards(_currentVelocity, targetVelocity, accelerationRate * Time.fixedDeltaTime);
            _rb.linearVelocity = _currentVelocity;

            // 2. Facing & Aiming Direction
            if (inRange && distance > 0.01f)
                _facingDirection = toPlayer.normalized;

            AimAt(toPlayer.normalized);
        }

        private IEnumerator BossPatternRoutine()
        {
            yield return new WaitForSeconds(1.5f); // Brief pause after activation before first special

            while (_isActivated)
            {
                yield return new WaitForSeconds(_specialAttackInterval);

                if (_player == null) yield break;

                int attackChoice = Random.Range(0, 3);

                if (attackChoice == 0)
                {
                    yield return StartCoroutine(ExecuteSpreadAttack());
                }
                else if (attackChoice == 1)
                {
                    yield return StartCoroutine(ExecuteRingBurstAttack());
                }
                else
                {
                    yield return StartCoroutine(ExecuteLinearStreamAttack());
                }
            }
        }

        private IEnumerator ExecuteSpreadAttack()
        {
            _isPerformingSpecial = true;
            _rb.linearVelocity = Vector2.zero;

            if (_standardShooter != null) _standardShooter.enabled = false;

            Vector2 toPlayer = ((Vector2)_player.position - (Vector2)transform.position).normalized;
            float baseAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
            float startAngle = baseAngle - (_spreadAngle * (_bulletSpreadCount / 2));

            for (int i = 0; i < _bulletSpreadCount; i++)
            {
                float currentAngle = startAngle + (i * _spreadAngle);
                Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);

                GameObject bullet = Instantiate(_bulletPrefab, _muzzle.position, rotation);
                bullet.GetComponent<Projectile>().Initialize(15, "Player");
            }

            yield return new WaitForSeconds(0.6f);

            if (_isActivated && _standardShooter != null) _standardShooter.enabled = true;
            _isPerformingSpecial = false;
        }

        private IEnumerator ExecuteRingBurstAttack()
        {
            _isPerformingSpecial = true;
            _rb.linearVelocity = Vector2.zero;

            if (_standardShooter != null) _standardShooter.enabled = false;

            yield return new WaitForSeconds(0.4f);

            float angleStep = 360f / _ringBulletCount;

            for (int i = 0; i < _ringBulletCount; i++)
            {
                float currentAngle = i * angleStep;
                Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);

                GameObject bullet = Instantiate(_bulletPrefab, transform.position, rotation);
                bullet.GetComponent<Projectile>().Initialize(12, "Player");
            }

            yield return new WaitForSeconds(0.6f);

            if (_isActivated && _standardShooter != null) _standardShooter.enabled = true;
            _isPerformingSpecial = false;
        }

        private IEnumerator ExecuteLinearStreamAttack()
        {
            _isPerformingSpecial = true;
            _rb.linearVelocity = Vector2.zero;

            if (_standardShooter != null) _standardShooter.enabled = false;

            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < _streamBulletCount; i++)
            {
                if (_player == null) break;

                Vector2 toPlayer = ((Vector2)_player.position - (Vector2)_muzzle.position).normalized;
                float angle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(0, 0, angle);

                GameObject bullet = Instantiate(_bulletPrefab, _muzzle.position, rotation);
                bullet.GetComponent<Projectile>().Initialize(10, "Player");

                yield return new WaitForSeconds(_streamFireRate);
            }

            yield return new WaitForSeconds(0.5f);

            if (_isActivated && _standardShooter != null) _standardShooter.enabled = true;
            _isPerformingSpecial = false;
        }

        private void AimAt(Vector2 direction)
        {
            if (direction.sqrMagnitude < 0.0001f || _aimPivot == null) return;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _aimPivot.rotation = Quaternion.Euler(0, 0, angle);

            if (_aimPivotRenderer != null)
                _aimPivotRenderer.flipY = Mathf.Abs(angle) > 0f;
        }
    }
}