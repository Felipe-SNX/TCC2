// MetricsManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class MetricsManager : MonoBehaviour
{
    public static MetricsManager Instance;

    // Variáveis internas da fase atual
    private float stopWatchLevel = 0f;
    private int countTries = 1;
    private string nameCurrentLevel;
    private bool activeStopWatch = false;
    private float finalTimeLevel = 0f;
    private int finalTriesLevel = 0;
    private int finalCollectiblesLevel = 0;
    private int countCollectibles = 0;

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
        countCollectibles = 0;
        activeStopWatch = (nameCurrentLevel != "Main_Menu");
    }

    public void CountTries() => countTries++;
    
    public void RegisterCollectible() => countCollectibles++;

    public void FinishLevelAndFreezeData()
    {
        activeStopWatch = false; 
        finalTimeLevel = Mathf.Round(stopWatchLevel * 100f) / 100f;    
        finalTriesLevel = countTries;
        finalCollectiblesLevel = countCollectibles; 
        Debug.Log($"[MÉTRICAS] Dados congelados! Tempo: {finalTimeLevel}s | Tentativas: {finalTriesLevel} | Moedas: {finalCollectiblesLevel}");   
    }

    public void SubmitDataWithSurvey(int responseScore, string email, int pin)
    {
        GameplayData pacoteCompleto = new()
        {
            currentLevel = nameCurrentLevel,
            time = finalTimeLevel,
            tries = finalTriesLevel,
            response = responseScore,
            email = email,  
            pin = pin.ToString(),
            colectables = finalCollectiblesLevel
        };

        if (TelemetryClient.Instance != null)
        {
            TelemetryClient.Instance.SubmitData(pacoteCompleto);
        }
        else
        {
            Debug.LogError("[MÉTRICAS] TelemetryClient não encontrado na cena!");
        }

        countTries = 1;
        countCollectibles = 0;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public string GetNameLevel() => nameCurrentLevel;
    public float GetTimeLevel() => finalTimeLevel;
    public int GetTriesLevel() => finalTriesLevel;
    public int GetCollectiblesCount() => finalCollectiblesLevel;

    public LevelStats GetLevelStats()
    {
        LevelStats pacoteCompleto = new()
        {
            nameCurrentLevel = nameCurrentLevel,
            finalTimeLevel = finalTimeLevel,
            finalTriesLevel = finalTriesLevel,
            finalCollectiblesLevel = finalCollectiblesLevel
        };

        return pacoteCompleto;
    }

}