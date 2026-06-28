using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationHandler : MonoBehaviour
{
    private Animator anim;
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void UpdateAnimations(Rigidbody2D rb, bool isGrounded)
    {
        if (anim == null) return;

        anim.SetFloat("velocityX", Mathf.Abs(rb.linearVelocity.x));
        anim.SetFloat("velocityY", rb.linearVelocity.y);
        anim.SetBool("grounded", isGrounded);
    }

    public void FlipSprite(float moveInput)
    {
        if (Mathf.Abs(moveInput) < 0.1f) return;

        Vector3 escala = transform.localScale;
        escala.x = Mathf.Sign(moveInput) * Mathf.Abs(escala.x);
        transform.localScale = escala;
    }
}