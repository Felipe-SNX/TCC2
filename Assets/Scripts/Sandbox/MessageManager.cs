using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance;

    private List<string> collectedParts = new List<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        //Debug.Log("Instance definido corretamente");
    }

    public void Collect(string part)
    {
        if (string.IsNullOrEmpty(part))
            return;

        // Evita duplicatas (opcional)
        if (!collectedParts.Contains(part))
        {
            collectedParts.Add(part);
        }

        // Segurança ao chamar UI
        if (UIManager.Instance != null)
        {
            //Debug.Log("Mensagem coletada: " + part);
            UIManager.Instance.ShowMessage(part);
        }
        else
        {
            Debug.LogWarning("UIManager não encontrado na cena!");
        }
    }

    public string GetFullMessage()
    {
        return string.Join(" ", collectedParts);
    }
}