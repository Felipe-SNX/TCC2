using UnityEngine;

public class PlantLogic : MonoBehaviour
{
    public enum GrowthDirection
    {
        Vertical,
        Horizontal
    }

    [Header("Configurações de Crescimento")]
    [SerializeField] private Sprite grownSprite;
    [SerializeField] private GrowthDirection growthDirection = GrowthDirection.Vertical;
    
    private Animator animator;
    private SpriteRenderer sr;
    private Collider2D plantCollider;
    private bool isGrown = false;
    

    

    void Awake()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        plantCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Se já cresceu ou não é o player, ignora
        if (isGrown || !other.CompareTag("Player")) return;

        // Tenta pegar o estado do player
        if (other.TryGetComponent<PlayerState>(out PlayerState player))
        {
            // Se o player conseguir usar a água (bool for true)
            if (player.UseWater()){
                Grow();
            } else {
                UIManager.Instance.ShowMessage("Você precisa de água para esta planta.");
            }
        }
    }

    void Grow()
    {
        isGrown = true;
        
        // Ativa animação
        if (animator != null) animator.SetTrigger("Crescer");

        // Troca o sprite se houver um definido
        if (grownSprite != null && sr != null) sr.sprite = grownSprite;

        // Altera a cor do SpriteRenderer ao crescer
        if (sr != null)
        {
            sr.color = Color.green;
        }
        
        // Configura a colisão com base no tipo de crescimento (vertical ou horizontal)
        if (growthDirection == GrowthDirection.Horizontal)
        {
            if (plantCollider != null)
            {
                plantCollider.isTrigger = false; 
                gameObject.layer = LayerMask.NameToLayer("Ground");
            }
        }
        else
        {
            // Altera tag de colisão do sprite após crescer para o tipo vertical (Escada)
            gameObject.tag = "Escada";
        }

        Debug.Log("A planta cresceu!");
    }
}