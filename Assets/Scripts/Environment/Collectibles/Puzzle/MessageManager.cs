using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance;

    private List<MessageFragment> collectedFragments = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Debug.Log("MessageManager inicializado.");
    }

    public void CollectFragment(MessageFragment fragment)
    {
        if (fragment == null)
        {
            Debug.LogWarning("Fragmento nulo.");
            return;
        }

        if (string.IsNullOrEmpty(fragment.text))
        {
            Debug.LogWarning("Texto do fragmento vazio.");
            return;
        }

        if (!collectedFragments.Exists(f => f.correctIndex == fragment.correctIndex))
        {
            collectedFragments.Add(fragment);

            Debug.Log("Fragmento coletado: " + fragment.text);

            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowMessage(fragment.text);
            }
            else
            {
                Debug.LogWarning("UIManager não encontrado na cena.");
            }
        }
    }

    public List<MessageFragment> GetCollectedFragments()
    {
        return collectedFragments;
    }

    public bool HasAllFragments(int totalFragments)
    {
        return collectedFragments.Count >= totalFragments;
    }

    public string GetFullMessage()
    {
        List<MessageFragment> orderedFragments = new(collectedFragments);

        orderedFragments.Sort((a, b) => a.correctIndex.CompareTo(b.correctIndex));

        List<string> parts = new();

        foreach (MessageFragment fragment in orderedFragments)
        {
            parts.Add(fragment.text);
        }

        return string.Join(" ", parts);
    }

    public void ClearFragments()
    {
        collectedFragments.Clear();
    }
}