using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightController : MonoBehaviour
{
    [Header("Configurações da Lanterna")]
    [SerializeField] private float lightDistance = 5f;
    [SerializeField] private float originOffset = 1f; // Distância da origem para não colidir com o próprio player
    [SerializeField] private LayerMask interactableLayer;

    private Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Melhoria: wasPressedThisFrame evita processamento desnecessário se segurar a tecla
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            ShootLight();
        }
    }

    void ShootLight()
    {
        if (mainCamera == null) return;

        // Cálculo da direção baseada no Mouse
        Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f;

        Vector2 direction = ((Vector2)mouseWorldPosition - (Vector2)transform.position).normalized;
        Vector2 origin = (Vector2)transform.position + direction * originOffset;

        // Debug visual no Editor
        Debug.DrawRay(origin, direction * lightDistance, Color.yellow, 0.5f);

        // Execução do Raycast
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, lightDistance, interactableLayer);

        if (hit.collider != null)
        {
            // Tenta pegar o componente no objeto atingido ou nos pais dele
            if (hit.collider.TryGetComponent<LightInteractable>(out LightInteractable interactable) || 
                hit.collider.GetComponentInParent<LightInteractable>() != null)
            {
                interactable = interactable ?? hit.collider.GetComponentInParent<LightInteractable>();
                interactable.OnLightHit();
            }
        }
    }
}