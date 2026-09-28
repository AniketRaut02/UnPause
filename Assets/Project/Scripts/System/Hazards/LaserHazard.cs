using UnityEngine;

namespace DarkPixelGD.Systems.Hazard
{
    [RequireComponent(typeof(LineRenderer))]
    public class LaserHazard : HazardBase
    {
        [SerializeField] private Transform _emitter;
        [SerializeField] private float _maxDistance = 20f;
        [SerializeField] private LayerMask _hitMask; // must include Player AND Movable/Wall layers

        private LineRenderer _line;

        private void Awake() => _line = GetComponent<LineRenderer>();

        private void FixedUpdate()
        {
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