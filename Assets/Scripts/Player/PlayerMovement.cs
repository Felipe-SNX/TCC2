using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;

    [Header("Detecção de Chão")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Detecção de Áreas")]
    [SerializeField] private LayerMask vineAreaLayer;
    [SerializeField] private float areaCheckRadius = 0.25f;

    [Header("Áudio de Movimento")]
    [SerializeField] private float movementAudioStopDelay = 0.12f;

    private enum MovementAudioState
    {
        None,
        Grass,
        Water,
        Vine
    }

    private MovementAudioState currentMovementAudio = MovementAudioState.None;
    private float lastMovementAudioTime;

    private Collider2D playerCollider;
    private Collider2D currentPlatform;
    private Animator anim;
    private Rigidbody2D rb;

    private float moveInput;
    private float verticalInput;
    private float defaultGravity;

    private bool isInWaterArea = false;
    private bool isInVineArea = false;

    private int waterAreaContacts = 0;
    private int vineAreaContacts = 0;

    public float MoveInput => moveInput;
    public float DefaultGravity => defaultGravity;
    public float VerticalInput => verticalInput;
    public float Speed => speed;

    private PlayerClimb playerClimbScript;
    private InputSystem_Actions controls;
/*     private PlayerWallSlide wallSlideScript;
    private PlayerWallJump wallJumpScript; */

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();
        defaultGravity = rb.gravityScale;

        controls = new InputSystem_Actions();

/*         wallSlideScript = GetComponent<PlayerWallSlide>();
        wallJumpScript = GetComponent<PlayerWallJump>(); */
        playerClimbScript = GetComponent<PlayerClimb>();

        controls.Player.Jump.performed += context => TryJump();
        controls.Player.Jump.canceled += context => CancelJump();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        StopMovementAudio();

        if (controls != null)
        {
            controls.Disable();
        }
    }

    private void Update()
    {
        if (PlayerState.Instance != null && PlayerState.Instance.IsMovementPaused)
        {
            StopMovementAudio();
            return;
        }

        Vector2 direction = controls.Player.Move.ReadValue<Vector2>();

        moveInput = direction.x;
        verticalInput = direction.y;

        ValidateAreaStates();

        FlipSprite();
        UpdateAnimations();
        HandleMovementAudio();
    }

    private void FixedUpdate()
    {
        if (PlayerState.Instance != null && PlayerState.Instance.IsMovementPaused)
        {
            StopMovementAudio();
            return;
        }

        HandleMovement();
    }

    private void UpdateAnimations()
    {
        if (anim == null)
            return;

        anim.SetFloat("velocityX", Mathf.Abs(rb.linearVelocity.x));
        anim.SetFloat("velocityY", rb.linearVelocity.y);
        anim.SetBool("grounded", IsGrounded());
    }

    private void TryJump()
    {
        if (PlayerState.Instance != null && PlayerState.Instance.IsMovementPaused)
            return;
/* 
        if (wallJumpScript != null && wallJumpScript.IsWallJumping)
            return; */

        if (verticalInput < -0.5f && currentPlatform != null)
        {
            StartCoroutine(FallThroughPlatform());
            return;
        }

        if (playerClimbScript != null && playerClimbScript.IsClimbing)
        {
            playerClimbScript.PlantJump();
            PlayJumpAudio();
            return;
        }
/* 
        if (wallSlideScript != null && !IsGrounded() && wallSlideScript.IsWalled() && wallJumpScript != null)
        {
            wallJumpScript.ExecuteJump();
            PlayJumpAudio();
            return;
        }
 */
        if (IsGrounded())
        {
            Jump();
        }
    }

    private void CancelJump()
    {
        if (rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        }
    }

    private void HandleMovement()
    {
  /*       if (wallJumpScript != null && wallJumpScript.IsWallJumping)
            return; */

        if (playerClimbScript != null && playerClimbScript.IsClimbing)
            return;

        rb.gravityScale = defaultGravity;
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        PlayJumpAudio();
    }

    private void PlayJumpAudio()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayJump();
        }
    }

    private void HandleMovementAudio()
    {
        if (AudioManager.Instance == null || rb == null)
            return;

        bool grounded = IsGrounded();
        bool isClimbing = playerClimbScript != null && playerClimbScript.IsClimbing;

        bool isMovingHorizontal =
            Mathf.Abs(moveInput) > 0.1f ||
            Mathf.Abs(rb.linearVelocity.x) > 0.15f;

        bool isMovingVertical =
            Mathf.Abs(verticalInput) > 0.1f ||
            Mathf.Abs(rb.linearVelocity.y) > 0.15f;

        if (isClimbing || isInVineArea)
        {
            if (isMovingVertical)
            {
                lastMovementAudioTime = Time.time;
                SetMovementAudio(MovementAudioState.Vine);
            }
            else if (Time.time - lastMovementAudioTime >= movementAudioStopDelay)
            {
                SetMovementAudio(MovementAudioState.None);
            }

            return;
        }

        if (currentMovementAudio == MovementAudioState.Vine)
        {
            SetMovementAudio(MovementAudioState.None);
        }

        if (isInWaterArea)
        {
            if (isMovingHorizontal)
            {
                lastMovementAudioTime = Time.time;
                SetMovementAudio(MovementAudioState.Water);
            }
            else if (Time.time - lastMovementAudioTime >= movementAudioStopDelay)
            {
                SetMovementAudio(MovementAudioState.None);
            }

            return;
        }

        if (grounded && isMovingHorizontal)
        {
            lastMovementAudioTime = Time.time;
            SetMovementAudio(MovementAudioState.Grass);
            return;
        }

        if (Time.time - lastMovementAudioTime >= movementAudioStopDelay)
        {
            SetMovementAudio(MovementAudioState.None);
        }
    }

    private void SetMovementAudio(MovementAudioState newState)
    {
        if (AudioManager.Instance == null)
            return;

        if (currentMovementAudio == newState)
            return;

        AudioManager.Instance.StopWalkGrass();
        AudioManager.Instance.StopWalkWater();
        AudioManager.Instance.StopClimbVine();

        currentMovementAudio = newState;

        switch (newState)
        {
            case MovementAudioState.Grass:
                AudioManager.Instance.PlayWalkGrass();
                break;

            case MovementAudioState.Water:
                AudioManager.Instance.PlayWalkWater();
                break;

            case MovementAudioState.Vine:
                AudioManager.Instance.PlayClimbVine();
                break;

            case MovementAudioState.None:
                break;
        }
    }

    private void StopMovementAudio()
    {
        if (AudioManager.Instance == null)
            return;

        AudioManager.Instance.StopWalkGrass();
        AudioManager.Instance.StopWalkWater();
        AudioManager.Instance.StopClimbVine();

        currentMovementAudio = MovementAudioState.None;
    }

    public void AddJumpForce()
    {
        Jump();
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void FlipSprite()
    {
/*         if (wallJumpScript != null && wallJumpScript.IsWallJumping)
            return; */

        Vector3 escala = transform.localScale;

        if (moveInput > 0.1f)
        {
            escala.x = Mathf.Abs(escala.x);
        }
        else if (moveInput < -0.1f)
        {
            escala.x = -Mathf.Abs(escala.x);
        }

        transform.localScale = escala;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentPlatform = collision.collider;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentPlatform = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger detectado: " + other.gameObject.name + " | Tag: " + other.tag);

        if (other.CompareTag("WaterArea"))
        {
            waterAreaContacts++;
            isInWaterArea = waterAreaContacts > 0;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayFallWater();
            }

            Debug.Log("Entrou na área da água. Contatos: " + waterAreaContacts);
        }

        if (other.CompareTag("VineArea"))
        {
            vineAreaContacts++;
            isInVineArea = vineAreaContacts > 0;

            Debug.Log("Entrou na área da trepadeira. Contatos: " + vineAreaContacts);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("WaterArea"))
        {
            waterAreaContacts--;
            waterAreaContacts = Mathf.Max(0, waterAreaContacts);

            isInWaterArea = waterAreaContacts > 0;

            if (!isInWaterArea && currentMovementAudio == MovementAudioState.Water)
            {
                SetMovementAudio(MovementAudioState.None);
            }

            Debug.Log("Saiu da área da água. Contatos: " + waterAreaContacts);
        }

        if (other.CompareTag("VineArea"))
        {
            vineAreaContacts--;
            vineAreaContacts = Mathf.Max(0, vineAreaContacts);

            isInVineArea = vineAreaContacts > 0;

            if (!isInVineArea && currentMovementAudio == MovementAudioState.Vine)
            {
                SetMovementAudio(MovementAudioState.None);
            }

            Debug.Log("Saiu da área da trepadeira. Contatos: " + vineAreaContacts);
        }
    }

    private void ValidateAreaStates()
    {
        bool touchingVineArea = Physics2D.OverlapCircle(transform.position, areaCheckRadius, vineAreaLayer);

        if (!touchingVineArea && isInVineArea)
        {
            vineAreaContacts = 0;
            isInVineArea = false;

            if (currentMovementAudio == MovementAudioState.Vine)
            {
                SetMovementAudio(MovementAudioState.None);
            }

            Debug.Log("Saiu da área da trepadeira por validação.");
        }
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
}