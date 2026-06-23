using System.Collections;
using UnityEngine;

public class PlantLogic : MonoBehaviour
{
    public enum GrowthDirection
    {
        Vertical,
        Horizontal
    }

    [Header("Crescimento")]
    [SerializeField] private GrowthDirection growthDirection = GrowthDirection.Vertical;
    [SerializeField] private SpriteRenderer bodyRenderer;
    [SerializeField] private BoxCollider2D climbCollider;
    [SerializeField] private float maxSize = 6f;
    [SerializeField] private float growSpeed = 3f;
    [SerializeField] private ParticleSystem growthParticles;
    [SerializeField] private PlantGlow plantGlow;

    private bool isGrown;
    private bool isGrowing;
    private bool playerContact;

    private InputSystem_Actions controls;

    void Awake()
    {
        controls = new InputSystem_Actions();

        controls.Player.Interact.performed += ctx =>
        {
            TentarCrescerPlanta();
        };
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void TentarCrescerPlanta()
    {
        if (!playerContact) return;
        if (isGrown || isGrowing) return;

        PlayerMovement movimento =
            FindAnyObjectByType<PlayerMovement>();

        if (movimento != null &&
            !movimento.IsGrounded())
        {
            Debug.Log(
                "Você precisa estar no chão para usar a água"
            );

            return;
        }

        if (PlayerState.Instance != null &&
            PlayerState.Instance.UseWater())
        {
            if(plantGlow != null)
                plantGlow.DisableGlow();

            StartCoroutine(Grow());
        }
        else
        {
            if(UIManager.Instance != null)
            {
                UIManager.Instance.ShowMessage(
                    "Você precisa de água para esta planta."
                );
            }
        }
    }

    IEnumerator Grow()
    {
        isGrowing = true;


        if(growthParticles != null)
            growthParticles.Play();


        Vector2 size = bodyRenderer.size;


        while(size.y < maxSize)
        {
            size.y +=
                growSpeed * Time.deltaTime;


            if(size.y > maxSize)
                size.y = maxSize;


            bodyRenderer.size = size;


            AtualizarCollider(size.y);


            yield return null;
        }


        if(growthParticles != null)
        {
            growthParticles.Stop(
                true,
                ParticleSystemStopBehavior.StopEmitting
            );
        }


        isGrowing = false;
        isGrown = true;


        FinalizarCrescimento();


        Debug.Log("A planta cresceu!");
    }   

    void AtualizarCollider(float height)
    {
        if(!climbCollider) return;

        climbCollider.size =
        new Vector2(
            climbCollider.size.x,
            height
        );

        // mantém o pé da planta fixo
        climbCollider.offset =
        new Vector2(
            climbCollider.offset.x,
            height / 2f
        );
    }

    void FinalizarCrescimento()
    {
        if(!climbCollider) return;


        if(growthDirection ==
           GrowthDirection.Vertical)
        {
            climbCollider.gameObject.tag =
                "Ladder";
        }


        if(growthDirection ==
           GrowthDirection.Horizontal)
        {
            climbCollider.isTrigger = false;


            climbCollider.gameObject.layer =
                LayerMask.NameToLayer("Ground");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerContact = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerContact = false;
        }
    }
}