using TMPro;
using UnityEngine;

namespace DarkPixelGD.Systems.Combat
{
    [RequireComponent(typeof(Animator))]
    public class DamageNumberUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;
        private Animator _animator;
       // private static readonly int _playHash = Animator.StringToHash("Play");

        private void Awake() => _animator = GetComponent<Animator>();

        public void Show(int amount)
        {
            _label.text = amount.ToString();
            //_animator.SetTrigger(_playHash);
        }

        // Wire this to an Animation Event at the end of the clip — see wire-up below
        public void DestroySelf() => Destroy(gameObject);
    }
}