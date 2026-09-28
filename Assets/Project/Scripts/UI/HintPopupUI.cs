using UnityEngine;
using TMPro;

public class HintPopupUI : MonoBehaviour
{
    public static HintPopupUI Instance { get; private set; }

    [SerializeField] private GameObject _root;
    [SerializeField] private TMP_Text _label;
    [SerializeField] private float _displayDuration = 2.5f;

    private float _hideTime;

    private void Awake()
    {
        Instance = this;
        _root.SetActive(false);
    }

    public void Show(string message)
    {
        _label.text = message;
        _root.SetActive(true);
        _hideTime = Time.unscaledTime + _displayDuration; // unscaled: safe to show around a pause/unpause moment
    }

    private void Update()
    {
        if (_root.activeSelf && Time.unscaledTime >= _hideTime)
            _root.SetActive(false);
    }
}