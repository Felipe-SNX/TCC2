using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; private set; }

    [Header("Estado do Inventário")]
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
    }

    public void SetPauseMovement(bool isPaused)
    {
        IsMovementPaused = isPaused;
        Debug.Log(isPaused ? "Movimento do Player Pausado." : "Movimento do Player Liberado.");
    }

    public void CollectWater()
    {
        sr = GetComponent<SpriteRenderer>();
        hasWater = true;
        Debug.Log("Player pegou água!");
        ChangeColor(sr);
    }

    public void DiscardWater()
    {
        sr = GetComponent<SpriteRenderer>();
        hasWater = false;
        Debug.Log("Player descartou a água!");
        ChangeColor(sr);
    }

    private void ChangeColor(SpriteRenderer sr)
    {
        if (hasWater && sr != null)
        {
            sr.color = Color.blue;
        } else {
            sr.color = new Color(0.8117f, 0.8117f, 0.8117f, 1f);
        }
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
        
        sr = GetComponent<SpriteRenderer>();
        ChangeColor(sr);

        return true;
    }

    public void PauseMovement()
    {
        IsMovementPaused = true;
    }

    public void ResumeMovement()
    {
        IsMovementPaused = false;
    }

    public bool CurrentWaterStatus() => hasWater;
}