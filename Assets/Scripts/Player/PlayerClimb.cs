using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerMovement))]
public class PlayerClimb : MonoBehaviour
{
    [Header("Configurações de Escalada")]
    [SerializeField] private float climbSpeed = 5f;

    public bool IsClimbing { get; private set; }

    private Rigidbody2D rb;
    private PlayerMovement playerMovement;
    
    private bool isNearLadder;
    private bool canClimb = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (PlayerState.Instancia != null && PlayerState.Instancia.IsMovementPaused) return;

        if (isNearLadder && Mathf.Abs(playerMovement.VerticalInput) > 0.1f && canClimb)
        {
            if (PlayerState.Instancia.CurrentWaterStatus())
            {
                IsClimbing = false;
                Debug.Log("O jogador não pode carregar a água para subir");
            }
            else
            {
                IsClimbing = true;
            }
        }
        else if (!isNearLadder || !canClimb)
        {
            IsClimbing = false;
        }
    }

    private void FixedUpdate()
    {
        if (IsClimbing)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(playerMovement.MoveInput * playerMovement.Speed, playerMovement.VerticalInput * climbSpeed);
        }
    }

    public void ExecutarPuloDaPlanta()
    {
        StartCoroutine(RotinaPularDaPlanta());
    }

    private IEnumerator RotinaPularDaPlanta()
    {
        canClimb = false;
        IsClimbing = false;

        playerMovement.AplicarForcaPulo(); 

        yield return new WaitForSeconds(0.2f);

        canClimb = true;
    }

    private void OnTriggerStay2D(Collider2D collision) => CheckClimbing(collision, true);
    private void OnTriggerExit2D(Collider2D collision) => CheckClimbing(collision, false);

    private void CheckClimbing(Collider2D collision, bool state)
    {
        if (collision.CompareTag("Escada")) isNearLadder = state;
    }
}