using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameplayData
{
    public string faseAtual;
    public float tempoSegundos;
    public int tentativas;          
}

public class MetricsManager : MonoBehaviour
{
    public static MetricsManager Instance;

    [Header("Configuração de Endpoints")]
    [SerializeField] private string urlEndpoint = "https://endpoint";

    // Variáveis internas da fase atual
    private float cronometroFase = 0f;
    private int contadorTentativas = 1;
    private string nomeFaseAtual;
    private bool cronometroAtivo = false;
    private float tempoFinalFase = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (cronometroAtivo)
        {
            cronometroFase += Time.deltaTime;
        }
    }

    private void OnSceneLoaded(Scene cena, LoadSceneMode modo)
    {
        nomeFaseAtual = cena.name;
        cronometroFase = 0f;
        cronometroAtivo = (nomeFaseAtual != "Main_Menu");
    }

    public void RegistrarTentativas()
    {
        contadorTentativas++;
    }

    public void FinalizarFaseECongelarDados()
    {
        cronometroAtivo = false; 
        tempoFinalFase = Mathf.Round(cronometroFase * 100f) / 100f;
        Debug.Log($"[MÉTRICAS] Gameplay congelada! Tempo: {tempoFinalFase}s | Tentativas: {contadorTentativas}");
    }

    public void EnviarDadosComQuestionario(int respCor, int respDificuldade, string feedback)
    {
        GameplayData pacoteCompleto = new GameplayData
        {
            faseAtual = nomeFaseAtual,
            tempoSegundos = tempoFinalFase,
            tentativas = contadorTentativas,
        };

        string jsonDados = JsonUtility.ToJson(pacoteCompleto);
        Debug.Log("[MÉTRICAS] Enviando pacote unificado: " + jsonDados);

        StartCoroutine(EnviarDadosWeb(jsonDados));

        contadorTentativas = 1;
    }

    private IEnumerator EnviarDadosWeb(string jsonTexto)
    {
        using (UnityWebRequest request = new UnityWebRequest(urlEndpoint, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonTexto);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("[MÉTRICAS] Sucesso absoluto! O site recebeu os dados e o questionário.");
            }
            else
            {
                Debug.LogWarning("[MÉTRICAS] Erro ao enviar para o endpoint: " + request.error);
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}