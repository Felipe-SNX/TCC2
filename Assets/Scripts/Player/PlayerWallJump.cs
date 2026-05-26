using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerMovement))]
public class PlayerWallJump : MonoBehaviour
{
    [Header("Configurações do Wall Jump")]
    [SerializeField] private Vector2 wallJumpForce = new Vector2(5f, 12f);
    [SerializeField] private float wallJumpDuration = 0.15f;
    
    public bool IsWallJumping { get; private set; }

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ExecutarPulo()
    {
        StartCoroutine(WallJump());
    }

    private IEnumerator WallJump()
    {
        IsWallJumping = true;

        float direcaoOposta = -Mathf.Sign(transform.localScale.x);

        // Zera a velocidade antes, para não dar um efeito de quicar
        rb.linearVelocity = Vector2.zero;

        // Aplica a força de disparo 
        rb.linearVelocity = new Vector2(wallJumpForce.x * direcaoOposta, wallJumpForce.y);

        // Vira o Sprite
        transform.localScale = new Vector3(direcaoOposta, 1, 1);

        // Não permite a interferência do jogador durante o movimento
        yield return new WaitForSeconds(wallJumpDuration);

        IsWallJumping = false;
    }
}