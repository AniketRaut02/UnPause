using UnityEngine;

namespace DarkPixelGD.Juice
{
    [CreateAssetMenu(fileName = "JuiceProfile", menuName = "Juice/JuiceProfileSO")]
    public class JuiceProfileSO : ScriptableObject
    {
        [Header("Camera Shake")]
        public Vector3 impulseVelocity = new Vector3(0.3f, 0.3f, 0f);
        public float shakeForce = 1f;

        [Header("Light Flash")]
        public bool useLightFlash = true; // per-profile opt-out, independent of the global settings toggle
        public Color lightColor = Color.white;

        public Sprite lightSprite;
        public float lightIntensity = 3f;
        public float lightRadius = 2f;
        public float lightDuration = 0.08f;
    }
}