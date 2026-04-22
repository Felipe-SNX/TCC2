using UnityEngine;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class DadosSessao {
    public string idJogador;
    public string nivelExperiencia; // Novato ou Experiente se existir
    public Dictionary<string, float> tempoPorCor = new Dictionary<string, float>();
}

public class MetricsManager : MonoBehaviour {
    private DadosSessao sessaoAtual = new DadosSessao();    
    private string corAtivaAtual = "Azul";
    private float timerCor = 0f;

    void Update() {
        // Acumula o tempo na cor que está ativa no momento
        timerCor += Time.deltaTime;
    }

    public void TrocarCorRegistrada(string novaCor) {
        // Salva o tempo acumulado da cor anterior antes de trocar
        if (sessaoAtual.tempoPorCor.ContainsKey(corAtivaAtual))
            sessaoAtual.tempoPorCor[corAtivaAtual] += timerCor;
        else
            sessaoAtual.tempoPorCor.Add(corAtivaAtual, timerCor);

        corAtivaAtual = novaCor;
        timerCor = 0f; // Reinicia para a nova cor
    }

    public void SalvarDadosJSON() {
        string json = JsonUtility.ToJson(sessaoAtual, true);
        File.WriteAllText(Application.persistentDataPath + "/metricas_tcc.json", json);
        Debug.Log("Dados salvos em: " + Application.persistentDataPath);
    }
}