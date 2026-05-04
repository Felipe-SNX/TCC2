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

    private Rigidbody2D rb;
    private float moveInput;
    private float verticalInput;
    private bool isClimbing;
    private float defaultGravity;

    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravity = rb.gravityScale;
    }

    void Update()
    {
        // Se o Dash pausar o movimento, ignora o resto
        if (IsMovementPaused) return;

        moveInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump") && IsGrounded() && !isClimbing)
        {
            Jump();
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
}