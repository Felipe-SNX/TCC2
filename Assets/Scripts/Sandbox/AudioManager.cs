using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

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
        musicSource.volume = musicVolume;
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

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);

        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
        currentClip = null;
    }
}