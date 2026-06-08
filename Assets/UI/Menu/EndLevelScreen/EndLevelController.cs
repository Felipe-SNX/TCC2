using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class EndLevelController : MonoBehaviour
{
    private VisualElement root;
    private Button btnReset;
    private Button btnVoltarMenu;

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null) return;
        
        root = uiDocument.rootVisualElement;

        var txtNomeFase = root.Q<Label>("txt-nome-fase");
        var txtTempoTotal = root.Q<Label>("txt-tempo-total");
        var txtTentativas = root.Q<Label>("txt-tentativas");
        
        btnReset = root.Q<Button>("btn-reset");
        btnVoltarMenu = root.Q<Button>("btn-voltar-menu");

        if (MetricsManager.Instance != null)
        {
            float tempo = MetricsManager.Instance.GetTimeLevel(); 
            int minutos = Mathf.FloorToInt(tempo / 60F);
            int segundos = Mathf.FloorToInt(tempo - minutos * 60);
            string tempoFormatado = string.Format("{0:00}:{1:00}", minutos, segundos);

            if (txtNomeFase != null) txtNomeFase.text = MetricsManager.Instance.GetNameLevel(); 
            if (txtTempoTotal != null) txtTempoTotal.text = "Tempo: " + tempoFormatado;
            if (txtTentativas != null) txtTentativas.text = "Tentativas: " + MetricsManager.Instance.GetTriesLevel();
        }

        if (btnReset != null) btnReset.clicked += RetryLevel;
        if (btnVoltarMenu != null) btnVoltarMenu.clicked += BackToMenu;
    }

    private void RetryLevel()
    {
        if (MetricsManager.Instance != null)
        {
            MetricsManager.Instance.CountTries(); 
        }
        
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main_Menu"); 
    }

    private void OnDisable()
    {
        if (btnReset != null) btnReset.clicked -= RetryLevel;
        if (btnVoltarMenu != null) btnVoltarMenu.clicked -= BackToMenu;
    }
}
