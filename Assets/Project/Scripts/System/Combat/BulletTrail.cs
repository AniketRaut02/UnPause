using System.Collections.Generic;
using UnityEngine;

namespace DarkPixelGD.Systems.Combat
{
    [RequireComponent(typeof(LineRenderer))]
    public class BulletTrail : MonoBehaviour
    {
        [SerializeField] private float _trailDuration = 0.15f;
        [SerializeField] private float _startWidth = 0.08f; // at the bullet (head)
        [SerializeField] private float _endWidth = 0f;       // at the tail

        private LineRenderer _line;
        private readonly List<(Vector3 pos, float time)> _points = new();

        private void Awake()
        {
            _line = GetComponent<LineRenderer>();
            _line.useWorldSpace = true;
            _line.positionCount = 0;

            // Alpha: index 0 (oldest/tail) = transparent, last index (newest/head) = opaque.
            // Set once — the position list's ordering never changes, so the gradient doesn't need recalculating per frame.
            var gradient = new Gradient();
            gradient.SetKeys(
                new[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
                new[] { new GradientAlphaKey(0f, 0f), new GradientAlphaKey(1f, 1f) }
            );
            _line.colorGradient = gradient;
            _line.widthCurve = new AnimationCurve(
                new Keyframe(0f, _endWidth),
                new Keyframe(1f, _startWidth)
            );
        }

        private void FixedUpdate() // same tick as Projectile's own movement — auto-freezes on pause
        {
            _points.Add((transform.position, Time.time));

            while (_points.Count > 0 && Time.time - _points[0].time > _trailDuration)
                _points.RemoveAt(0);

            _line.positionCount = _points.Count;
            for (int i = 0; i < _points.Count; i++)
                _line.SetPosition(i, _points[i].pos);
        }
    }
}