using System;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UIElements;

public class LevelSelectionController : MonoBehaviour, IMenuPopup
{
    public Action AoFechar { get; set; }

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        Button btnVerde = root.Q<Button>("btn-fase-verde");
        Button btnVermelha = root.Q<Button>("btn-fase-vermelha");
        Button btnAmarela = root.Q<Button>("btn-fase-amarela");
        Button btnVoltar = root.Q<Button>("btn-voltar");

        ConfigurarBotao(btnVerde, () => CarregarFase("Map_Green"));
        ConfigurarBotao(btnVermelha, () => CarregarFase("Map_Red"));
        ConfigurarBotao(btnAmarela, () => CarregarFase("Map_Yellow"));

        btnVerde?.RegisterCallback<GeometryChangedEvent>(evt => btnVerde.Focus());

        if (btnVoltar != null) btnVoltar.clicked += FecharTela;
    }

    private void ConfigurarBotao(Button botao, Action acao)
    {
        if (botao == null) return;
        botao.clicked += acao;
        botao.RegisterCallback<NavigationSubmitEvent>(evt => acao.Invoke());
    }

    private void CarregarFase(string nomeDaCena)
    {
        Debug.Log($"Iniciando a cena: {nomeDaCena}");
        GlobalData.nextScene = nomeDaCena;        
        SceneManager.LoadScene("Loading");
    }

    private void FecharTela()
    {
        AoFechar?.Invoke();
        gameObject.SetActive(false);
    }
}