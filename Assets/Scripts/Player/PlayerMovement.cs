using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpForce = 12f;
    
    [Header("Detecção de Chão")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Collider2D playerCollider;
    private Collider2D currentPlatform;
    private Rigidbody2D rb;
    private float moveInput;
    private float verticalInput;
    private float defaultGravity;

    // Parâmetros Expostos
    public float MoveInput => moveInput;
    public float DefaultGravity => defaultGravity;
    public float VerticalInput => verticalInput;
    public float Speed => speed;

    // Outros Componentes
    private PlayerClimb playerClimbScript;
    private InputSystem_Actions controls;
    private PlayerWallSlide wallSlideScript;
    private PlayerWallJump wallJumpScript;

    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        defaultGravity = rb.gravityScale;

        controls = new InputSystem_Actions();
        wallSlideScript = GetComponent<PlayerWallSlide>();
        wallJumpScript = GetComponent<PlayerWallJump>();
        playerClimbScript = GetComponent<PlayerClimb>();

        controls.Player.Jump.performed += context => TryJump();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void Update()
    {
        // Se alguma mecânica pausar o movimento, ignora o resto
        if (PlayerState.Instance != null && PlayerState.Instance.IsMovementPaused) return;

        Vector2 direction = controls.Player.Move.ReadValue<Vector2>();
        
        moveInput = direction.x;
        verticalInput = direction.y;

        FlipSprite();
    }

    private void TryJump()
    {
        // Trava de pause e de Wall Jump
        if (PlayerState.Instance != null && PlayerState.Instance.IsMovementPaused) return;
        if (wallJumpScript != null && wallJumpScript.IsWallJumping) return;

        // Descer Plataforma
        if (verticalInput < -0.5f && currentPlatform != null)
        {
            StartCoroutine(FallThroughPlatform());
            return; 
        }

        // Pulo na Planta
        if (playerClimbScript != null && playerClimbScript.IsClimbing)
        {
            playerClimbScript.PlantJump();
            return;
        }

        // Wall Jump
        if (wallSlideScript != null && !IsGrounded() && wallSlideScript.IsWalled() && wallJumpScript != null) 
        {
            wallJumpScript.ExecuteJump();
            return;
        }

        // Pulo Normal 
        if (IsGrounded())
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        if (PlayerState.Instance != null && PlayerState.Instance.IsMovementPaused) return;
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (wallJumpScript != null && wallJumpScript.IsWallJumping) return;
        if (playerClimbScript != null && playerClimbScript.IsClimbing) return;

        rb.gravityScale = defaultGravity;
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
    }

    private void Jump() => rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    public void AddJumpForce() => Jump();
    public bool IsGrounded() => Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

  private void FlipSprite()
    {
        // Não realiza no WallJump, pois em pulos consecutivos ele precisa estar virado contra a parede sempre
        if (wallJumpScript != null && wallJumpScript.IsWallJumping) return;

        Vector3 escala = transform.localScale;

        if (moveInput > 0.1f) 
        {
            // Pega o valor puro e garante que é POSITIVO (Olha para a direita)
            escala.x = Mathf.Abs(escala.x);
        }
        else if (moveInput < -0.1f) 
        {
            // Pega o valor puro e garante que é NEGATIVO (Olha para a esquerda)
            escala.x = -Mathf.Abs(escala.x);
        }

        transform.localScale = escala;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentPlatform = collision.collider;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentPlatform = null;
        }
    }

    private IEnumerator FallThroughPlatform()
    {
        // Salva a plataforma para poder ligar a colisão dela depois
        Collider2D platformToIgnore = currentPlatform;

        if (playerCollider != null && platformToIgnore != null)
        {
            // Ignora a colisão para permitir o jogador passar
            Physics2D.IgnoreCollision(playerCollider, platformToIgnore, true);
            // Tempo de espera para religar a colisão
            yield return new WaitForSeconds(0.4f);
            // Liga a colisão novamente
            Physics2D.IgnoreCollision(playerCollider, platformToIgnore, false);
        }
    }
}