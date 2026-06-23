using UnityEngine;

public class CollectibleMessage : MonoBehaviour
{
    [Header("Fragmento da Mensagem")]
    [SerializeField] private MessageFragment fragment;

    [Header("Configuração de limpeza")]
    [SerializeField] private float destroyDistanceBehindCamera = 10f;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (MessageManager.Instance == null)
        {
            Debug.LogError("MessageManager.Instance está NULL!");
            return;
        }

        if (fragment == null || string.IsNullOrEmpty(fragment.text))
        {
            Debug.LogWarning("Fragmento da mensagem não configurado neste coletável.");
            return;
        }

        MessageManager.Instance.CollectFragment(fragment);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayCollectMessage();
        }

        Destroy(gameObject);
    }

    private void Update()
    {
        if (mainCamera == null)
            return;

        if (transform.position.y < mainCamera.transform.position.y - destroyDistanceBehindCamera)
        {
            Destroy(gameObject);
        }
    }
}