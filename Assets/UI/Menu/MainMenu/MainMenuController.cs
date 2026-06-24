using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.UI.Menu.MainMenu
{
    public class MainMenuController : MonoBehaviour, IMenuPopup
    {
        public Action AoFechar { get; set; }
        
        [Header("Telas Secundárias")]
        [SerializeField] private GameObject objetoConfiguracoes;
        [SerializeField] private GameObject objetoSelecaoFases;
        [SerializeField] private GameObject objetoCreditos;
        
        [Header("Configurações de Cor e Animação")]
        public float velocidadeTrocaDeCor = 0.4f; 
        public float velocidadePreenchimento = 50f;
        
        private Button btnJogar;
        private VisualElement root;

        private VisualElement mascaraTitulo;
        private Label tituloColorido;
        
        private readonly Color[] paleta = new Color[] { Color.green, Color.blue, Color.yellow, Color.red };
        private int indiceCorAtual = 0;
        private float progressoTransicaoCor = 0f;
        
        private float progressoPreenchimento = 0f;
        private bool tituloCarregado = false; 

        private void OnEnable()
        {
            root = GetComponent<UIDocument>().rootVisualElement;

            var telaPrincipal = root.Q<VisualElement>("painel-principal");
            var containerBotoes = root.Q<VisualElement>(className: "container-botoes");

            if (telaPrincipal != null) telaPrincipal.AddToClassList("tela-escondida");
            if (containerBotoes != null) containerBotoes.AddToClassList("botoes-escondidos");

            root.schedule.Execute(() => {
                if (telaPrincipal != null) telaPrincipal.RemoveFromClassList("tela-escondida");
                if (containerBotoes != null) containerBotoes.RemoveFromClassList("botoes-escondidos");
            }).StartingIn(50);

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

            mascaraTitulo = root.Q<VisualElement>("mascara-titulo");
            tituloColorido = root.Q<Label>("titulo-frente");

            progressoPreenchimento = 0f;
            tituloCarregado = false;
            if (mascaraTitulo != null) mascaraTitulo.style.width = Length.Percent(0);
        }

        private void Update()
        {
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

            if (!tituloCarregado && mascaraTitulo != null)
            {
                progressoPreenchimento += Time.deltaTime * velocidadePreenchimento;
                mascaraTitulo.style.width = Length.Percent(Mathf.Clamp(progressoPreenchimento, 0, 100));

                if (progressoPreenchimento >= 100f)
                {
                    tituloCarregado = true;
                }
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
}