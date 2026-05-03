using UnityEngine;

public class CollectibleMessage : MonoBehaviour
{
    [TextArea(3, 10)] // Melhora a visualização no Inspector
    [SerializeField] private string messagePart;

    private Camera mainCamera;

    private void Start()
    {
        // Cache da câmera para evitar busca no Update
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Verifica se o MessageManager existe antes de chamar
            if (MessageManager.Instance != null)
            {
                MessageManager.Instance.Collect(messagePart);
                Destroy(gameObject);
            }
        }
    }

    void Update()
    {
        // Checa se a câmera foi encontrada
        if (mainCamera == null) return;

        // Garbage Collection manual
        if (transform.position.y < mainCamera.transform.position.y - 10f)
        {
            Destroy(gameObject);
        }
    }
}