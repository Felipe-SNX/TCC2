using UnityEngine;
using UnityEngine.UIElements;

public class StartScreenController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuObject; 
    
    private VisualElement root;
    private VisualElement titleMask;
    private Label coloredTitle;
    private Label promptText;
    private Color[] paleta = new Color[] { Color.green, Color.blue, Color.yellow, Color.red };
    private int currentColorIndex = 0;
    private float timeColor = 0;
    private float fillDuration = 0;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        titleMask = root.Q<VisualElement>("mascara-titulo");
        coloredTitle = root.Q<Label>("titulo-frente");
        promptText = root.Q<Label>("prompt-text");
    }

    private void Update()
    {
        timeColor += Time.deltaTime * 1.5f;
        if (timeColor >= 1f) { timeColor = 0; currentColorIndex = (currentColorIndex + 1) % paleta.Length; }
        
        Color corFinal = Color.Lerp(paleta[currentColorIndex], paleta[(currentColorIndex + 1) % paleta.Length], timeColor);
        if (coloredTitle != null) coloredTitle.style.color = corFinal;

        fillDuration += Time.deltaTime * 45f;
        if (fillDuration > 120f) fillDuration = 0;
        if (titleMask != null) titleMask.style.width = Length.Percent(Mathf.Clamp(fillDuration, 0, 100));

        if (promptText != null)
        {
            float alpha = (Mathf.Sin(Time.time * 3f) + 1f) / 2f; 
            promptText.style.opacity = Mathf.Lerp(0.2f, 0.8f, alpha);
        }

        if (Input.anyKeyDown)
        {
            ProsseguirParaMenu();
        }
    }

    private void ProsseguirParaMenu()
    {
        // Desativa esta tela e ativa o Menu Principal
        mainMenuObject.SetActive(true);
        gameObject.SetActive(false);
    }
}