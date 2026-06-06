using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FinishFlag : MonoBehaviour
{
    [Header("Interface")]
    [SerializeField] private GameObject questionnaireScreen;

    private bool completedLevel = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !completedLevel)
        {
            FinishLevel();
        }
    }

    private void FinishLevel()
    {
        completedLevel = true;

        if (MetricsManager.Instance != null)
        {
            MetricsManager.Instance.FinishLevelAndFreezeData();
        }

        if (PlayerState.Instance != null)
        {
            PlayerState.Instance.PauseMovement();
        }

        Rigidbody2D rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (questionnaireScreen != null)
        {
            questionnaireScreen.SetActive(true);
        }
    }
}