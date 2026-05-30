using UnityEngine;

public class EndPhaseTrigger : MonoBehaviour
{
    public MessagePuzzleUI puzzleUI;
    public int totalFragmentsRequired = 3;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (MessageManager.Instance == null)
        {
            Debug.LogError("MessageManager não encontrado!");
            return;
        }

        if (!MessageManager.Instance.HasAllFragments(totalFragmentsRequired))
        {
            Debug.Log("Você ainda não coletou todos os fragmentos.");
            return;
        }

        puzzleUI.OpenPuzzle(MessageManager.Instance.GetCollectedFragments());
    }
}