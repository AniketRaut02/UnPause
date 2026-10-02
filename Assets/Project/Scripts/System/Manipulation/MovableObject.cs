using DarkPixelGD.Audio;
using DarkPixelGD.Juice;
using UnityEngine;

namespace DarkPixelGD.Systems.Manipulation
{
    public interface IManipulable
    {
        bool CanRotate { get; }
        bool CanMove { get; }
        void Rotate(int direction);
        void Move(int direction);
        void SetHighlighted(bool on);
        void SetSelected(bool on);
    }

    public class MovableObject : MonoBehaviour, IManipulable
    {
        [Header("Rotation Constraints")]
        [SerializeField] private bool _canRotate = true;
        [SerializeField] private float _rotationStep = 90f;
        [SerializeField] private float _minAngle = -90f;
        [SerializeField] private float _maxAngle = 90f;

        [Header("Move Constraints")]
        [SerializeField] private bool _canMove = true;
        [SerializeField] private Vector2 _moveAxis = Vector2.right;
        [SerializeField] private float _moveStep = 1f;
        [SerializeField] private float _minOffset = -2f;
        [SerializeField] private float _maxOffset = 2f;

        [Header("Visuals")]
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Color _highlightColor = Color.yellow;
        [SerializeField] private Color _selectedColor = Color.green;

        [Header("Audio")]
        [SerializeField] private AudioEventChannelSO _audioChannel;
        [SerializeField] private SoundDataSO _rotateSoundSO;
        [SerializeField] private SoundDataSO _moveSoundSO;
        [SerializeField] private SoundDataSO _selectedSoundSO;

        [Header("Juice Refrences")]
        [SerializeField] private JuiceEventChannelSO _juiceChannel;
        [SerializeField] private JuiceProfileSO _rotateJuiceProfile;
        [SerializeField] private JuiceProfileSO _moveJuiceProfile;

        private Color _baseColor;
        private float _currentAngle;
        private float _currentOffset;
        private Vector3 _startPosition;

        public bool CanRotate => _canRotate;
        public bool CanMove => _canMove;

        private bool _isHighlighted;
        private bool _isSelected;

        private void Awake()
        {
            MovableObjectRegistry.Register(this);
            _startPosition = transform.position;
            if (_renderer != null) _baseColor = _renderer.color;
        }

        private void OnDestroy() => MovableObjectRegistry.Unregister(this);

        public void Rotate(int direction)
        {
            if (!_canRotate) return;
            _currentAngle = Mathf.Clamp(_currentAngle + direction * _rotationStep, _minAngle, _maxAngle);
            transform.rotation = Quaternion.Euler(0, 0, _currentAngle);
            if (_audioChannel != null && _rotateSoundSO != null) { _audioChannel.RaisePlaySound(_rotateSoundSO, transform.position); }
            if (_juiceChannel != null && _rotateJuiceProfile != null) _juiceChannel.RaiseJuice(_rotateJuiceProfile, transform.position);
        }

        public void Move(int direction)
        {
            if (!_canMove) return;
            _currentOffset = Mathf.Clamp(_currentOffset + direction * _moveStep, _minOffset, _maxOffset);
            transform.position = _startPosition + (Vector3)(_moveAxis.normalized * _currentOffset);
            if (_audioChannel != null && _moveSoundSO != null) { _audioChannel.RaisePlaySound(_moveSoundSO, transform.position); }
            if (_juiceChannel != null && _moveJuiceProfile != null) _juiceChannel.RaiseJuice(_moveJuiceProfile, transform.position);
        }

        public void SetHighlighted(bool on)
        {
            _isHighlighted = on;
            RefreshColor();
        }

        public void SetSelected(bool on)
        {
            _isSelected = on;
            RefreshColor();
            if (_audioChannel != null && _selectedSoundSO != null) { _audioChannel.RaisePlaySound(_selectedSoundSO, transform.position); }
        }

        private void RefreshColor()
        {
            if (_renderer == null) return;
            _renderer.color = _isSelected ? _selectedColor : (_isHighlighted ? _highlightColor : _baseColor);
        }
    }
}