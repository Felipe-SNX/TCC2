using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightController : MonoBehaviour
{
    public float lightDistance = 5f;
    public LayerMask interactableLayer;

    void Update()
    {
        if (Keyboard.current.eKey.isPressed)
        {
            Debug.Log("Lanterna ativada");
            ShootLight();
        }
    }

    void ShootLight()
    {
        Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f;

        Vector2 direction =
            ((Vector2)mouseWorldPosition - (Vector2)transform.position).normalized;

        Vector2 origin = (Vector2)transform.position + direction * 1f;

        Debug.DrawRay(origin, direction * lightDistance, Color.yellow);

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            direction,
            lightDistance,
            interactableLayer
        );

        if (hit.collider != null)
        {
            Debug.Log("Acertou: " + hit.collider.name);

            LightInteractable interactable =
                hit.collider.GetComponentInParent<LightInteractable>();

            if (interactable != null)
            {
                interactable.OnLightHit();
            }
            else
            {
                Debug.LogWarning("Objeto não possui LightInteractable!");
            }
        }
    }
}