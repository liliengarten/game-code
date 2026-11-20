using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameVolumeSettings", menuName = "Scriptable Objects/GameVolumeSettings")]
public class GameVolumeSettings : ScriptableObject {
    private const float MAX_VOLUME = 0f;

    public float MusicVolume { get; private set; }
    public float SfxVolume { get; private set; }

    public EventHandler OnMusicVolumeChanged;
    public EventHandler OnSfxVolumeChanged;
    public EventHandler OnVolumeSettingsLoaded;

    public void LoadFromUserPreferences() {
        float playerPrefsMusicVolume = PlayerPrefs.GetFloat("MusicVolume", MAX_VOLUME);
        float playerPrefsSfxVolume = PlayerPrefs.GetFloat("SfxVolume", MAX_VOLUME);

        if (playerPrefsMusicVolume != MusicVolume) {
            MusicVolume = playerPrefsMusicVolume;
        }

        if (playerPrefsSfxVolume != SfxVolume) {
            SfxVolume = playerPrefsSfxVolume;
        }
        
        OnMusicVolumeChanged?.Invoke(this, System.EventArgs.Empty);
        OnSfxVolumeChanged?.Invoke(this, System.EventArgs.Empty);
        OnVolumeSettingsLoaded?.Invoke(this, System.EventArgs.Empty);
    }

    public void SaveSettingsToUserPreferences() {
        if (PlayerPrefs.GetFloat("MusicVolume") != MusicVolume) {
            PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        }

        if (PlayerPrefs.GetFloat("SfxVolume") != SfxVolume) {
            PlayerPrefs.SetFloat("SfxVolume", SfxVolume);
        }

        PlayerPrefs.Save();
    }

    public void ChangeMusicVolume(float value) {
        MusicVolume = value;
        OnMusicVolumeChanged?.Invoke(this, System.EventArgs.Empty);
        SaveSettingsToUserPreferences();
    }

    public void ChangeSfxVolume(float value) {
        SfxVolume = value;
        OnSfxVolumeChanged?.Invoke(this, System.EventArgs.Empty);
        SaveSettingsToUserPreferences();
    }
}