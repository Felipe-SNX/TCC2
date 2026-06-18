using System;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UIElements;

public class LevelSelectionController : MonoBehaviour, IMenuPopup
{
    public Action AoFechar { get; set; }

    [Header("Animação de Carregamento")]
    public float velocidadePreenchimento = 50f; 
    private float progressoPreenchimento = 0f;
    
    private VisualElement root;
    private VisualElement mascaraTitulo;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        progressoPreenchimento = 0f;
        mascaraTitulo = root.Q<VisualElement>("mascara-titulo");
        if (mascaraTitulo != null) mascaraTitulo.style.width = Length.Percent(0);

        // Limpa o foco inicial por segurança
        root.schedule.Execute(() => {
            var elementoFocado = root.panel?.focusController?.focusedElement as VisualElement;
            if (elementoFocado != null)
            {
                elementoFocado.Blur();
            }
        }).StartingIn(10);

        Button btnVerde = root.Q<Button>("btn-fase-verde");
        Button btnVermelha = root.Q<Button>("btn-fase-vermelha");
        Button btnAmarela = root.Q<Button>("btn-fase-amarela");
        Button btnVoltar = root.Q<Button>("btn-voltar");

        ConfigurarBotao(btnVerde, () => CarregarFase("Map_Green"));
        ConfigurarBotao(btnVermelha, () => CarregarFase("Map_Red"));
        ConfigurarBotao(btnAmarela, () => CarregarFase("Map_Yellow"));

        btnVerde?.RegisterCallback<GeometryChangedEvent>(evt => btnVerde.Focus());

        if (btnVoltar != null) btnVoltar.clicked += FecharTela;
    }

    void Start()
    {
        if (AudioManager.Instance != null && root != null)
        {
            AudioManager.Instance.ConnectButtons(root);
        }
    }

    private void Update()
    {
        if (root == null || root.style.display == DisplayStyle.None) return;

        if (mascaraTitulo != null && progressoPreenchimento < 100f)
        {
            progressoPreenchimento += Time.deltaTime * velocidadePreenchimento;
            mascaraTitulo.style.width = Length.Percent(Mathf.Clamp(progressoPreenchimento, 0, 100));
        }
    }

    private void ConfigurarBotao(Button botao, Action acao)
    {
        if (botao == null) return;
        botao.clicked += acao;
        botao.RegisterCallback<NavigationSubmitEvent>(evt => acao.Invoke());
    }

    private void CarregarFase(string nomeDaCena)
    {
        Debug.Log($"Iniciando a cena: {nomeDaCena}");
        GlobalData.nextScene = nomeDaCena;        
        SceneManager.LoadScene("Loading");
    }

    private void FecharTela()
    {
        AoFechar?.Invoke();
        gameObject.SetActive(false);
    }
}