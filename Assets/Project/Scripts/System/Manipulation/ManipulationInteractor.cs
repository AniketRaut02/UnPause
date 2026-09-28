using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace DarkPixelGD.Systems.Manipulation
{
    public class ManipulationInteractor : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private LayerMask _movableLayerMask;

        private MovableObject _selected;

        private void Start()
        {
            PauseService.Instance.OnPauseChanged += HandlePauseChanged;
        }
        private void OnDisable()
        {
            if (PauseService.Instance != null)
                PauseService.Instance.OnPauseChanged -= HandlePauseChanged;
        }

        private void HandlePauseChanged(bool isPaused)
        {
            foreach (var movable in MovableObjectRegistry.All)
                movable.SetHighlighted(isPaused);

            if (!isPaused) Deselect();
        }

        private void Update()
        {
            if (PauseService.Instance == null || !PauseService.Instance.IsPaused) return;

            if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame) return;

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

            Vector2 worldPos = _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            var hit = Physics2D.OverlapPoint(worldPos, _movableLayerMask);

            if (hit != null && hit.TryGetComponent(out MovableObject movable))
                Select(movable);
            else
                Deselect();
        }

        private void Select(MovableObject movable)
        {
            if (_selected != null) _selected.SetSelected(false);
            _selected = movable;
            _selected.SetSelected(true);
            ManipulationPromptUI.Instance.ShowFor(_selected);
        }

        private void Deselect()
        {
            if (_selected != null) _selected.SetSelected(false);
            _selected = null;
            ManipulationPromptUI.Instance.Hide();
        }
    }
}