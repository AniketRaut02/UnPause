using System;
using UnityEngine;

namespace DarkPixelGD.Juice
{
    [CreateAssetMenu(fileName = "JuiceEventChannel", menuName = "Juice/JuiceEventChannelSO")]
    public class JuiceEventChannelSO : ScriptableObject
    {
        public event Action<JuiceProfileSO, Vector3> OnJuiceRequested;
        public void RaiseJuice(JuiceProfileSO profile, Vector3 position)
        {
            if (profile == null) return;
            OnJuiceRequested?.Invoke(profile, position);
        }
    }
}