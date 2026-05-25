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

    private bool playerContact = false;
    private InputSystem_Actions controls;

    void Awake()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        plantCollider = GetComponent<Collider2D>();

        controls = new InputSystem_Actions();
        
        controls.Player.Interact.performed += context => TentarCrescerPlanta();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void TentarCrescerPlanta()
    {        
        if (!playerContact || isGrown) return;

        if (PlayerState.Instancia != null)
        {
            if (PlayerState.Instancia.UseWater())
            {
                Grow();
            } 
            else 
            {
                if (UIManager.Instance != null)
                    UIManager.Instance.ShowMessage("Você precisa de água para esta planta.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerContact = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerContact = false;
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