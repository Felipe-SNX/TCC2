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
    private Collider2D plataformaAtual;
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
    private InputSystem_Actions controles;
    private PlayerWallSlide wallSlideScript;
    private PlayerWallJump wallJumpScript;

    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        defaultGravity = rb.gravityScale;

        controles = new InputSystem_Actions();
        wallSlideScript = GetComponent<PlayerWallSlide>();
        wallJumpScript = GetComponent<PlayerWallJump>();
        playerClimbScript = GetComponent<PlayerClimb>();

        controles.Player.Jump.performed += context => TentarPulo();
    }

    private void OnEnable()
    {
        controles.Enable();
    }

    private void OnDisable()
    {
        controles.Disable();
    }

    void Update()
    {
        // Se o Dash pausar o movimento, ignora o resto
        if (PlayerState.Instancia != null && PlayerState.Instancia.IsMovementPaused) return;

        Vector2 direcao = controles.Player.Move.ReadValue<Vector2>();
        
        moveInput = direcao.x;
        verticalInput = direcao.y;

        FlipSprite();
    }

    private void TentarPulo()
    {
        // Trava de pause e de Wall Jump
        if (PlayerState.Instancia != null && PlayerState.Instancia.IsMovementPaused) return;
        if (wallJumpScript != null && wallJumpScript.IsWallJumping) return;

        // Descer Plataforma
        if (verticalInput < -0.5f && plataformaAtual != null)
        {
            StartCoroutine(DescerPlataforma());
            return; 
        }

        // Pulo na Planta
        if (playerClimbScript != null && playerClimbScript.IsClimbing)
        {
            playerClimbScript.ExecutarPuloDaPlanta();
            return;
        }

        // Wall Jump
        if (wallSlideScript != null && !IsGrounded() && wallSlideScript.IsWalled()) 
        {
            wallJumpScript?.ExecutarPulo(); 
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
        if (PlayerState.Instancia != null && PlayerState.Instancia.IsMovementPaused) return;
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
    public void AplicarForcaPulo() => Jump();
    public bool IsGrounded() => Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

    private void FlipSprite()
    {
        if (wallJumpScript != null && wallJumpScript.IsWallJumping) return;

        if (moveInput > 0.1f) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < -0.1f) transform.localScale = new Vector3(-1, 1, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlataformaAtravessavel"))
        {
            plataformaAtual = collision.collider;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlataformaAtravessavel"))
        {
            plataformaAtual = null;
        }
    }

    private IEnumerator DescerPlataforma()
    {
        //Salva a plataforma para não poder ligar a colisão dela depois
        Collider2D plataformaParaIgnorar = plataformaAtual;

        if (playerCollider != null && plataformaParaIgnorar != null)
        {
            //Ignora a colisão para permite o jogador passar
            Physics2D.IgnoreCollision(playerCollider, plataformaParaIgnorar, true);
            //Tempo de espera para religar a colisão
            yield return new WaitForSeconds(0.3f);
            //Liga a colisão novamente
            Physics2D.IgnoreCollision(playerCollider, plataformaParaIgnorar, false);
        }
    }
}