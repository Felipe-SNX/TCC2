using UnityEngine;

public class EndPhaseTrigger : MonoBehaviour
{
    [Header("Puzzle")]
    public MessagePuzzleUI puzzleUI;
    public int totalFragmentsRequired = 3;

    [Header("Fim de fase")]
    [SerializeField] private GameObject finalPhaseUI;

    private bool puzzleOpened = false;
    private bool phaseCompleted = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (phaseCompleted)
            return;

        if (puzzleOpened)
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

        puzzleOpened = true;
        puzzleUI.OpenPuzzle(MessageManager.Instance.GetCollectedFragments());
    }

    public void CompletePhase()
    {
        if (phaseCompleted)
            return;

        phaseCompleted = true;

        Debug.Log("Fim da fase acionado!");

        Time.timeScale = 1f;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayEndPhase();
        }

        if (finalPhaseUI != null)
        {
            finalPhaseUI.SetActive(true);
        }
    }
}