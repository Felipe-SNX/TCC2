using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance { get; private set; }

    [SerializeField] private List<string> collectedParts = new List<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Debug.Log("Instance definido corretamente"); 
    }

    public void Collect(string part)
    {
        if (string.IsNullOrEmpty(part)) return;

        if (!collectedParts.Contains(part))
        {
            collectedParts.Add(part);
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowMessage(part);
        }
        else
        {
            Debug.LogWarning("UIManager não encontrado!");
        }
    }

    public string GetFullMessage()
    {
        // Une as partes coletadas em um único texto formatado
        return string.Join("\n", collectedParts); 
    }

    // Método para limpar as mensagens ao reiniciar o jogo ou fase
    public void ClearMessages()
    {
        collectedParts.Clear();
    }
}