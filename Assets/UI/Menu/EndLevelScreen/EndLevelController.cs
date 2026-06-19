using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class EndLevelController : MonoBehaviour
{
    private VisualElement root;
    private VisualElement mascaraTitulo;
    private float progressoPreenchimento = 0f;
    [SerializeField] private float velocidadePreenchimento = 50f;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        mascaraTitulo = root.Q<VisualElement>("mascara-titulo");

        // Preenche dados
        var txtNomeFase = root.Q<Label>("txt-nome-fase");
        var txtTempoTotal = root.Q<Label>("txt-tempo-total");
        var txtTentativas = root.Q<Label>("txt-tentativas");
        var txtColetaveis = root.Q<Label>("txt-coletaveis");
        
        if (MetricsManager.Instance != null)
        {
            float tempo = MetricsManager.Instance.GetTimeLevel(); 
            string tempoFormatado = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(tempo / 60F), Mathf.FloorToInt(tempo % 60));

            if (txtNomeFase != null) txtNomeFase.text = MetricsManager.Instance.GetNameLevel(); 
            if (txtTempoTotal != null) txtTempoTotal.text = "Tempo: " + tempoFormatado;
            if (txtTentativas != null) txtTentativas.text = "Tentativas: " + MetricsManager.Instance.GetTriesLevel();
            if (txtColetaveis != null) txtColetaveis.text = "Coletáveis: " + MetricsManager.Instance.GetCollectiblesCount();
        }

        root.Q<Button>("btn-reset").clicked += RetryLevel;
        root.Q<Button>("btn-voltar-menu").clicked += BackToMenu;

        progressoPreenchimento = 0f;
    }

    private void Update()
    {
        if (mascaraTitulo != null && progressoPreenchimento < 100f)
        {
            progressoPreenchimento += Time.deltaTime * velocidadePreenchimento;
            mascaraTitulo.style.width = Length.Percent(Mathf.Clamp(progressoPreenchimento, 0, 100));
        }
    }

    private void RetryLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    private void BackToMenu() => SceneManager.LoadScene("Main_Menu");
}