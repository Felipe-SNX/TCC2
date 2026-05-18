using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Mixer Central")]
    public AudioMixer meuMixer;

    [Header("Music Clips")]
    public AudioClip menuMusic;
    public AudioClip levelMusic;
    public AudioClip pauseMusic;

    [Header("Audio Source")]
    public AudioSource musicSource;

    [Header("Volume")]
    [Range(0f, 1f)]
    public float musicVolume = 0.1f;

    private AudioClip currentClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }

        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = 1f;
    }

    private void Start()
    {
        float volumeMasterSalvo = PlayerPrefs.GetFloat("VolumeMaster", 1f);
        float volumeSFXSalvo = PlayerPrefs.GetFloat("VolumeSFX", 1f);
        float volumeMusicaSalvo = PlayerPrefs.GetFloat("VolumeMusica", 1f);

        SetMasterVolume(volumeMasterSalvo);
        SetSFXVolume(volumeSFXSalvo);
        SetMusicVolume(volumeMusicaSalvo);
    }

    public void PlayMenuMusic()
    {
        PlayMusic(menuMusic);
    }

    public void PlayLevelMusic()
    {
        PlayMusic(levelMusic);
    }

    public void PlayPauseMusic()
    {
        PlayMusic(pauseMusic);
    }

    private void PlayMusic(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioClip não configurado no AudioManager.");
            return;
        }

        if (currentClip == clip)
            return;

        currentClip = clip;
        musicSource.clip = clip;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void SetMasterVolume(float volume)
    {
        float db = volume > 0 ? Mathf.Log10(volume) * 20 : -80f;
        
        if (meuMixer != null)
        {
            meuMixer.SetFloat("MasterVol", db); 
        }

        PlayerPrefs.SetFloat("VolumeMaster", volume);
    }

    public void SetMusicVolume(float volume)
    {
        float db = volume > 0 ? Mathf.Log10(volume) * 20 : -80f;
        
        if (meuMixer != null)
        {
            meuMixer.SetFloat("MusicaVol", db); 
        }

        PlayerPrefs.SetFloat("VolumeMusica", volume);
    }

    public void SetSFXVolume(float volume)
    {
        float db = volume > 0 ? Mathf.Log10(volume) * 20 : -80f;
        
        if (meuMixer != null)
        {
            meuMixer.SetFloat("SFXVol", db); 
        }

        PlayerPrefs.SetFloat("VolumeSFX", volume);
    }

    public float GetMasterVolume()
    {
        return PlayerPrefs.GetFloat("VolumeMaster", 1f);
    }

    public float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat("VolumeSFX", 1f);
    }

    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat("VolumeMusica", 1f);
    }

    public void StopMusic()
    {
        musicSource.Stop();
        currentClip = null;
    }
}