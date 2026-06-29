using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;
using UnityEngine.EventSystems;
using Assets.UI.Menu.ConfigScreen;

namespace Assets.UI.Menu.PauseMenu
{
    public class PauseController : MonoBehaviour
    {
        public static PauseController Instancia { get; private set; }

        [SerializeField] private GameObject objetoConfiguracoes;
        
        private VisualElement root;
        public bool JogoPausado { get; private set; } = false;

        private Button btnRetomar;
        private Button btnReset;
        private Button btnConfig;
        private Button btnMenu;
        private InputSystem_Actions controles;

        [Header("Animação do Título")]
        public float velocidadePreenchimento = 50f;
        private float progressoPreenchimento = 0f;
        private VisualElement mascaraTitulo;

        private void Awake()
        {
            if (Instancia != null && Instancia != this)
            {
                Destroy(gameObject);
                return;
            }
            Instancia = this;

            controles = new InputSystem_Actions();
            controles.Player.Pause.performed += ctx => AlternarPause();
        }

        void Start()
        {
            if (UIAudioManager.Instance != null)
            {
                UIAudioManager.Instance.ConnectButtons(root);
            }
        }

        private void OnEnable()
        {
            controles.Enable();

            root = GetComponent<UIDocument>().rootVisualElement;
            
            btnRetomar = root.Q<Button>("btn-retomar");
            btnReset = root.Q<Button>("btn-reset");
            btnConfig = root.Q<Button>("btn-config");
            btnMenu = root.Q<Button>("btn-menu");

            mascaraTitulo = root.Q<VisualElement>("mascara-titulo");

            ConfigurarBotao(btnRetomar, RetomarJogo);
            ConfigurarBotao(btnReset, ReiniciarFase);
            ConfigurarBotao(btnConfig, AbrirConfiguracoes);
            ConfigurarBotao(btnMenu, VoltarMenuPrincipal);

            root.style.display = DisplayStyle.None;
        }

        private void OnDisable()
        {
            controles.Disable();
            if (btnReset != null) btnReset.clicked -= ReiniciarFase;
        }

        private void Update()
        {
            // Só executa a animação se a tela de pause estiver aberta
            if (root == null || root.style.display == DisplayStyle.None) return;

            if (mascaraTitulo != null && progressoPreenchimento < 100f)
            {
                progressoPreenchimento += Time.unscaledDeltaTime * velocidadePreenchimento;
                mascaraTitulo.style.width = Length.Percent(Mathf.Clamp(progressoPreenchimento, 0, 100));
            }
        }

        private void ConfigurarBotao(Button botao, Action acao)
        {
            if (botao == null) return;
            botao.clicked += acao;
            botao.RegisterCallback<NavigationSubmitEvent>(evt => acao.Invoke());
        }

        private IEnumerator FocarAposDesenhar(Button botao)
        {
            yield return new WaitForSecondsRealtime(0.05f); // Uso de Realtime pois o tempo está pausado
            
            if (botao != null)
            {
                if (EventSystem.current != null)
                {
                    EventSystem.current.SetSelectedGameObject(this.gameObject);
                }
                botao.Focus();
            }
        }

        public void AlternarPause()
        {
            if (objetoConfiguracoes != null && objetoConfiguracoes.activeSelf) 
                return;

            if (JogoPausado)
                RetomarJogo();
            else
                PausarJogo();
        }

        private void PausarJogo()
        {
            JogoPausado = true;
            Time.timeScale = 0f; 
            root.style.display = DisplayStyle.Flex; 

            progressoPreenchimento = 0f;
            if (mascaraTitulo != null) mascaraTitulo.style.width = Length.Percent(0);

            if (btnRetomar != null)
            {
                StartCoroutine(FocarAposDesenhar(btnRetomar));
            }
        }

        private void RetomarJogo()
        {
            JogoPausado = false;
            Time.timeScale = 1f; 
            root.style.display = DisplayStyle.None; 
        }

        private void ReiniciarFase()
        {
            if (MetricsManager.Instance != null)
            {
                MetricsManager.Instance.CountTries(); 
            }

            Time.timeScale = 1f;
            string nomeCenaAtual = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(nomeCenaAtual);
            Debug.Log($"[PAUSE] Fase {nomeCenaAtual} reiniciada. Tentativa computada nas métricas!");
        }

        private void AbrirConfiguracoes()
        {
            if (objetoConfiguracoes != null)
            {
                root.style.display = DisplayStyle.None; 
                
                var scriptConfig = objetoConfiguracoes.GetComponent<ConfigController>();
                if (scriptConfig != null)
                {
                    scriptConfig.AoFechar = () => { 
                        root.style.display = DisplayStyle.Flex; 
                        if (btnConfig != null) 
                        {
                            StartCoroutine(FocarAposDesenhar(btnConfig));
                        }
                    };
                }
                objetoConfiguracoes.SetActive(true);
            }
        }

        private void VoltarMenuPrincipal()
        {
            Time.timeScale = 1f; 
            SceneManager.LoadScene("Main_Menu"); 
        }
    }
}