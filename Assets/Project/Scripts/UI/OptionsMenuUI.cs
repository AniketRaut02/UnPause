using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenuUI : MonoBehaviour
{
    [SerializeField] private GameSettingsSO _settings;
    [SerializeField] private GameObject _root;
    [SerializeField] private AudioMixer _audioMixer; 
    [SerializeField] private Slider _musicSlider, _sfxSlider, _sensitivitySlider, _narratorSlider;
    [SerializeField] private TMP_Dropdown _resolutionDropdown;

    private Resolution[] _resolutions;

    private void OnEnable()
    {
        _settings.Load();
        _musicSlider.SetValueWithoutNotify(_settings.musicVolume);
        _sfxSlider.SetValueWithoutNotify(_settings.sfxVolume);
        _sensitivitySlider.SetValueWithoutNotify(_settings.mouseSensitivity);
        _narratorSlider.SetValueWithoutNotify(_settings.narratorVolume);
        PopulateResolutions();
        ApplyAudio();
    }

    private void PopulateResolutions()
    {
        _resolutions = Screen.resolutions;
        var options = new List<string>();
        foreach (var r in _resolutions) options.Add($"{r.width}x{r.height}");
        _resolutionDropdown.ClearOptions();
        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.SetValueWithoutNotify(_settings.resolutionIndex);

        var currentRes = _resolutions[_settings.resolutionIndex];
        Screen.SetResolution(currentRes.width, currentRes.height, Screen.fullScreen);
    }

    // Hook to each control's OnValueChanged in the Inspector
    public void OnMusicChanged() { _settings.musicVolume = _musicSlider.value; ApplyAudio(); }
    public void OnSfxChanged() { _settings.sfxVolume = _sfxSlider.value; ApplyAudio(); }
    public void OnSensitivityChanged() { _settings.mouseSensitivity = _sensitivitySlider.value; _settings.NotifyChanged(); }

    public void OnNarratorChanged() { _settings.narratorVolume = _narratorSlider.value; ApplyAudio(); }

    public void OnResolutionChanged()
    {
        _settings.resolutionIndex = _resolutionDropdown.value;
        var r = _resolutions[_resolutionDropdown.value];
        Screen.SetResolution(r.width, r.height, Screen.fullScreen);
    }

    private void ApplyAudio()
    {
        // Sliders are linear 0-1; AudioMixer expects logarithmic dB.
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Max(_settings.musicVolume, 0.0001f)) * 20f);
        _audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Max(_settings.sfxVolume, 0.0001f)) * 20f);
        _audioMixer.SetFloat("NarratorVolume", Mathf.Log10(Mathf.Max(_settings.narratorVolume, 0.0001f)) * 20f);
    }

    public void OnBackPressed()
    {
        _settings.Save();
        _settings.NotifyChanged();
        _root.gameObject.SetActive(false);
    }
}