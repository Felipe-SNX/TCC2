using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject modal;
    public TextMeshProUGUI messageText;
    public float displayTime = 3f;

    private Coroutine currentRoutine;

    private void Awake()
    {
        // Garante apenas uma instância
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ShowMessage(string msg)
    {
        if (currentRoutine != null)
        {
            //Debug.Log("Mostrando mensagem: " + msg);
            StopCoroutine(currentRoutine);
        }

        currentRoutine = StartCoroutine(ShowMessageRoutine(msg));
    }

    private IEnumerator ShowMessageRoutine(string msg)
    {
        if (modal == null || messageText == null)
        {
            //Debug.LogError("UIManager não está configurado no Inspector!");
            yield break;
        }

        modal.SetActive(true);
        messageText.text = msg;

        yield return new WaitForSeconds(displayTime);

        modal.SetActive(false);
    }
}