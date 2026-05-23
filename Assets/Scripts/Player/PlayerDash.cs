using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("Configurações de Dash")]
    [SerializeField] private float dashVelocity = 24f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private Rigidbody2D rb;
    private PlayerMovement movement;
    private bool canDash = true;

    private InputSystem_Actions controles;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();

        controles = new InputSystem_Actions();
        
        controles.Player.Dash.performed += context => TentarDash();
    }

    private void OnEnable()
    {
        controles.Enable();
    }

    private void OnDisable()
    {
        controles.Disable();
    }

    private void TentarDash()
    {
        if (canDash)
        {
            StartCoroutine(PerformDash());
        }
    }

    private IEnumerator PerformDash()
    {
        canDash = false;
        
        // Avisa o script de movimento para parar de processar
        movement.IsMovementPaused = true; 
        
        rb.gravityScale = 0f;
        
        // Usa a direção do input do PlayerMovement ou o lado que o sprite está virado
        float rawDirection = movement.MoveInput != 0 ? movement.MoveInput : transform.localScale.x;
        float dashDirection = Mathf.Sign(rawDirection);
        rb.linearVelocity = new Vector2(dashDirection * dashVelocity, 0f);

        yield return new WaitForSeconds(dashTime);

        // Devolve a gravidade e libera o movimento
        rb.gravityScale = movement.DefaultGravity;
        movement.IsMovementPaused = false; 

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}