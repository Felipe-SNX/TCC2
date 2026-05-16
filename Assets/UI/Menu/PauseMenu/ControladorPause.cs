using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ControladorPause : MonoBehaviour
{
    public static ControladorPause Instancia { get; private set; }

    [SerializeField] private GameObject objetoConfiguracoes;
    
    private VisualElement root;
    public bool JogoPausado { get; private set; } = false;

    Button btnReset;

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }
        Instancia = this;
    }

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        
        Button btnRetomar = root.Q<Button>("btn-retomar");
        btnReset = root.Q<Button>("btn-reset");
        Button btnConfig = root.Q<Button>("btn-config");
        Button btnMenu = root.Q<Button>("btn-menu");

        if (btnRetomar != null) btnRetomar.clicked += RetomarJogo;
        if (btnReset != null) btnReset.clicked += ReiniciarFase;
        if (btnConfig != null) btnConfig.clicked += AbrirConfiguracoes;
        if (btnMenu != null) btnMenu.clicked += VoltarMenuPrincipal;

        root.style.display = DisplayStyle.None;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AlternarPause();
        }
    }

    public void AlternarPause()
    {
        // Se a tela de configurações estiver aberta, não permite despausar pelo ESC
        if (objetoConfiguracoes != null && objetoConfiguracoes.activeSelf) 
            return;

        if (JogoPausado)
            RetomarJogo();
        else
            PausarJogo();
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

    private void OnDisable()
    {
        if (btnReset != null) btnReset.clicked -= ReiniciarFase;
    }

    private void PausarJogo()
    {
        JogoPausado = true;
        Time.timeScale = 0f; // Congela a física e animações do jogo
        root.style.display = DisplayStyle.Flex; 
    }

    private void RetomarJogo()
    {
        JogoPausado = false;
        Time.timeScale = 1f; // Descongela o jogo
        root.style.display = DisplayStyle.None; 
    }

    private void AbrirConfiguracoes()
    {
        if (objetoConfiguracoes != null)
        {
            root.style.display = DisplayStyle.None; 
            
            var scriptConfig = objetoConfiguracoes.GetComponent<ControladorConfiguracoes>();
            if (scriptConfig != null)
            {
                scriptConfig.AoFechar = () => { root.style.display = DisplayStyle.Flex; };
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