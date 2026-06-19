using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ConfigController : MonoBehaviour, IMenuPopup
{
    public Action AoFechar { get; set; }

    [Header("Sub-Sistemas")]
    public ConfigAudioView viewAudio;
    public ConfigControlsView viewControles;
    public ConfigGraphicsView viewGraficos;

    [Header("Animação de Pulso")]
    public float velocidadePulso = 5f;

    [Header("Animação de Carregamento")]
    public float velocidadePreenchimento = 50f; 
    private float progressoPreenchimento = 0f;
    
    private VisualElement root;
    private Label tituloAba;
    private VisualElement conteudoAudio;
    private VisualElement conteudoControles;
    private VisualElement conteudoGraficos;
    private Button btnAudio;
    private Button btnControles;
    private Button btnGraficos;

    // --- Referências de Animação ---
    private VisualElement mascaraTitulo;
    private List<VisualElement> elementosOpacidade = new List<VisualElement>();
    private List<VisualElement> bordasGraficos = new List<VisualElement>();
    private List<VisualElement> linhasBotoes = new List<VisualElement>();

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        // 1. Buscando Elementos Base
        tituloAba = root.Q<Label>("titulo-aba");
        conteudoAudio = root.Q<VisualElement>("conteudo-audio");
        conteudoControles = root.Q<VisualElement>("conteudo-controles");
        conteudoGraficos = root.Q<VisualElement>("conteudo-graficos");

        ScrollView listaControles = root.Q<ScrollView>("lista-controles-container");

        // 2. Inicializando Sub-Views
        if (viewControles != null && listaControles != null)
        {
            viewControles.Inicializar(listaControles);
        }

        btnAudio = root.Q<Button>("btn-aba-audio");
        btnControles = root.Q<Button>("btn-aba-controles");
        btnGraficos = root.Q<Button>("btn-aba-graficos");
        Button btnVoltar = root.Q<Button>("btn-voltar");

        if (viewAudio != null && conteudoAudio != null) viewAudio.Inicializar(conteudoAudio);
        if (viewControles != null && conteudoControles != null) viewControles.Inicializar(conteudoControles);
        if (viewGraficos != null && conteudoGraficos != null) viewGraficos.Inicializar(conteudoGraficos);

        // 3. Configurando Eventos de Clique
        if (btnAudio != null) btnAudio.clicked += MostrarAbaAudio;
        if (btnControles != null) btnControles.clicked += MostrarAbaControles;
        if (btnGraficos != null) btnGraficos.clicked += MostrarAbaGraficos;
        if (btnVoltar != null) btnVoltar.clicked += FecharTela; 

        // 4. Preparando Elementos para Animação Pulsante
        ConfigurarAnimacaoCromatica();

        MostrarAbaAudio();

        progressoPreenchimento = 0f;
        if (mascaraTitulo != null) mascaraTitulo.style.width = Length.Percent(0);
    }

    void Start()
    {
        // Mantendo a arquitetura do Singleton intacta conforme suas prioridades técnicas
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ConnectButtons(root);
        }
    }

    private void Update()
    {
        if (root == null || root.style.display == DisplayStyle.None) return;

        // Cria uma onda matemática suave que oscila entre 0 e 1
        float pulso = (Mathf.Sin(Time.time * velocidadePulso) + 1f) / 2f; 
        
        // Limita a opacidade para que os elementos não sumam completamente
        float opacidadeForte = Mathf.Lerp(0.4f, 1f, pulso);
        float opacidadeFraca = Mathf.Lerp(0.2f, 0.8f, pulso);

        // 1. Pisca a opacidade dos Sliders (a cor já foi definida no CSS/Sub-View)
        foreach (var el in elementosOpacidade)
        {
            if (el != null) el.style.opacity = opacidadeForte;
        }

        // 2. Pisca as linhas dos botões selecionados (Fixo em Vermelho)
        Color vermelhoPulso = new Color(1f, 0.2f, 0.2f, opacidadeForte);
        foreach (var linha in linhasBotoes)
        {
            if (linha != null) linha.style.backgroundColor = vermelhoPulso;
        }

        // 3. Pisca as bordas dos Dropdowns de Gráficos (Fixo em Ciano)
        Color cianoPulso = new Color(0f, 1f, 1f, opacidadeFraca);
        foreach (var borda in bordasGraficos)
        {
            if (borda != null) 
            {
                // Alterado para modificar estritamente a borda inferior, preservando o estilo limpo
                borda.style.borderBottomColor = cianoPulso;
                
                // Garante que as outras direções permaneçam transparentes/zeradas
                borda.style.borderTopColor = Color.clear;
                borda.style.borderLeftColor = Color.clear;
                borda.style.borderRightColor = Color.clear;
            }
        }

        // 4. Máscara do título "Configurações"
        if (mascaraTitulo != null && progressoPreenchimento < 100f)
        {
            progressoPreenchimento += Time.deltaTime * velocidadePreenchimento;
            mascaraTitulo.style.width = Length.Percent(Mathf.Clamp(progressoPreenchimento, 0, 100));
        }
    }

    private void ConfigurarAnimacaoCromatica()
    {
        mascaraTitulo = root.Q<VisualElement>("mascara-titulo");

        // Busca apenas as tags puras que definem O QUE vai piscar, sem se importar com a cor
        elementosOpacidade = root.Query<VisualElement>(className: "elemento-pulsante").ToList();
        bordasGraficos = root.Query<VisualElement>(className: "borda-cromatica").ToList();
        linhasBotoes = root.Query<VisualElement>("linha-inferior").ToList();
    }

    private void FecharTela()
    {
        AoFechar?.Invoke(); 
        gameObject.SetActive(false);
    }

    private void MostrarAbaAudio()
    {
        EsconderAbas();
        tituloAba.text = "Áudio";
        conteudoAudio.style.display = DisplayStyle.Flex;
        btnAudio.AddToClassList("botao-ativo");
    }

    private void MostrarAbaControles()
    {
        EsconderAbas();
        tituloAba.text = "Controles";
        conteudoControles.style.display = DisplayStyle.Flex;
        btnControles.AddToClassList("botao-ativo");
    }

    private void MostrarAbaGraficos()
    {
        EsconderAbas();
        tituloAba.text = "Gráficos";
        conteudoGraficos.style.display = DisplayStyle.Flex;
        btnGraficos.AddToClassList("botao-ativo");
    }

    private void EsconderAbas()
    {
        conteudoAudio.style.display = DisplayStyle.None;
        conteudoControles.style.display = DisplayStyle.None;
        conteudoGraficos.style.display = DisplayStyle.None;

        btnGraficos.RemoveFromClassList("botao-ativo");
        btnControles.RemoveFromClassList("botao-ativo");
        btnAudio.RemoveFromClassList("botao-ativo");
    }
}