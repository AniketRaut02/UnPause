using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/GameSettingsSO")]
public class GameSettingsSO : ScriptableObject
{
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float narratorVolume = 1f;
    public float mouseSensitivity = 1f;
    public int resolutionIndex = 0;

    public event Action OnSettingsChanged;
    public void NotifyChanged() => OnSettingsChanged?.Invoke();

    private const string MusicKey = "settings_music", SfxKey = "settings_sfx",
                          SensKey = "settings_sens", ResKey = "settings_res";
    private const string NarratorKey = "settings_narrator";

    public void Load()
    {
        musicVolume = PlayerPrefs.GetFloat(MusicKey, 1f);
        sfxVolume = PlayerPrefs.GetFloat(SfxKey, 1f);
        mouseSensitivity = PlayerPrefs.GetFloat(SensKey, 1f);
        resolutionIndex = PlayerPrefs.GetInt(ResKey, 0);
        narratorVolume = PlayerPrefs.GetFloat(NarratorKey, 1f);
    }

    public void Save()
    {
        PlayerPrefs.SetFloat(MusicKey, musicVolume);
        PlayerPrefs.SetFloat(SfxKey, sfxVolume);
        PlayerPrefs.SetFloat(SensKey, mouseSensitivity);
        PlayerPrefs.SetInt(ResKey, resolutionIndex);
        PlayerPrefs.SetFloat(NarratorKey, narratorVolume);
        PlayerPrefs.Save();
    }
}