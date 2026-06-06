using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameplayData
{
    public string currentLevel;
    public float time;
    public int tries;   
    public int colorResponse; 
    public int difficultyResponse;
}

public class MetricsManager : MonoBehaviour
{
    public static MetricsManager Instance;

    [Header("Configuração de Endpoints")]
    [SerializeField] private string urlEndpoint = "http://localhost:8000/respostas";

    // Variáveis internas da fase atual
    private float stopWatchLevel = 0f;
    private int countTries = 1;
    private string nameCurrentLevel;
    private bool activeStopWatch = false;
    private float finalTimeLevel = 0f;

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
        if (activeStopWatch)
        {
            stopWatchLevel += Time.deltaTime;
        }
    }

    private void OnSceneLoaded(Scene cena, LoadSceneMode modo)
    {
        nameCurrentLevel = cena.name;
        stopWatchLevel = 0f;
        activeStopWatch = (nameCurrentLevel != "Main_Menu");
    }

    public void CountTries()
    {
        countTries++;
    }

    public void FinishLevelAndFreezeData()
    {
        activeStopWatch = false; 
        finalTimeLevel = Mathf.Round(stopWatchLevel * 100f) / 100f;        
        Debug.Log($"[MÉTRICAS] Gameplay congelada! Tempo: {finalTimeLevel}s | Tentativas: {countTries}");
    }

    public void SubmitDataWithSurvey(int colorScore, int difficultyScore)
    {
        GameplayData pacoteCompleto = new GameplayData
        {
            currentLevel = nameCurrentLevel,
            time = finalTimeLevel,
            tries = countTries,
            colorResponse = colorScore,
            difficultyResponse = difficultyScore,
        };

        string jsonDados = JsonUtility.ToJson(pacoteCompleto);
        Debug.Log("[MÉTRICAS] Enviando pacote unificado: " + jsonDados);

        StartCoroutine(SendData(jsonDados));

        countTries = 1;
    }

    private IEnumerator SendData(string jsonTexto)
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

    public string GetNameLevel()
    {
        return nameCurrentLevel; 
    }

    public float GetTimeLevel()
    {
        return finalTimeLevel; 
    }

    public int GetTriesLevel()
    {
        return countTries; 
    }
}