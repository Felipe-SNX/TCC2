using System.Collections;
using UnityEngine;

public class MovimentacaoPersonagem : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float speed = 8f;
    public float jumpForce = 12f;
    public float climbSpeed = 5f;

    [Header("Configurações de Dash")]
    public float dashVelocity = 24f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;
    private bool canDash = true;
    private bool isDashing;

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
        //Verifica se não está em modo dash
        if (isDashing) return;

        // Inputs Globais
        moveInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //Lógica de Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        // Lógica de Pulo
        if (Input.GetButtonDown("Jump") && IsGrounded() && !isClimbing)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        if (isDashing) return;

        // Lógica de Escalar vs Andar
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

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        
        // Salva a gravidade atual e zera para o dash ser reto
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        // Aplica a velocidade baseada na direção que o jogador está olhando
        // Se não houver input, dash para a direita
        float dashDirection = moveInput != 0 ? moveInput : transform.localScale.x;
        rb.linearVelocity = new Vector2(dashDirection * dashVelocity, 0f);

        yield return new WaitForSeconds(dashTime);

        // Restaura o estado original
        rb.gravityScale = originalGravity;
        isDashing = false;

        // Espera o cooldown para permitir outro dash
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
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
