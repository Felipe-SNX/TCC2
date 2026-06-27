using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [Header("SFX Clips")]
    [SerializeField] private AudioClip walkGrassSFX;
    [SerializeField] private AudioClip fallWaterSFX;
    [SerializeField] private AudioClip walkWaterSFX;
    [SerializeField] private AudioClip collectWaterSFX;
    [SerializeField] private AudioClip climbVineSFX;
    [SerializeField] private AudioClip jumpSFX;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource loopSfxSource;

    private AudioClip currentLoopSFX;

    private void Awake()
    {
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }

        if (loopSfxSource == null)
        {
            loopSfxSource = gameObject.AddComponent<AudioSource>();
            loopSfxSource.loop = true;
            loopSfxSource.playOnAwake = false;
        }
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
        float finalVol = AudioSettingsCore.Instance.SfxVolume * AudioSettingsCore.Instance.MasterVolume;
        sfxSource.volume = finalVol;
        loopSfxSource.volume = finalVol;
    }

    public void PlayJump() => PlaySFX(jumpSFX);
    public void PlayFallWater() => PlaySFX(fallWaterSFX);
    public void PlayCollectWater() => PlaySFX(collectWaterSFX);

    public void PlayWalkGrass() => PlayLoopSFX(walkGrassSFX);
    public void StopWalkGrass() => StopLoopSFX(walkGrassSFX);
    
    public void PlayWalkWater() => PlayLoopSFX(walkWaterSFX);
    public void StopWalkWater() => StopLoopSFX(walkWaterSFX);
    
    public void PlayClimbVine() => PlayLoopSFX(climbVineSFX);
    public void StopClimbVine() => StopLoopSFX(climbVineSFX);

    public void StopAnyLoopSFX()
    {
        loopSfxSource.Stop();
        currentLoopSFX = null;
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null) sfxSource.PlayOneShot(clip);
    }

    private void PlayLoopSFX(AudioClip clip)
    {
        if (clip == null || (currentLoopSFX == clip && loopSfxSource.isPlaying)) return;
        currentLoopSFX = clip;
        loopSfxSource.clip = clip;
        loopSfxSource.Play();
    }

    private void StopLoopSFX(AudioClip clip)
    {
        if (clip != null && loopSfxSource.clip == clip)
        {
            loopSfxSource.Stop();
            currentLoopSFX = null;
        }
    }

    public void ManageVineAudio(float yInput)
    {
        if (Mathf.Abs(yInput) > 0.1f)
            PlayClimbVine();
        else
            StopClimbVine();
    }

    public void ManageWaterAudio(float xInput)
    {
        if (Mathf.Abs(xInput) > 0.1f)
            PlayWalkWater();
        else
            StopWalkWater();
    }

    public void ManageGrassAudio(float xInput)
    {
        if (Mathf.Abs(xInput) > 0.1f)
            PlayWalkGrass();
        else
            StopWalkGrass();
    }

    public void StopAllMovementSounds()
    {
        StopWalkGrass();
        StopWalkWater();
        StopClimbVine();
    }
}