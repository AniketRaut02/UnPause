using UnityEngine;
using UnityEngine.EventSystems;

namespace DarkPixelGD.UI
{
    public class UIButtonSFX : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        [SerializeField] private Audio.AudioEventChannelSO _channel;
        [SerializeField] private Audio.SoundDataSO _hoverSound;
        [SerializeField] private Audio.SoundDataSO _clickSound;

        public void OnPointerEnter(PointerEventData eventData) => _channel.RaisePlaySound(_hoverSound, Vector3.zero);
        public void OnPointerClick(PointerEventData eventData) => _channel.RaisePlaySound(_clickSound, Vector3.zero);
    }
}