using System.Collections;
using UnityEngine;
using UnityEngine.Android;

namespace DarkPixelGD.Combat
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MuzzleFlash : MonoBehaviour
    {
        [SerializeField] private float _flashDuration = 0.05f;
        [SerializeField] private Sprite[] muzzleSprites;
        private SpriteRenderer _renderer;
        private Coroutine _routine;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.enabled = false;
        }

        public void Flash()
        {
            if (_routine != null) StopCoroutine(_routine);
            _routine = StartCoroutine(FlashRoutine());
        }

        private IEnumerator FlashRoutine()
        {
            _renderer.sprite = muzzleSprites[ChooseRandomMuzzleSprite()];
            _renderer.enabled = true;
            yield return new WaitForSeconds(_flashDuration); // scaled: won't get stuck visible if you fire right as pause hits
            _renderer.enabled = false;
        }

        int ChooseRandomMuzzleSprite()
        {
            int totalSprites = muzzleSprites.Length;
            if (totalSprites == 0) return 0;
            return Random.Range(0, totalSprites); 
        }
    }
}