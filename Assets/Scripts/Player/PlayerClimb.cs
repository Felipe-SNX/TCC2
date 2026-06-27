using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerClimb : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] private float horizontalSpeed = 8f; 

    public bool IsClimbing { get; private set; }

    private Rigidbody2D rb;
    private PlayerInputController input; 
    
    private bool isNearLadder;
    private bool canClimb = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInputController>();
    }

    private void Update()
    {
        if (PlayerState.Instance != null && PlayerState.Instance.IsMovementPaused) 
        {
            IsClimbing = false;
            return;
        }

        if (isNearLadder) 
            Debug.Log("Perto da escada! Input vertical: " + input.MoveVector.y);

        bool wantsToClimb = Mathf.Abs(input.MoveVector.y) > 0.1f;
        
        if (isNearLadder && wantsToClimb && canClimb)
        {
            if (PlayerState.Instance.CurrentWaterStatus())
            {
                IsClimbing = false;
                Debug.Log("O jogador não pode carregar a água para subir");
            }
            else
            {
                IsClimbing = true;
            }
        }
        else
        {
            IsClimbing = false;
        }
    }

    private void FixedUpdate()
    {
        if (IsClimbing)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(input.MoveVector.x * horizontalSpeed, input.MoveVector.y * climbSpeed);
        }
        else
        {
            rb.gravityScale = 3f; 
        }
    }

    public void PlantJump()
    {
        StartCoroutine(PlantJumpCoroutine());
    }

    private IEnumerator PlantJumpCoroutine()
    {
        canClimb = false;
        IsClimbing = false;
        yield return new WaitForSeconds(0.2f);
        canClimb = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.CompareTag("Ladder") || collision.CompareTag("VineArea"))
        {
            isNearLadder = true;
            Debug.Log("Detectou entrada na escada!");
        }
    }

    private void OnTriggerStay2D(Collider2D collision) 
    {
        if (collision.CompareTag("Ladder") || collision.CompareTag("VineArea"))
        {
            isNearLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {
        if (collision.CompareTag("Ladder") || collision.CompareTag("VineArea"))
        {
            isNearLadder = false;
            IsClimbing = false;
            Debug.Log("Saiu da escada!");
        }
    }

    private void SetLadderState(Collider2D collision, bool state)
    {
        if (collision.CompareTag("Ladder") || collision.CompareTag("VineArea")) 
        {
            isNearLadder = state;
            Debug.Log($"Colisão com {collision.tag} detectada! Estado: {state}");
        }
    }
}