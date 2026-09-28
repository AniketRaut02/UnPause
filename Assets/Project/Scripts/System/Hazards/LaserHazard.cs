using UnityEngine;

namespace DarkPixelGD.Systems.Hazard
{
    [RequireComponent(typeof(LineRenderer))]
    public class LaserHazard : HazardBase
    {
        [SerializeField] private Transform _emitter;
        [SerializeField] private float _maxDistance = 20f;
        [SerializeField] private LayerMask _hitMask; // must include Player AND Movable/Wall layers
        [SerializeField] private float _rotationSpeed = 0f; // degrees/sec; 0 = static beam


        private LineRenderer _line;

        private void Awake() => _line = GetComponent<LineRenderer>();


        private void FixedUpdate()
        {
            if (_ignoresPause) return; // this instance is handled in Update() instead
            TickLaser(Time.fixedDeltaTime);
        }

        private void Update()
        {
            if (!_ignoresPause) return; // normal lasers are handled in FixedUpdate instead
            TickLaser(Time.unscaledDeltaTime);
        }

        private void TickLaser(float deltaTime)
        {
            if (_rotationSpeed != 0f)
                _emitter.Rotate(0, 0, _rotationSpeed * deltaTime);

            Vector2 origin = _emitter.position;
            Vector2 direction = _emitter.right;
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, _maxDistance, _hitMask);
            Vector2 endPoint = hit.collider != null ? hit.point : origin + direction * _maxDistance;

            _line.SetPosition(0, origin);
            _line.SetPosition(1, endPoint);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
                KillPlayer(hit.collider.gameObject);
        }
    }
}