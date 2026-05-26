using UnityEngine;

public class CameraLookController : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private Transform cameraTarget;

    [Header("Configurações de Visão")]
    [SerializeField] private float lookDistance = 4f; // Quão longe a câmera pode ir
    [SerializeField] private float smoothSpeed = 3f;  // Velocidade do movimento 

    private InputSystem_Actions controls;
    private Vector2 lookInput;
    private Vector2 defaultLocalPosition;

    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        controls = new InputSystem_Actions();
        
        // Lê os direcionais ou o mouse (retorna um Vector2 com X e Y)
        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        
        // Quando soltar o botão, zera o vetor para a câmera voltar ao centro
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        if (cameraTarget != null)
        {
            // Salva a posição inicial (centro do personagem)
            defaultLocalPosition = cameraTarget.localPosition;
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        if (cameraTarget == null) return;

        Vector2 currentLook = lookInput;

        // Correção para que não inverta os controles ao personagem virar
        currentLook.x *= Mathf.Sign(transform.localScale.x);
        
        // Verifica se o jogador está tentando andar
        if (playerMovement != null && Mathf.Abs(playerMovement.MoveInput) > 0.1f)
        {
            // Força a câmera a ignorar o direcional de olhar e voltar para o centro
            currentLook = Vector2.zero;
        }

        // Calcula a posição desejada: Centro + (Direção * Distância máxima)
        Vector2 targetPos = defaultLocalPosition + (currentLook.normalized * lookDistance);

        // Move o CameraTarget suavemente da posição atual até a posição desejada usando Lerp
        cameraTarget.localPosition = Vector2.Lerp(cameraTarget.localPosition, targetPos, Time.deltaTime * smoothSpeed);
    }
}
