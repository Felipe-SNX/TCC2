using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour, IMenuPopup
{
    public Action AoFechar { get; set; }
    
    [Header("Telas Secundárias")]
    [SerializeField] private GameObject objetoConfiguracoes;
    [SerializeField] private GameObject objetoSelecaoFases;
    [SerializeField] private GameObject objetoCreditos;
    
    [Header("Configurações de Cor")]
    public float velocidadeTrocaDeCor = 1.5f;
    public float velocidadePreenchimento = 45f;
    
    private Button btnJogar;
    private VisualElement root;

    private VisualElement mascaraTitulo;
    private Label tituloColorido;
    private List<VisualElement> linhasBotoes = new List<VisualElement>();
    private Color[] paleta = new Color[] { Color.green, Color.blue, Color.yellow, Color.red };
    private int indiceCorAtual = 0;
    private float progressoTransicaoCor = 0f;
    private float progressoPreenchimento = 0f;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        // Configuração de Navegação e Cliques
        Button btnOpcoes = root.Q<Button>("btn-opcoes");
        Button btnCreditos = root.Q<Button>("btn-creditos");
        btnJogar = root.Q<Button>("btn-jogar");
        Button btnSair = root.Q<Button>("btn-sair");

        if (btnJogar != null) 
        {
            btnJogar.RegisterCallback<GeometryChangedEvent>(DefinirFocoInicial);
            btnJogar.clicked += () => AbrirTela(objetoSelecaoFases);
        }
        if (btnCreditos != null) btnCreditos.clicked += () => AbrirTela(objetoCreditos);
        if (btnOpcoes != null) btnOpcoes.clicked += () => AbrirTela(objetoConfiguracoes);
        if (btnSair != null) btnSair.clicked += SairDoJogo;

        // Configuração das Referências de Animação
        mascaraTitulo = root.Q<VisualElement>("mascara-titulo");
        tituloColorido = root.Q<Label>("titulo-frente");

        linhasBotoes.Clear();
        var todosBotoes = root.Query<Button>().ToList();
        foreach (var btn in todosBotoes)
        {
            var linha = btn.Q<VisualElement>("linha-inferior");
            if (linha != null)
            {
                linhasBotoes.Add(linha);
            }
        }
    }

    void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ConnectButtons(root);
            AudioManager.Instance.PlayMenuMusic();
        }
    }

    private void Update()
    {
        // Só executa as animações se a tela principal estiver visível
        if (root == null || root.style.display == DisplayStyle.None) return;

        progressoTransicaoCor += Time.deltaTime * velocidadeTrocaDeCor;
        
        if (progressoTransicaoCor >= 1f)
        {
            progressoTransicaoCor = 0f;
            indiceCorAtual = (indiceCorAtual + 1) % paleta.Length;
        }

        int proximoIndice = (indiceCorAtual + 1) % paleta.Length;
        Color corMisturada = Color.Lerp(paleta[indiceCorAtual], paleta[proximoIndice], progressoTransicaoCor);

        if (tituloColorido != null) tituloColorido.style.color = corMisturada;
        foreach (var linha in linhasBotoes)
        {
            linha.style.backgroundColor = corMisturada;
        }

        if (mascaraTitulo != null)
        {
            progressoPreenchimento += Time.deltaTime * velocidadePreenchimento;
            
            if (progressoPreenchimento > 120f) 
            {
                progressoPreenchimento = 0f; 
            }

            mascaraTitulo.style.width = Length.Percent(Mathf.Clamp(progressoPreenchimento, 0, 100));
        }
    }

    private void DefinirFocoInicial(GeometryChangedEvent evt)
    {
        btnJogar.Focus();
        btnJogar.UnregisterCallback<GeometryChangedEvent>(DefinirFocoInicial);
    }

    private void AbrirTela(GameObject objetoFase)
    {
        if (objetoFase != null)
        {
            var popup = objetoFase.GetComponent<IMenuPopup>();

            if (popup != null)
            {
                root.style.display = DisplayStyle.None;

                popup.AoFechar = () => { 
                    root.style.display = DisplayStyle.Flex; 
                    
                    // Retorna o foco para o botão de jogar ao fechar o popup
                    if (btnJogar != null) btnJogar.Focus();
                };

                objetoFase.SetActive(true);
            }
            else 
            {
                Debug.LogError($"O objeto {objetoFase.name} não tem um script que implementa IMenuPopup!");
            }
        }
    }

    private void SairDoJogo()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}