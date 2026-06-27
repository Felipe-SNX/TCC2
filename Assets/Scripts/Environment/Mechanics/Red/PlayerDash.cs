using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInputController))]
public class PlayerDash : MonoBehaviour
{
    [Header("Configurações de Dash")]
    [SerializeField] private float dashVelocity = 24f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private Rigidbody2D rb;
    private PlayerInputController input;
    private bool canDash = true;
    private float defaultGravity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInputController>();
        defaultGravity = rb.gravityScale;
    }

    private void Update()
    {
        if (input.DashTriggered && canDash)
        {
            StartCoroutine(PerformDash());
            input.ResetDashInput();
        }
    }

    private IEnumerator PerformDash()
    {
        canDash = false;
        
        PlayerState.Instance?.SetPauseMovement(true); 
        
        rb.gravityScale = 0f;
        
        float direction = input.MoveVector.x != 0 ? Mathf.Sign(input.MoveVector.x) : Mathf.Sign(transform.localScale.x);
        rb.linearVelocity = new Vector2(direction * dashVelocity, 0f);

        yield return new WaitForSeconds(dashTime);

        rb.gravityScale = defaultGravity;
        PlayerState.Instance?.SetPauseMovement(false);

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}