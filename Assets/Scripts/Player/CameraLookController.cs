using UnityEngine;

[RequireComponent(typeof(PlayerInputController))]
public class CameraLookController : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private Transform cameraTarget;

    [Header("Configurações de Visão")]
    [SerializeField] private float lookDistance = 4f;
    [SerializeField] private float smoothSpeed = 3f;

    private PlayerInputController input;
    private Vector2 defaultLocalPosition;

    private void Awake()
    {
        input = GetComponent<PlayerInputController>();

        if (cameraTarget != null)
        {
            defaultLocalPosition = cameraTarget.localPosition;
        }
    }

    private void Update()
    {
        if (cameraTarget == null) return;

        Vector2 currentLook = input.LookVector; 

        currentLook.x *= Mathf.Sign(transform.localScale.x);
        
        if (input.MoveVector.magnitude > 0.1f)
        {
            currentLook = Vector2.zero;
        }

        Vector2 targetPos = defaultLocalPosition + (currentLook.normalized * lookDistance);

        cameraTarget.localPosition = Vector2.Lerp(cameraTarget.localPosition, targetPos, Time.deltaTime * smoothSpeed);
    }
}