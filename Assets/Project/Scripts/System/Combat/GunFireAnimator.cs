using UnityEngine;

namespace DarkPixelGD.Systems.Combat
{
    public class GunFireAnimator : MonoBehaviour
    {
        private Animator _animator;
        private const string _fireStateName = "GunFire"; // must match the state's name in the Controller

        private void Awake() => _animator = GetComponent<Animator>();

        public void PlayFire() => _animator.Play(_fireStateName, 0, 0f);
    }
}