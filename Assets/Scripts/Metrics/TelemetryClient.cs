// TelemetryClient.cs
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class TelemetryClient : MonoBehaviour
{
    public static TelemetryClient Instance;

    [Header("Configuração de Endpoints")]
    [SerializeField] private string urlEndpoint = "http://localhost:8000/api/v1/jogo/respostas";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SubmitData(GameplayData data)
    {
        string jsonPayload = JsonUtility.ToJson(data);
        Debug.Log("[TelemetryClient] Enviando pacote unificado: " + jsonPayload);

        StartCoroutine(PostRequest(jsonPayload));
    }

    private IEnumerator PostRequest(string jsonTexto)
    {
        using (UnityWebRequest request = new(urlEndpoint, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonTexto);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("[TelemetryClient] Sucesso absoluto! O site recebeu os dados e o questionário.");
            }
            else
            {
                Debug.LogWarning("[TelemetryClient] Erro ao enviar para o endpoint: " + request.error);
            }
        }
    }
}