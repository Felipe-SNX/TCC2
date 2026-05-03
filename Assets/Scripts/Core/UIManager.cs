using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Componentes de UI")]
    [SerializeField] private GameObject modal;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private float displayTime = 3f;

    private Coroutine currentRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (modal != null) modal.SetActive(false);
    }

    public void ShowMessage(string msg)
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }

        currentRoutine = StartCoroutine(ShowMessageRoutine(msg));
    }

    private IEnumerator ShowMessageRoutine(string msg)
    {
        if (modal == null || messageText == null) yield break;

        messageText.text = msg;
        modal.SetActive(true);

        yield return new WaitForSeconds(displayTime);

        modal.SetActive(false);
        currentRoutine = null;
    }
}