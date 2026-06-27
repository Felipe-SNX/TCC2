using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private const string KEY_VOLUME_MASTER = "VolumeMaster";
    private const string KEY_VOLUME_MUSICA = "VolumeMusica";
    private const string KEY_VOLUME_SFX = "VolumeSFX";

    [Header("Music Clips")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip levelMusic;
    [SerializeField] private AudioClip pauseMusic;
    [SerializeField] private AudioClip buttonClickSound;

    [Header("SFX Clips")]
    [SerializeField] private AudioClip walkGrassSFX;
    [SerializeField] private AudioClip fallWaterSFX;
    [SerializeField] private AudioClip walkWaterSFX;
    [SerializeField] private AudioClip collectWaterSFX;
    [SerializeField] private AudioClip climbVineSFX;
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip collectMessageSFX;
    [SerializeField] private AudioClip endPhaseSFX;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource loopSfxSource;

    [Header("Volume")]
    [Range(0f, 1f)] [SerializeField] private float masterVolume = 0.5f;
    [Range(0f, 1f)] [SerializeField] private float musicVolume = 0.5f;
    [Range(0f, 1f)] [SerializeField] private float sfxVolume = 0.8f;

    private AudioClip currentMusic;
    private AudioClip currentLoopSFX;

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

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        if (loopSfxSource == null)
        {
            loopSfxSource = gameObject.AddComponent<AudioSource>();
        }

        musicSource.loop = true;
        musicSource.playOnAwake = false;

        sfxSource.loop = false;
        sfxSource.playOnAwake = false;

        loopSfxSource.loop = true;
        loopSfxSource.playOnAwake = false;

        masterVolume = PlayerPrefs.GetFloat(KEY_VOLUME_MASTER, 50f) / 100f;
        musicVolume = PlayerPrefs.GetFloat(KEY_VOLUME_MUSICA, 50f) / 100f;
        sfxVolume = PlayerPrefs.GetFloat(KEY_VOLUME_SFX, 80f) / 100f;

        ApplyVolumes();
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

    public void PlayButtonClick(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip, sfxVolume * masterVolume);
        }
    }

    private void PlayMusic(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("Music clip não configurado.");
            return;
        }

        if (currentMusic == clip && musicSource.isPlaying)
            return;

        currentMusic = clip;
        musicSource.clip = clip;
        ApplyVolumes();
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
        currentMusic = null;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("SFX clip não configurado.");
            return;
        }

        sfxSource.PlayOneShot(clip, sfxVolume * masterVolume);
    }

    public void PlayLoopSFX(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("Loop SFX clip não configurado.");
            return;
        }

        if (currentLoopSFX == clip && loopSfxSource.isPlaying)
            return;

        currentLoopSFX = clip;
        loopSfxSource.clip = clip;
        ApplyVolumes();
        loopSfxSource.Play();
    }

    public void StopLoopSFX(AudioClip clip)
    {
        if (clip == null)
            return;

        if (loopSfxSource.clip == clip)
        {
            loopSfxSource.Stop();
            currentLoopSFX = null;
        }
    }

    public void StopAnyLoopSFX()
    {
        loopSfxSource.Stop();
        currentLoopSFX = null;
    }

    public void PlayWalkGrass()
    {
        PlayLoopSFX(walkGrassSFX);
    }

    public void StopWalkGrass()
    {
        StopLoopSFX(walkGrassSFX);
    }

    public void PlayWalkWater()
    {
        PlayLoopSFX(walkWaterSFX);
    }

    public void StopWalkWater()
    {
        StopLoopSFX(walkWaterSFX);
    }

    public void PlayClimbVine()
    {
        PlayLoopSFX(climbVineSFX);
    }

    public void StopClimbVine()
    {
        StopLoopSFX(climbVineSFX);
    }

    public void PlayFallWater()
    {
        PlaySFX(fallWaterSFX);
    }

    public void PlayCollectWater()
    {
        PlaySFX(collectWaterSFX);
    }

    public void PlayJump()
    {
        PlaySFX(jumpSFX);
    }

    public void PlayCollectMessage()
    {
        PlaySFX(collectMessageSFX);
    }

    public void PlayEndPhase()
    {
        PlaySFX(endPhaseSFX);
    }

    public void PlayCoinSFX(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("SFX clip não configurado.");
            return;
        }

        sfxSource.PlayOneShot(clip, sfxVolume * masterVolume);
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);

        PlayerPrefs.SetFloat(KEY_VOLUME_MASTER, masterVolume * 100f);
        PlayerPrefs.Save();

        ApplyVolumes();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);

        PlayerPrefs.SetFloat(KEY_VOLUME_MUSICA, musicVolume * 100f);
        PlayerPrefs.Save();

        ApplyVolumes();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);

        PlayerPrefs.SetFloat(KEY_VOLUME_SFX, sfxVolume * 100f);
        PlayerPrefs.Save();

        ApplyVolumes();
    }

    private void ApplyVolumes()
    {
        if (musicSource != null)
        {
            musicSource.volume = musicVolume * masterVolume;
        }

        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume * masterVolume;
        }

        if (loopSfxSource != null)
        {
            loopSfxSource.volume = sfxVolume * masterVolume;
        }
    }

    public void ConnectButtons(VisualElement tela)
    {
        if (tela == null || tela == null) return;

        var todosOsBotoes = tela.Query<Button>().ToList();

        foreach (var botao in todosOsBotoes)
        {
            botao.clicked += () => PlayButtonClick(buttonClickSound);
        }
    }
}