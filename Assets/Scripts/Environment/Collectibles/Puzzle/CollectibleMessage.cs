using UnityEngine;

public class CollectibleMessage : MonoBehaviour
{
    [Header("Fragmento da Mensagem")]
    [SerializeField] private MessageFragment fragment;

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

        if (UIAudioManager.Instance != null)
        {
            UIAudioManager.Instance.PlayCollectMessage();
        }

        Debug.Log("Fragmento coletado e removido da cena.");

        Destroy(gameObject);
    }
}