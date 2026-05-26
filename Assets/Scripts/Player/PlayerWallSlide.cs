using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerMovement))]
public class PlayerWallSlide : MonoBehaviour
{
    [Header("Detecção de Parede")]
    [SerializeField] private Transform wallSensor;
    [SerializeField] private float wallCheckRadius = 0.2f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallSlidingSpeed = 2f;

    private Rigidbody2D rb;
    private PlayerMovement playerMovement;

    public bool IsWallSliding { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (PlayerState.Instance != null && PlayerState.Instance.IsMovementPaused) 
            return;

        CheckWall();
    }

    private void FixedUpdate()
    {
        ApplyWallSlide();
    }

    private void CheckWall()
    {
        if (!playerMovement.IsGrounded() && IsWalled() && playerMovement.MoveInput != 0f)
        {
            IsWallSliding = true;
        }
        else
        {
            IsWallSliding = false;
        }
    }

    private void ApplyWallSlide()
    {
        if (IsWallSliding)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlidingSpeed, float.MaxValue));
        }
    }

    public bool IsWalled() => Physics2D.OverlapCircle(wallSensor.position, wallCheckRadius, wallLayer);
}
