using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class HoverImageToggle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image buttonImage;

    void Awake()
    {
        // Grab the Image component attached to the Button
        buttonImage = GetComponent<Image>();

        // Start with the image disabled (optional, change to true if you want it visible by default)
        buttonImage.enabled = false;
    }

    // Triggered when the mouse enters the UI element bounds
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.enabled = true;
    }

    // Triggered when the mouse leaves the UI element bounds
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.enabled = false;
    }
}