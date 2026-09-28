using UnityEngine;
using UnityEngine.UI;


namespace DarkPixelGD.Player
{
    [CreateAssetMenu(fileName="PlayerData", menuName ="Player/PlayerData")]
    public class PlayerDataSO : ScriptableObject
    {
        [Header("Movement Data")]
        public float maxSpeed = 8f;
        public float accelaration = 60f;
        public float deceleration = 60f;

        [Header("Dash Ability Data")]
        public bool canDash = true;
        public float dashBurstSpeed = 25f;
        public float dashDuration = 0.15f;
        public float dashCooldown = 1.0f;
    }
}
