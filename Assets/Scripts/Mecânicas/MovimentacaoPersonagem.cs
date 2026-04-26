using UnityEngine;

public class MovimentacaoPersonagem : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float speed = 8f;
    public float jumpForce = 12f;
    public float climbSpeed = 5f;

    [Header("Referências")]
    public Rigidbody2D rb;
    public LayerMask groundLayer; // Para o pulo

    // Variáveis de controle
    private float moveInput;
    private float verticalInput;
    private bool isClimbing;
    private float defaultGravity;

    void Start()
    {
        defaultGravity = rb.gravityScale;
    }

    void Update()
    {
        // Inputs Globais
        moveInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Lógica de Pulo
        if (Input.GetButtonDown("Jump") && IsGrounded() && !isClimbing)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        // 3. Lógica de Escalar vs Andar
        if (isClimbing)
        {
            rb.gravityScale = 0f; // Tira a gravidade na trepadeira
            rb.linearVelocity = new Vector2(moveInput * speed, verticalInput * climbSpeed);
        }
        else
        {
            rb.gravityScale = defaultGravity; // Devolve a gravidade no chão
            rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
        }
    }

    // Gatilhos para a Trepadeira
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Escada")) isClimbing = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Escada")) isClimbing = false;
    }

    bool IsGrounded() {
        return true; 
    }
}
