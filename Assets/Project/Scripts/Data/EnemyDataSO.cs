using UnityEngine;

namespace DarkPixelGD.Enemy
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyDataSO")]
    public class EnemyDataSO : ScriptableObject
    {
        public float maxSpeed = 3f;
        public float acceleration = 20f;
        public float deceleration = 25f;
        public float detectRange = 8f;

        [Header("Hold Position")]
        public float stoppingDistance = 3f;  
        public float resumeBuffer = 0.5f;    

        private void OnValidate()
        {
            stoppingDistance = Mathf.Clamp(stoppingDistance, 0f, detectRange);
            resumeBuffer = Mathf.Max(0f, resumeBuffer);
        }
    }
}