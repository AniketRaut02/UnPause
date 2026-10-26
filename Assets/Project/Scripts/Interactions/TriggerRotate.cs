using DarkPixelGD.Player;
using System.Collections;
using UnityEngine;

namespace DarkPixelGD.Systems.Environment
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggerRotate : MonoBehaviour
    {
        [Header("Target to Rotate")]
        [SerializeField] private Transform _objectToRotate; // If left empty, rotates this GameObject

        [Header("Rotation Settings")]
        [SerializeField] private float _targetAngleZ = 90f;   // The Z-axis angle to rotate towards
        [SerializeField] private float _rotationSpeed = 180f; // Degrees per second
        [SerializeField] private bool _triggerOnce = true;

        private bool _hasTriggered;

        private void Awake()
        {
            if (_objectToRotate == null)
            {
                _objectToRotate = transform;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (_triggerOnce && _hasTriggered) return;

            _hasTriggered = true;
            if (other.GetComponent<PlayerSoul>().CurrentSoul >= 80)
            {
                RotateRoutine();
            }
          
        }

        private void RotateRoutine()
        {
            Quaternion startRotation = _objectToRotate.rotation;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, _targetAngleZ);

            float angleDifference = Quaternion.Angle(startRotation, targetRotation);
            float elapsed = 0f;

            // Calculate duration based on speed and distance
            float duration = angleDifference / _rotationSpeed;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                // Smoothly interpolate rotation
                _objectToRotate.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            }

            _objectToRotate.rotation = targetRotation; // Ensure it hits the exact target
        }
    }
}