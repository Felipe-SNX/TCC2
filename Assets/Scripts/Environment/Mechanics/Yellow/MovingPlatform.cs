using UnityEngine;

public class MovingPlatform : LightInteractable
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float targetHeight = 3f;
    [SerializeField] private bool moveDown = false; // Flexibilidade para subir ou descer

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        startPosition = transform.position;
        
        // Calcula o destino baseado na altura e direção
        float direction = moveDown ? -1f : 1f;
        targetPosition = startPosition + Vector3.up * (targetHeight * direction);
    }

    void Update()
    {
        if (isMoving)
        {
            MovePlatform();
        }
    }

    private void MovePlatform()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        // Se chegou no destino, para de processar o movimento
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            isMoving = false;
            Debug.Log("Plataforma chegou ao destino.");
        }
    }

    // Sobrescrita do método da base
    public override void OnLightHit()
    {
        if (isMoving || transform.position == targetPosition) return;

        Debug.Log("Energia recebida: Movendo plataforma...");
        isMoving = true;
        
        // Chamar a base se quiser manter o Log ou lógica de cor da base
        base.OnLightHit(); 
    }
}