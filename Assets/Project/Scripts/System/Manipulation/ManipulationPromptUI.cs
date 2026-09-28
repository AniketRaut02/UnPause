using UnityEngine;

namespace DarkPixelGD.Systems.Manipulation
{
    public class ManipulationPromptUI : MonoBehaviour
    {
        public static ManipulationPromptUI Instance { get; private set; }

        [SerializeField] private GameObject _root;
        [SerializeField] private RectTransform _panel;

        private MovableObject _target;

        private void Awake() { Instance = this; Hide(); }

        public void ShowFor(MovableObject target)
        {
            _target = target;
            _root.SetActive(true);
        }

        public void Hide()
        {
            _target = null;
            _root.SetActive(false);
        }

        private void LateUpdate()
        {
            if (_target == null || !_root.activeSelf) return;
            _panel.position = Camera.main.WorldToScreenPoint(_target.transform.position);
        }

        // Rotate/Move buttons' OnClick() in the Inspector
        public void OnRotateClockwise() => _target?.Rotate(1);
        public void OnRotateCounterClockwise() => _target?.Rotate(-1);
        public void OnMovePositive() => _target?.Move(1);
        public void OnMoveNegative() => _target?.Move(-1);
    }
}