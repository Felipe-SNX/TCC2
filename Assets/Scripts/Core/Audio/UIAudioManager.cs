using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(AudioSource))]
public class UIAudioManager : MonoBehaviour
{
    public static UIAudioManager Instance { get; private set; }

    [Header("UI SFX Clips")]
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip collectMessageSFX;
    [SerializeField] private AudioClip endPhaseSFX;
    [SerializeField] private AudioClip coinSound; 

    private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        sfxSource = GetComponent<AudioSource>();
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
        sfxSource.volume = AudioSettingsCore.Instance.SfxVolume * AudioSettingsCore.Instance.MasterVolume;
    }

    public void PlayCollectMessage() => PlaySFX(collectMessageSFX);
    public void PlayEndPhase() => PlaySFX(endPhaseSFX);
    private void PlayButtonClick() => PlaySFX(buttonClickSound);

    public void PlayCoinSFX()
    {
        if (coinSound != null) sfxSource.PlayOneShot(coinSound);
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null) sfxSource.PlayOneShot(clip);
    }

    public void ConnectButtons(VisualElement tela)
    {
        if (tela == null) return;
        var todosOsBotoes = tela.Query<Button>().ToList();
        foreach (var botao in todosOsBotoes)
        {
            botao.clicked += PlayButtonClick;
        }
    }
}