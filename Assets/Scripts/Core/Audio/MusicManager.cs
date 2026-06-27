using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Music Clips")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip levelMusic;
    [SerializeField] private AudioClip pauseMusic;

    private AudioSource musicSource;
    private AudioClip currentMusic;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        musicSource = GetComponent<AudioSource>();
        musicSource.loop = true;
    }

    private void Start()
    {
        AudioSettingsCore.Instance.OnVolumeChanged += UpdateVolume;
        UpdateVolume();
    }

    private void OnDestroy()
    {
        if (AudioSettingsCore.Instance != null)
            AudioSettingsCore.Instance.OnVolumeChanged -= UpdateVolume;
    }

    private void UpdateVolume()
    {
        float finalVolume = AudioSettingsCore.Instance.MusicVolume * AudioSettingsCore.Instance.MasterVolume;
        Debug.Log($"MusicManager: Volume calculado = {finalVolume}");
        musicSource.volume = finalVolume;
    }

    public void PlayMenuMusic() => PlayMusic(menuMusic);
    public void PlayLevelMusic() => PlayMusic(levelMusic);
    public void PlayPauseMusic() => PlayMusic(pauseMusic);

    private void PlayMusic(AudioClip clip)
    {
        Debug.Log("MusicManager: Tentei tocar " + (clip != null ? clip.name : "NULO"));
        if (clip == null || (currentMusic == clip && musicSource.isPlaying)) return;

        currentMusic = clip;
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
        currentMusic = null;
    }
}