using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; private set; }

    [Header("Configurações Visuais")]
    [SerializeField] private Color normalColor = new Color(0.8117f, 0.8117f, 0.8117f, 1f);
    [SerializeField] private Color waterColor = Color.blue;

    [Header("Estado")]
    [SerializeField] private bool hasWater = false;
    public bool IsMovementPaused { get; private set; }

    private SpriteRenderer sr;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        sr = GetComponent<SpriteRenderer>();
    }

    public void SetPauseMovement(bool isPaused)
    {
        IsMovementPaused = isPaused;
        Debug.Log(isPaused ? "Movimento do Player Pausado." : "Movimento do Player Liberado.");
    }

    public void CollectWater()
    {
        hasWater = true;
        Debug.Log("Player pegou água!");
        UpdateVisuals();
    }

    public void DiscardWater()
    {
        hasWater = false;
        Debug.Log("Player descartou a água!");
        UpdateVisuals();
    }

    public bool UseWater()
    {
        if (!hasWater)
            return false;

        hasWater = false;

        if (AbilityPulseEffect.Instance != null)
        {
            AbilityPulseEffect.Instance.PlayPulse(Color.green);
        }
        
        UpdateVisuals();
        return true;
    }

    private void UpdateVisuals()
    {
        if (sr != null)
        {
            sr.color = hasWater ? waterColor : normalColor;
        }
    }

    public void PauseMovement() => IsMovementPaused = true;
    public void ResumeMovement() => IsMovementPaused = false;
    public bool CurrentWaterStatus() => hasWater;
}