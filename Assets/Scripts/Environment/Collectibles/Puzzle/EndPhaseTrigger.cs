using UnityEngine;

public class EndPhaseTrigger : MonoBehaviour
{
    [Header("Puzzle")]
    [SerializeField] private MessagePuzzleUI puzzleUI;
    [SerializeField] private int totalFragmentsRequired = 3;

    [Header("Fim de fase")]
    [SerializeField] private GameObject finalPhaseUI;

    public float Distance; 
    public float moveSpeed = 1.0f; 
    public float originalY; 
    private bool puzzleOpened = false;
    private bool phaseCompleted = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (phaseCompleted || puzzleOpened)
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

        if (UIAudioManager.Instance != null)
        {
            UIAudioManager.Instance.PlayEndPhase();
        }

        if (finalPhaseUI != null)
        {
            finalPhaseUI.SetActive(true);
        }
    }

    private void Start()
    {
        originalY = transform.position.y;
    }
    private void Update()
    {
        float newY = originalY + Mathf.Sin(Time.time * moveSpeed) * Distance;
        transform.position = new Vector2(transform.position.x, newY);
    }
}