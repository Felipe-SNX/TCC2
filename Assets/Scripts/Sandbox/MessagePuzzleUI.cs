using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MessagePuzzleUI : MonoBehaviour
{
    public GameObject puzzlePanel;
    public Transform collectedPartsArea;
    public Transform answerSlotsArea;
    public GameObject fragmentButtonPrefab;
    public Button confirmButton;
    public TextMeshProUGUI feedbackText;
    private List<MessageFragment> answerOrder = new List<MessageFragment>();
    public void OpenPuzzle(List<MessageFragment> fragments)
    {
        puzzlePanel.SetActive(true);
        puzzlePanel.transform.SetAsLastSibling();
        Time.timeScale = 0f;
        feedbackText.text = "";
        answerOrder.Clear();
        ClearArea(collectedPartsArea);
        ClearArea(answerSlotsArea);
        List<MessageFragment> shuffled = new List<MessageFragment>(fragments);
        Shuffle(shuffled);
        foreach (MessageFragment fragment in shuffled)
        {
            CreateFragmentButton(fragment, collectedPartsArea);
        }
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(CheckAnswer);
        Debug.Log("Puzzle aberto com " + fragments.Count + " fragmentos.");
    }
    private void CreateFragmentButton(MessageFragment fragment, Transform parent)
    {
        GameObject buttonObject = Instantiate(fragmentButtonPrefab, parent);
        LayoutElement layout = buttonObject.GetComponent<LayoutElement>();
        if (layout == null)
        {
            layout = buttonObject.AddComponent<LayoutElement>();
        }
        layout.preferredWidth = 180f;
        layout.preferredHeight = 50f;
        layout.flexibleWidth = 0f;
        layout.flexibleHeight = 0f;
        TextMeshProUGUI text = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
        text.text = fragment.text;
        text.enableWordWrapping = false;
        text.overflowMode = TextOverflowModes.Overflow;
        text.alignment = TextAlignmentOptions.Center;
        Button button = buttonObject.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            Debug.Log("Fragmento selecionado: " + fragment.text);
            SelectFragment(fragment, buttonObject);
        });
    }
    private void SelectFragment(MessageFragment fragment, GameObject originalButton)
    {
        answerOrder.Add(fragment);
        originalButton.SetActive(false);
        GameObject answerButton = Instantiate(fragmentButtonPrefab, answerSlotsArea);
        LayoutElement layout = answerButton.GetComponent<LayoutElement>();
        if (layout == null)
        {
            layout = answerButton.AddComponent<LayoutElement>();
        }
        layout.preferredWidth = 180f;
        layout.preferredHeight = 50f;
        layout.flexibleWidth = 0f;
        layout.flexibleHeight = 0f;
        TextMeshProUGUI text = answerButton.GetComponentInChildren<TextMeshProUGUI>();
        text.text = fragment.text;
        text.enableWordWrapping = false;
        text.overflowMode = TextOverflowModes.Overflow;
        text.alignment = TextAlignmentOptions.Center;
        Button button = answerButton.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            Debug.Log("Fragmento removido da resposta: " + fragment.text);
            answerOrder.Remove(fragment);
            Destroy(answerButton);
            originalButton.SetActive(true);
        });
    }
    private void CheckAnswer()
    {
        Debug.Log("Botão Confirmar clicado.");
        if (answerOrder.Count == 0)
        {
            feedbackText.text = "Monte a mensagem corretamente.";
            return;
        }
        if (MessageManager.Instance != null)
        {
            int totalCollected = MessageManager.Instance.GetCollectedFragments().Count;
            if (answerOrder.Count < totalCollected)
            {
                feedbackText.text = "Use todos os fragmentos.";
                return;
            }
        }
        for (int i = 0; i < answerOrder.Count; i++)
        {
            if (answerOrder[i].correctIndex != i)
            {
                feedbackText.text = "A mensagem ainda não está correta.";
                return;
            }
        }
        feedbackText.text = "Puzzle resolvido!";
        Debug.Log("Puzzle resolvido!");
        StartCoroutine(ClosePuzzleAfterDelay());
    }
    private IEnumerator ClosePuzzleAfterDelay()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        puzzlePanel.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log("Fim da fase liberado.");
    }
    private void ClearArea(Transform area)
    {
        foreach (Transform child in area)
        {
            Destroy(child.gameObject);
        }
    }
    private void Shuffle(List<MessageFragment> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            MessageFragment temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
