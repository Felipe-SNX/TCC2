using UnityEngine;
using System;

public class AudioSettingsCore : MonoBehaviour
{
    public static AudioSettingsCore Instance { get; private set; }

    public event Action OnVolumeChanged;

    private const string KEY_VOLUME_MASTER = "VolumeMaster";
    private const string KEY_VOLUME_MUSICA = "VolumeMusica";
    private const string KEY_VOLUME_SFX = "VolumeSFX";

    public float MasterVolume { get; private set; }
    public float MusicVolume { get; private set; }
    public float SfxVolume { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadPreferences();
    }

    private void LoadPreferences()
    {
        MasterVolume = PlayerPrefs.GetFloat(KEY_VOLUME_MASTER, 50f) / 100f;
        MusicVolume = PlayerPrefs.GetFloat(KEY_VOLUME_MUSICA, 50f) / 100f;
        SfxVolume = PlayerPrefs.GetFloat(KEY_VOLUME_SFX, 80f) / 100f;
    }

    public void SetMasterVolume(float volume)
    {
        MasterVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(KEY_VOLUME_MASTER, MasterVolume * 100f);
        PlayerPrefs.Save();
        OnVolumeChanged?.Invoke();
    }

    public void SetMusicVolume(float volume)
    {
        MusicVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(KEY_VOLUME_MUSICA, MusicVolume * 100f);
        PlayerPrefs.Save();
        OnVolumeChanged?.Invoke();
    }

    public void SetSFXVolume(float volume)
    {
        SfxVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(KEY_VOLUME_SFX, SfxVolume * 100f);
        PlayerPrefs.Save();
        OnVolumeChanged?.Invoke();
    }
}