using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float climbSpeed = 5f;
    
    [Header("Detecção de Chão")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    public bool IsMovementPaused { get; set; } 
    public float MoveInput => moveInput;
    public float DefaultGravity => defaultGravity;
    private Collider2D playerCollider;
    private Collider2D plataformaAtual;
    private Rigidbody2D rb;
    private float moveInput;
    private float verticalInput;
    private bool isClimbing;
    private float defaultGravity;

    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        defaultGravity = rb.gravityScale;
    }

    void Update()
    {
        // Se o Dash pausar o movimento, ignora o resto
        if (IsMovementPaused) return;

        moveInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            if (verticalInput < -0.5f && plataformaAtual != null)
            {
                StartCoroutine(DescerPlataforma());
            }
            else if (IsGrounded() && !isClimbing)
            {
                Jump();
            }
        }

        FlipSprite();
    }

    void FixedUpdate()
    {
        if (IsMovementPaused) return;
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(moveInput * speed, verticalInput * climbSpeed);
        }
        else
        {
            rb.gravityScale = defaultGravity;
            rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
        }
    }

    private void Jump() => rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

    private bool IsGrounded() => Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

    private void FlipSprite()
    {
        if (moveInput > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision) => CheckClimbing(collision, true);
    private void OnTriggerExit2D(Collider2D collision) => CheckClimbing(collision, false);

    private void CheckClimbing(Collider2D collision, bool state)
    {
        if (collision.CompareTag("Escada")) isClimbing = state;
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