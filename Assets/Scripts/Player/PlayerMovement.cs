using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInputController))]
[RequireComponent(typeof(PlayerAnimationHandler))]
[RequireComponent(typeof(PlayerAudioManager))]
[RequireComponent(typeof(PlayerAreaDetector))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Configurações Físicas")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;

    [Header("Detecção de Chão")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private PlayerInputController input;
    private PlayerAnimationHandler animHandler;
    private PlayerAudioManager audio;
    private PlayerAreaDetector areaDetector;
    private PlayerClimb playerClimb;

    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private Collider2D currentPlatform;
    private float verticalInput;
    private float defaultGravity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        defaultGravity = rb.gravityScale;

        input = GetComponent<PlayerInputController>();
        animHandler = GetComponent<PlayerAnimationHandler>();
        audio = GetComponent<PlayerAudioManager>();
        areaDetector = GetComponent<PlayerAreaDetector>();
        playerClimb = GetComponent<PlayerClimb>();
    }

    private void Update()
    {
        if (PlayerState.Instance != null && PlayerState.Instance.IsMovementPaused)
        {
            audio.StopAllMovementSounds();
            return;
        }

        verticalInput = input.MoveVector.y;

        if (input.JumpTriggered) 
        {
            TryJump();
            input.ResetJumpInputs(); 
        }
        
        if (input.JumpCancelled)
        {
            CancelJump();
            input.ResetJumpInputs();
        }

        animHandler.FlipSprite(input.MoveVector.x);
        animHandler.UpdateAnimations(rb, IsGrounded());
        HandleAudioStates();
    }

    private void FixedUpdate()
    {
        if (PlayerState.Instance != null && PlayerState.Instance.IsMovementPaused) return;
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (playerClimb != null && playerClimb.IsClimbing) return;

        rb.gravityScale = defaultGravity;
        rb.linearVelocity = new Vector2(input.MoveVector.x * speed, rb.linearVelocity.y);
    }

    private void TryJump()
    {
        if (PlayerState.Instance != null && PlayerState.Instance.IsMovementPaused)
            return;

        if (input.MoveVector.y < -0.5f && currentPlatform != null)
        {
            StartCoroutine(FallThroughPlatform());
            return;
        }

        if (playerClimb != null && playerClimb.IsClimbing)
        {
            playerClimb.PlantJump();
            audio.PlayJump();
            return;
        }

        if (IsGrounded())
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        audio.PlayJump();
    }

    private void CancelJump()
    {
        if (rb.linearVelocity.y > 0f)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
    }

    private void HandleAudioStates()
    {
        if (areaDetector.IsInVine || (playerClimb != null && playerClimb.IsClimbing))
            audio.ManageVineAudio(input.MoveVector.y);
            
        else if (areaDetector.IsInWater)
            audio.ManageWaterAudio(input.MoveVector.x);
            
        else if (IsGrounded())
            audio.ManageGrassAudio(input.MoveVector.x);
            
        else
            audio.StopAllMovementSounds();
    }

    public bool IsGrounded()
    {
        bool grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        return grounded;
    }

    private IEnumerator FallThroughPlatform()
    {
        Collider2D platformToIgnore = currentPlatform;

        if (playerCollider != null && platformToIgnore != null)
        {
            Physics2D.IgnoreCollision(playerCollider, platformToIgnore, true);
            yield return new WaitForSeconds(0.4f);
            Physics2D.IgnoreCollision(playerCollider, platformToIgnore, false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) { 
        if (collision.gameObject.CompareTag("OneWayPlatform")) 
            currentPlatform = collision.collider; 
    }
    private void OnCollisionExit2D(Collision2D collision) { 
        if (collision.gameObject.CompareTag("OneWayPlatform")) 
            currentPlatform = null; 
    }
}