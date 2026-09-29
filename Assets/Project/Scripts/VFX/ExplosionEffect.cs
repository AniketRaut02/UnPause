using UnityEngine;

namespace DarkPixelGD.VFX
{
    [RequireComponent(typeof(Animator))]
    public class ExplosionEffect : MonoBehaviour
    {
      //  private Animator _animator;
     //   private static readonly int _playHash = Animator.StringToHash("Play");

       // private void Awake() => _animator = GetComponent<Animator>();
        //private void Start() => _animator.SetTrigger(_playHash); // plays immediately on spawn, no external trigger call needed

        // Wire to an Animation Event on the clip's last frame — same cleanup pattern as DamageNumberUI
        public void DestroySelf() => Destroy(gameObject);
    }
}