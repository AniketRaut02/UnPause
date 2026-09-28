using System;
using UnityEngine;


namespace DarkPixelGD.Interaction
{ 

    [RequireComponent(typeof(Collider2D))]
    public class PortalTrigger : MonoBehaviour
    {
        public event Action OnLevelComplete;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            Debug.Log("Portal Activated");
            OnLevelComplete?.Invoke();
        }
    }
}