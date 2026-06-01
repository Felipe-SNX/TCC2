using System;
using UnityEngine;
using UnityEngine.InputSystem; 
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;
using UnityEngine.EventSystems; 

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

    private void OnEnable()
    {
        controles.Enable(); // Liga os controles

        root = GetComponent<UIDocument>().rootVisualElement;
        
        btnRetomar = root.Q<Button>("btn-retomar");
        btnReset = root.Q<Button>("btn-reset");
        btnConfig = root.Q<Button>("btn-config");
        btnMenu = root.Q<Button>("btn-menu");

        ConfigurarBotao(btnRetomar, RetomarJogo);
        ConfigurarBotao(btnReset, ReiniciarFase);
        ConfigurarBotao(btnConfig, AbrirConfiguracoes);
        ConfigurarBotao(btnMenu, VoltarMenuPrincipal);

        root.style.display = DisplayStyle.None;
    }

    private void OnDisable()
    {
        controles.Disable(); // Desliga os controles
        
        if (btnReset != null) btnReset.clicked -= ReiniciarFase;
    }

    private void ConfigurarBotao(Button botao, Action acao)
    {
        if (botao == null) return;
        
        botao.clicked += acao;
        botao.RegisterCallback<NavigationSubmitEvent>(evt => acao.Invoke());
    }

    private IEnumerator FocarAposDesenhar(Button botao)
    {
        yield return new WaitForSecondsRealtime(0.05f);
        
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

        if (btnRetomar != null)
        {
            StartCoroutine(FocarAposDesenhar(btnRetomar));
        }
    }

    private void DefinirFocoInicial(GeometryChangedEvent evt)
    {
        btnRetomar.Focus();
        btnRetomar.UnregisterCallback<GeometryChangedEvent>(DefinirFocoInicial);
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
            MetricsManager.Instance.RegistrarTentativas(); 
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