using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerMovement))]
public class PlayerWallJump : MonoBehaviour
{
    [Header("Configurações do Wall Jump")]
    [SerializeField] private Vector2 wallJumpForce = new Vector2(5f, 12f);
    [SerializeField] private float wallJumpDuration = 0.4f;
    [SerializeField] private float wallJumpTime = 0.2f;
    
    public bool IsWallJumping { get; private set; }
    private float wallJumpDirection;
    private float wallJumpCounter;

    private Rigidbody2D rb;
    private PlayerWallSlide wallSlideScript;
    private PlayerMovement movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        wallSlideScript = GetComponent<PlayerWallSlide>();
        movement = GetComponent<PlayerMovement>();
    }

    public void ExecuteJump()
    {
        WallJump();
    }

    private void WallJump()
    {
        if (wallSlideScript.IsWallSliding)
        {
            IsWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpCounter = wallJumpTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpCounter -= Time.deltaTime;
        }

        if(wallJumpCounter > 0f)
        {
            IsWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpForce.x, wallJumpForce.y);
            wallJumpCounter = 0f;

        if (Mathf.Sign(transform.localScale.x) != Mathf.Sign(wallJumpDirection))
        {
            Vector3 escala = transform.localScale;
            escala.x = Mathf.Sign(wallJumpDirection) * Mathf.Abs(escala.x);
            transform.localScale = escala;
        }

            Invoke(nameof(StopWallJumping), wallJumpDuration);
        }
    }

    private void StopWallJumping()
    {
        IsWallJumping = false;
    }
}