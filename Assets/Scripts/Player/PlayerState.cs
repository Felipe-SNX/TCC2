using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instancia { get; private set; }

    [Header("Estado do Inventário")]
    [SerializeField] private bool hasWater = false;

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

    public void CollectWater()
    {
        sr = GetComponent<SpriteRenderer>();
        hasWater = true;
        Debug.Log("Player pegou água!");
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