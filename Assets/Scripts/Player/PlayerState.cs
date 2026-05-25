using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instancia { get; private set; }

    [Header("Estado do Inventário")]
    [SerializeField] private bool hasWater = false;

    public bool IsMovementPaused { get; private set; }

    private SpriteRenderer sr;
    
    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }
        Instancia = this;
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
            sr.color = Color.red;
        }
    }

    public bool UseWater()
    {
        if (hasWater)
        {
            hasWater = false;
            Debug.Log("Player usou água!");
            if (hasWater && sr != null)
            {
                sr.color = Color.blue;
            } else {
                sr.color = Color.red;
            }
            return true;
        }

        Debug.Log("O jogador não possui água.");
        return false;
    }

    public bool CurrentWaterStatus() => hasWater;
}