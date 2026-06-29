using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class InvisibleWall : MonoBehaviour
{
    [Header("Puzzle")]
    [SerializeField] private int totalFragmentsRequired = 3;

    [Header("Feedback")]
    [SerializeField]
    [TextArea]
    private string missingFragmentsMessage =
        "Você precisa encontrar todos os fragmentos da mensagem antes de prosseguir.";

    private bool messageShown = false;

    private void Update()
    {
        TryUnlockWall();
    }

    private void TryUnlockWall()
    {
        if (MessageManager.Instance == null)
            return;

        if (MessageManager.Instance.HasAllFragments(totalFragmentsRequired))
        {
            Debug.Log("Todos os fragmentos coletados. Parede removida.");
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (!messageShown)
        {
            messageShown = true;

            UIManager.Instance?.ShowMessage(missingFragmentsMessage);

            Debug.Log("Player tentou passar sem todos os fragmentos.");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        messageShown = false;
    }
}