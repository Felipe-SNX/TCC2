using UnityEngine;

public class LightInteractable : MonoBehaviour
{
    [Header("Estado de Energia")]
    [SerializeField] private bool isPowered = false;
    
    // Getter público para que outros scripts consultem o estado
    public bool IsPowered => isPowered;

    public virtual void OnLightHit()
    {
        if (isPowered) return;

        Energize();
    }

    protected virtual void Energize()
    {
        isPowered = true;
        Debug.Log($"[Energia] {gameObject.name} agora está ativo!");
        
        // Feedback visual simples trocando a cor "ideia"
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
        {
            sr.color = Color.yellow; // Representa que está energizado
        }
    }

    // Método para desligar, caso necessário
    public virtual void ResetPower()
    {
        isPowered = false;
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
        {
            sr.color = Color.white;
        }
    }
}