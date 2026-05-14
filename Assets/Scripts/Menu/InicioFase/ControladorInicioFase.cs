using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class ControladorInicioFase : MonoBehaviour
{
    [Header("Configurações da Fase")]
    [Tooltip("Escreva o nome da fase que aparecerá na tela inicial")]
    public string nomeDaFase = "Fase 1: O Início";

    private Label txtNomeFase;
    private Label txtAnuncio;
    private VisualElement root;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        
        txtNomeFase = root.Q<Label>("txt-nome-fase");
        txtAnuncio = root.Q<Label>("txt-anuncio");

        if (txtNomeFase != null)
        {
            txtNomeFase.text = nomeDaFase.ToUpper(); 
        }

        Time.timeScale = 0f; 
        StartCoroutine(SequenciaReadyGo());
    }

    private IEnumerator SequenciaReadyGo()
    {
        txtAnuncio.text = "PREPARA";
        txtAnuncio.style.color = new StyleColor(Color.white);
        
        yield return new WaitForSecondsRealtime(1.5f); 

        txtAnuncio.text = "VAI!";
        
        Color corAmarela;
        ColorUtility.TryParseHtmlString("#FFD700", out corAmarela);
        
        txtAnuncio.style.color = new StyleColor(corAmarela);
        
        txtAnuncio.style.scale = new StyleScale(new Vector2(1.1f, 1.1f));

        yield return new WaitForSecondsRealtime(1f);

        root.style.display = DisplayStyle.None; 
        Time.timeScale = 1f; 
    }
}